using System.Linq;
using System.Windows.Input;

using Caliburn.Micro;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;

using ScriptCsPad.Completion;

namespace ScriptCsPad.Controls
{
    public class ScriptEditor : TextEditor
    {
        private readonly IScriptManager _scriptManager;

        private CompletionWindow _completionWindow;

        public ScriptEditor()
        {
            _scriptManager = IoC.Get<IScriptManager>();

            ShowLineNumbers = true;

            TextArea.TextEntering += OnTextEntering;
            TextArea.TextEntered += OnTextEntered;
        }

        private void OnTextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length <= 0 || _completionWindow == null) return;

            if (!char.IsLetterOrDigit(e.Text[0]))
            {
                _completionWindow.CompletionList.RequestInsertion(e);
            }
        }

        private void OnTextEntered(object sender, TextCompositionEventArgs e)
        {
            if (CaretOffset <= 0) return;

            var isTrigger = _scriptManager.IsCompletionTriggerCharacter(CaretOffset - 1);
            if (!isTrigger) return;

            _completionWindow = new CompletionWindow(TextArea);

            var data = _completionWindow.CompletionList.CompletionData;

            var completion = _scriptManager.GetCompletion(CaretOffset, Text[CaretOffset - 1]).ToList();
            if (!completion.Any())
            {
                _completionWindow = null;
                return;
            }

            foreach (var completionData in completion)
            {
                data.Add(new CompletionData(completionData));
            }

            _completionWindow.Show();
            _completionWindow.Closed += (o, args) => _completionWindow = null;
        }
    }
}