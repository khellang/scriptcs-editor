using Roslyn.Compilers;
using Roslyn.Services;
using Roslyn.Services.Host;

namespace ScriptCsPad
{
    public class ScriptWorkspace : TrackingWorkspace, IScriptWorkspace
    {
        public ScriptWorkspace(IWorkspaceServiceProvider workspaceServiceProvider)
            : base(workspaceServiceProvider, true, true) { }

        public override bool IsSupported(WorkspaceFeature feature)
        {
            return feature == WorkspaceFeature.OpenDocument || feature == WorkspaceFeature.UpdateDocument;
        }

        public void SetCurrentSolution(ISolution solution)
        {
            SetLatestSolution(solution);
            RaiseWorkspaceChangedEventAsync(WorkspaceEventKind.SolutionChanged, solution);
        }

        public void OpenDocument(DocumentId documentId, ITextContainer textContainer)
        {
            OnDocumentOpened(documentId, textContainer);
        }
    }
}