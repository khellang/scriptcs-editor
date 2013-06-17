using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

using ScriptCsPad.Completion;

namespace ScriptCsPad
{
    public class ScriptManager : IScriptManager
    {
        private static readonly Type[] AssemblyTypes = new[] { typeof(object), typeof(Uri), typeof(Enumerable) };

        private readonly ICompletionService _completionService;

        private readonly IScriptWorkspace _scriptWorkspace;

        private readonly CompilationOptions _compilationOptions;

        private readonly ParseOptions _parseOptions;

        private readonly IEnumerable<PortableExecutableReference> _references;

        private DocumentId _currentDocumentId;

        private ProjectId _previousProjectId;

        private int _documentNumber;

        public ScriptManager(ICompletionService completionService, IScriptWorkspace scriptWorkspace)
        {
            _completionService = completionService;
            _scriptWorkspace = scriptWorkspace;

            _compilationOptions = new CompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            _parseOptions = new ParseOptions(CompatibilityMode.None, LanguageVersion.CSharp6, true, SourceCodeKind.Script);

            var metadataFileProvider = _scriptWorkspace.CurrentSolution.MetadataFileProvider;
            _references = AssemblyTypes.Select(type => GetReference(metadataFileProvider, type));
        }

        private static PortableExecutableReference GetReference(MetadataFileProvider metadataFileProvider, Type t)
        {
            return metadataFileProvider.GetReference(t.Assembly.Location, MetadataReferenceProperties.Assembly);
        }

        private IDocument CurrentScript
        {
            get { return _scriptWorkspace.CurrentSolution.GetDocument(_currentDocumentId); }
        }

        private static IText UsingText
        {
            get
            {
                var usingStatements = AssemblyTypes.Select(t => string.Format("using {0};", t.Namespace));
                return new StringText(string.Join(Environment.NewLine, usingStatements));
            }
        }

        public void SetCurrentScript(ITextContainer container)
        {
            var currentSolution = _scriptWorkspace.CurrentSolution;

            IProject project;
            if (_previousProjectId == null)
            {
                DocumentId id;
                project = CreateSubmissionProject(currentSolution);
                currentSolution = project.Solution.AddDocument(project.Id, project.Name, UsingText, out id);
                _previousProjectId = project.Id;
            }

            project = CreateSubmissionProject(currentSolution);
            var currentDocument = SetSubmissionDocument(container, project);
            _currentDocumentId = currentDocument.Id;
        }

        private IProject CreateSubmissionProject(ISolution solution)
        {
            var name = "Submission#" + _documentNumber++;
            var projectId = ProjectId.CreateNewId(solution.Id, name);

            var version = VersionStamp.Create();
            var compilationOptions = _compilationOptions.WithScriptClassName(name);

            var projectInfo = new ProjectInfo(
                projectId,
                version,
                name,
                name,
                LanguageNames.CSharp,
                compilationOptions: compilationOptions,
                parseOptions: _parseOptions,
                metadataReferences: _references,
                isSubmission: true);

            solution = solution.AddProject(projectInfo);

            if (_previousProjectId != null)
            {
                solution = solution.AddProjectReference(projectId, _previousProjectId);
            }

            return solution.GetProject(projectId);
        }

        private IDocument SetSubmissionDocument(ITextContainer textContainer, IProject project)
        {
            DocumentId id;
            var solution = project.Solution.AddDocument(project.Id, project.Name, textContainer.CurrentText, out id);

            _scriptWorkspace.SetCurrentSolution(solution);
            _scriptWorkspace.OpenDocument(id, textContainer);

            return solution.GetDocument(id);
        }

        public bool IsCompletionTriggerCharacter(int position)
        {
            var currentScriptText = CurrentScript.GetText();
            var completionProviders = _completionService.GetDefaultCompletionProviders();

            return _completionService.IsTriggerCharacter(currentScriptText, position, completionProviders);
        }

        public IEnumerable<CompletionItem> GetCompletion(int position, char triggerChar)
        {
            var completionTrigger = CompletionTriggerInfo.CreateTypeCharTriggerInfo(triggerChar);
            var completionProviders = _completionService.GetDefaultCompletionProviders();

            var groups = _completionService.GetGroups(
                CurrentScript, 
                position, 
                completionTrigger, 
                completionProviders, 
                CancellationToken.None);

            if (groups == null) yield break;

            foreach (var completionItem in groups.SelectMany(x => x.Items))
            {
                yield return completionItem;
            }
        }
    }
}