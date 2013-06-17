using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

using Roslyn.Compilers;
using Roslyn.Services;

namespace ScriptCsPad.Completion
{
    public class CompletionData : ICompletionData
    {
        private readonly CompletionItem _completionItem;

        private string _description;

        public CompletionData(CompletionItem completionItem)
        {
            _completionItem = completionItem;
        }

        public ImageSource Image
        {
            get
            {
                if (!_completionItem.Glyph.HasValue) return null;

                var fileName = string.Format("{0}.png", _completionItem.Glyph).ToLowerInvariant();
                var resourceName = string.Format("ScriptCsPad.Resources.{0}", fileName);
                
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        Debug.Print(resourceName);
                        return null;
                    }

                    var bitmapImage = new BitmapImage();

                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.DecodePixelHeight = 16;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }
        }

        public string Text
        {
            get { return _completionItem.DisplayText; }
        }

        public object Content
        {
            get { return _completionItem.DisplayText; }
        }

        public object Description
        {
            get { return _description ?? (_description = _completionItem.GetDescription().ToDisplayString()); }
        }

        public double Priority { get { return 0; } }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            var offset = completionSegment.Offset - 1;
            var length = completionSegment.Length + 1;

            textArea.Document.Replace(offset, length, Text);
        }
    }
}