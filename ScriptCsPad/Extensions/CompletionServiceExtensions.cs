using Roslyn.Services;

using ScriptCsPad.Completion;

namespace ScriptCsPad.Extensions
{
    public static class CompletionServiceExtensions
    {
        public static ICompletionService GetCompletionService(this ILanguageServiceProvider provider)
        {
            var serviceType = typeof(ILanguageService);

            var methodInfo = provider.GetMethodInfo(x => x.GetService<ILanguageService>());
            var genericMethodDefinition = methodInfo.GetGenericMethodDefinition();

            var typeName = string.Format("{0}.{1}", serviceType.Namespace, typeof(ICompletionService).Name);
            var genericMethod = genericMethodDefinition.MakeGenericMethod(serviceType.Assembly.GetType(typeName));

            var service = genericMethod.Invoke(provider, null) as ILanguageService;

            return new CompletionService(service);
        }
    }
}