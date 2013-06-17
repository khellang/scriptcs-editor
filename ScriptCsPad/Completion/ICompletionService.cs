using System.Collections.Generic;
using System.Threading;

using Roslyn.Compilers;
using Roslyn.Services;

namespace ScriptCsPad.Completion
{
    public interface ICompletionService
    {
        IEnumerable<ICompletionProvider> GetDefaultCompletionProviders();

        TextSpan GetDefaultTrackingSpan(IDocument document, int position, CancellationToken cancellationToken);

        IEnumerable<CompletionItemGroup> GetGroups(
            IDocument document,
            int position,
            CompletionTriggerInfo triggerInfo,
            IEnumerable<ICompletionProvider> completionProviders,
            CancellationToken cancellationToken);

        bool IsTriggerCharacter(IText text, int characterPosition, IEnumerable<ICompletionProvider> completionProviders);
    }
}