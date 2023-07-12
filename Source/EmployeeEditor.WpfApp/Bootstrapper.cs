using Autofac;
using Caliburn.Micro;
using EmployeeEditor.WpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace EmployeeEditor.WpfApp
{
    public class Bootstrapper : BootstrapperBase
    {
        private IContainer _container = null!;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AppDependencyInjectionModule>();
            
            _container = builder.Build();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<MainWindowViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.IsRegistered(service))
                    return _container.Resolve(service);
            }

            if (_container.IsRegisteredWithKey(key, service))
                return _container.ResolveKeyed(key, service);

            throw new Exception($"Could not locate any instances of contract {key ?? service.Name}.");
        }

        protected override IEnumerable<object>? GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Dispose();
            base.OnExit(sender, e);
        }
    }
}
