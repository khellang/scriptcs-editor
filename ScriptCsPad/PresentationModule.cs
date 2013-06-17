using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;

using Autofac;

using Caliburn.Micro;

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using Roslyn.Compilers;
using Roslyn.Services;
using Roslyn.Services.Host;

using ScriptCs;

using ScriptCsPad.Completion;
using ScriptCsPad.Extensions;

using Module = Autofac.Module;

namespace ScriptCsPad
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register ViewModels
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("ViewModel"))
                   .Where(type => NamespaceEndsWith(type, "ViewModels"))
                   .Where(type => !type.IsAbstract)
                   .AssignableTo<INotifyPropertyChanged>()
                   .AsSelf()
                   .InstancePerDependency();

            // Register Views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("View"))
                   .Where(type => NamespaceEndsWith(type, "Views"))
                   .AsSelf()
                   .InstancePerDependency();

            var workspaceServiceProvider = DefaultServices.WorkspaceServicesFactory.CreateWorkspaceServiceProvider("RoslynPad");
            builder.RegisterInstance(workspaceServiceProvider).As<IWorkspaceServiceProvider>();

            builder.Register(c => c.Resolve<IScriptWorkspace>().CurrentSolution.LanguageServicesFactory
                .CreateLanguageServiceProvider(LanguageNames.CSharp).GetCompletionService()).As<ICompletionService>();

            builder.RegisterType<ScriptManager>().As<IScriptManager>().SingleInstance();
            builder.RegisterType<ScriptWorkspace>().As<IScriptWorkspace>().SingleInstance();

            builder.RegisterType<NullLogger>().As<Common.Logging.ILog>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<FilePreProcessor>().As<IFilePreProcessor>();

            builder.Register(c => HighlightingManager.Instance).As<IHighlightingDefinitionReferenceResolver>().SingleInstance();
            builder.Register(c => LoadHighlightingDefinition(c.Resolve<IHighlightingDefinitionReferenceResolver>())).SingleInstance();

            builder.Register<IWindowManager>(c => new MetroWindowManager()).SingleInstance();
            builder.Register<IEventAggregator>(c => new EventAggregator()).SingleInstance();
        }

        private static IHighlightingDefinition LoadHighlightingDefinition(IHighlightingDefinitionReferenceResolver referenceResolver)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("ScriptCsPad.Resources.ScriptCs.xshd"))
            {
                if (stream == null) throw new InvalidOperationException("Could not find highlighting definition.");

                using (var reader = XmlReader.Create(stream))
                {
                    return HighlightingLoader.Load(reader, referenceResolver);
                }
            }
        }

        private static bool NamespaceEndsWith(Type type, string value)
        {
            return !string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.EndsWith(value);
        }
    }
}