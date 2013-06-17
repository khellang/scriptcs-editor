using System.IO;
using System.Windows.Forms;

using Caliburn.Micro;

using ICSharpCode.AvalonEdit.Highlighting;

using ScriptCsPad.Controls;

namespace ScriptCsPad.ViewModels
{
    public sealed class ShellViewModel : Conductor<ScriptEditorViewModel>.Collection.OneActive
    {
        private readonly IHighlightingDefinition _highlighting;

        private readonly IScriptManager _scriptManager;

        public ShellViewModel(IHighlightingDefinition highlighting, IScriptManager scriptManager)
        {
            _highlighting = highlighting;
            _scriptManager = scriptManager;
            DisplayName = "ScriptCsPad";
            StatusBar = new StatusBarViewModel();
        }

        public StatusBarViewModel StatusBar { get; set; }

        protected override void ChangeActiveItem(ScriptEditorViewModel newItem, bool closePrevious)
        {
            _scriptManager.SetCurrentScript(newItem);
            base.ChangeActiveItem(newItem, closePrevious);
        }

        public void New()
        {
            var scriptEditor = new ScriptEditorViewModel("untitled", string.Empty, _highlighting);
            Items.Add(scriptEditor);
            ActivateItem(scriptEditor);
        }

        public void Open()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "C# Scripts|*.csx",
                CheckFileExists = true,
                Multiselect = false
            };

            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) return;

            var fileName = Path.GetFileName(dialog.FileName);
            var content = File.ReadAllText(dialog.FileName);

            var scriptEditor = new ScriptEditorViewModel(fileName, content, _highlighting);
            Items.Add(scriptEditor);
            ActivateItem(scriptEditor);
        }
    }
}