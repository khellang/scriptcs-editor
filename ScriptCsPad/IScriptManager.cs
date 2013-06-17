using System.Collections.Generic;

using Roslyn.Compilers;
using Roslyn.Services;

namespace ScriptCsPad
{
    public interface IScriptManager
    {
        void SetCurrentScript(ITextContainer container);

        bool IsCompletionTriggerCharacter(int position);

        IEnumerable<CompletionItem> GetCompletion(int position, char triggerChar);
    }
}