using Roslyn.Compilers;
using Roslyn.Services;

namespace ScriptCsPad
{
    public interface IScriptWorkspace
    {
        ISolution CurrentSolution { get; }

        void SetCurrentSolution(ISolution solution);

        void OpenDocument(DocumentId documentId, ITextContainer textContainer);
    }
}