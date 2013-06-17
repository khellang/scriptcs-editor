using System;
using System.Collections.Generic;
using System.Diagnostics;

using Autofac;

using Caliburn.Micro;

using ScriptCsPad.ViewModels;

using IContainer = Autofac.IContainer;

namespace ScriptCsPad
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private IContainer _container;

        protected override void Configure()
        {
            LogManager.GetLog = type => new DebugLogger(type);

            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            _container = builder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                object instance;
                if (_container.TryResolve(service, out instance))
                    return instance;
            }
            else
            {
                object instance;
                if (_container.TryResolveNamed(key, service, out instance))
                    return instance;
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        private static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<PresentationModule>();
            builder.RegisterModule<AutosubscriberModule>();
        }
    }

    public class DebugLogger : ILog
    {
        private readonly Type _type;

        public DebugLogger(Type type)
        {
            _type = type;
        }

        public void Info(string format, params object[] args)
        {
            LogMessage("INFO", string.Format(format, args));
        }

        public void Warn(string format, params object[] args)
        {
            LogMessage("WARN", string.Format(format, args));
        }

        public void Error(Exception exception)
        {
            LogMessage("ERROR", string.Format("{0}: {1}", exception.GetType().Name, exception.Message));
        }

        private void LogMessage(string level, string message)
        {
            Debug.WriteLine("[{0} - {1} - {2}] - {3}", DateTime.Now, level, _type.Name, message);
        }
    }
}