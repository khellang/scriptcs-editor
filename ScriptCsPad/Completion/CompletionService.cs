using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using Roslyn.Compilers;
using Roslyn.Services;

namespace ScriptCsPad.Completion
{
    public class CompletionService : ICompletionService
    {
        private static readonly ConcurrentDictionary<MethodBase, MethodBase> MethodMap =
            new ConcurrentDictionary<MethodBase, MethodBase>();

        private readonly ILanguageService _languageService;

        private readonly Type _languageServiceType;

        public CompletionService(ILanguageService languageService)
        {
            _languageService = languageService;
            _languageServiceType = languageService.GetType();
        }

        public IEnumerable<ICompletionProvider> GetDefaultCompletionProviders()
        {
            return InvokeWrappedMethod<IEnumerable<ICompletionProvider>>(MethodBase.GetCurrentMethod());
        }

        public TextSpan GetDefaultTrackingSpan(IDocument document, int position, CancellationToken cancellationToken)
        {
            return InvokeWrappedMethod<TextSpan>(MethodBase.GetCurrentMethod(), document, position, cancellationToken);
        }

        public IEnumerable<CompletionItemGroup> GetGroups(
            IDocument document,
            int position,
            CompletionTriggerInfo triggerInfo,
            IEnumerable<ICompletionProvider> completionProviders,
            CancellationToken cancellationToken)
        {
            return InvokeWrappedMethod<IEnumerable<CompletionItemGroup>>(
                MethodBase.GetCurrentMethod(), document, position, triggerInfo, completionProviders, cancellationToken);
        }

        public bool IsTriggerCharacter(IText text, int characterPosition, IEnumerable<ICompletionProvider> completionProviders)
        {
            return InvokeWrappedMethod<bool>(MethodBase.GetCurrentMethod(), text, characterPosition, completionProviders);
        }

        private T InvokeWrappedMethod<T>(MethodBase wrapperMethod, params object[] args)
        {
            var wrappedMethod = MethodMap.GetOrAdd(wrapperMethod, GetWrappedMethod);
            return (T) wrappedMethod.Invoke(_languageService, args);
        }

        private MethodBase GetWrappedMethod(MethodBase wrapperMethod)
        {
            var parameters = wrapperMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            return _languageServiceType.GetMethod(wrapperMethod.Name, parameters);
        }
    }
}