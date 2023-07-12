using System.Linq;
using Autofac;
using Caliburn.Micro;
using EmployeeEditor.Database;
using EmployeeEditor.Domain;
using EmployeeEditor.WpfApp.ViewModels;
using EmployeeEditor.WpfApp.Views;

namespace EmployeeEditor.WpfApp;

public class AppDependencyInjectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<DatabaseDependencyInjection>();
        builder.RegisterModule<DomainDependencyInjection>();

        builder.RegisterType<EventAggregator>()
            .As<IEventAggregator>()
            .SingleInstance();

        builder.RegisterType<WindowManager>()
            .As<IWindowManager>()
            .SingleInstance();

        // Rejestruj widoki
        builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
            .Where(type => type.Name.EndsWith("View"))
            .Except<MainWindowView>()
            .AsSelf();

        // Rejestruj viewmodele
        builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
            .Where(type => type.Name.EndsWith("ViewModel"))
            .Except<MainWindowViewModel>()
            .AsSelf()
            .InstancePerDependency();

        builder.RegisterTypes(typeof(MainWindowView), typeof(MainWindowViewModel))
            .AsSelf()
            .SingleInstance();
    }
}