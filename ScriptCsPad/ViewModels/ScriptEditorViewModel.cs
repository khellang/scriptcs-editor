using System;
using System.ComponentModel;

using Caliburn.Micro;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;

using Roslyn.Compilers;

namespace ScriptCsPad.ViewModels
{
    public class ScriptEditorViewModel : Screen, ITextContainer
    {
        private TextDocument _document;

        private bool _isModified;

        private string _name;

        public ScriptEditorViewModel(string name, string content, IHighlightingDefinition highlighting)
        {
            Name = name;
            Content = content;
            CurrentText = new StringText(content);
            Highlighting = highlighting;
            References = new BindableCollection<string>();
            UsingStatements = new BindableCollection<string>();
        }

        private IText OldText { get; set; }

        public IText CurrentText { get; private set; }

        public event EventHandler<TextChangeEventArgs> TextChanged;

        public IHighlightingDefinition Highlighting { get; set; }

        public BindableCollection<string> References { get; set; }

        public BindableCollection<string> UsingStatements { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public TextDocument Document
        {
            get { return _document; }
            set
            {
                if (Equals(value, _document)) return;
                _document = value;
                NotifyOfPropertyChange(() => Document);
            }
        }

        public string Content
        {
            get { return Document.Text; }
            set
            {
                if (Document == null)
                {
                    Document = new TextDocument(value);
                    Document.UndoStack.PropertyChanged += OnUndoStackPropertyChanged;
                    Document.Changing += OnDocumentChanging;
                    Document.Changed += OnDocumentChanged;
                }
                else
                {
                    Document.Text = value;
                }
            }
        }

        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                if (value.Equals(_isModified)) return;
                _isModified = value;
                NotifyOfPropertyChange(() => IsModified);

                if (value)
                {
                    Document.UndoStack.MarkAsOriginalFile();
                }
            }
        }

        protected virtual void OnTextChanged(TextChangeEventArgs e)
        {
            var handler = TextChanged;
            if (handler != null) handler(this, e);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close && Document != null)
            {
                Document.UndoStack.PropertyChanged -= OnUndoStackPropertyChanged;
                Document.Changing -= OnDocumentChanging;
                Document.Changed -= OnDocumentChanged;
            }

            base.OnDeactivate(close);
        }

        private void OnDocumentChanging(object sender, DocumentChangeEventArgs e)
        {
            OldText = CurrentText;
        }

        private void OnDocumentChanged(object sender, DocumentChangeEventArgs e)
        {
            CurrentText = new StringText(Document.Text);
            OnTextChanged(new TextChangeEventArgs(OldText, CurrentText, new TextChangeRange[0]));
        }

        private void OnUndoStackPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOriginalFile")
            {
                IsModified = !Document.UndoStack.IsOriginalFile;
            }
        }
    }
}