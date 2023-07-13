using Autofac;
using Caliburn.Micro;
using EmployeeEditor.Database;
using EmployeeEditor.Domain;
using EmployeeEditor.Domain.Interfaces;
using EmployeeEditor.WpfApp.ViewModels;
using EmployeeEditor.WpfApp.Views;
using MahApps.Metro.Controls;
using System.Linq;
using System.Reflection;
using IValidator = EmployeeEditor.WpfApp.Models.Validators.IValidator;
using Module = Autofac.Module;

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

        var types = AssemblySource.Instance.ToArray();

        RegisterServices(builder, types);
        RegisterViewModels(builder, types);
        RegisterViews(builder, types);
        RegisterValidatyors(builder, types);
    }

    private static void RegisterValidatyors(ContainerBuilder builder, Assembly[] types)
    {
        builder.RegisterAssemblyTypes(types)
            .Where(type => typeof(IValidator).IsAssignableFrom(type))
            .AsSelf()
            .InstancePerDependency();
    }

    private static void RegisterViewModels(ContainerBuilder builder, Assembly[] asemblyTypes)
    {
        builder.RegisterAssemblyTypes(asemblyTypes)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .Except<MainWindowViewModel>()
            .AsSelf()
            .InstancePerDependency();

        builder.RegisterTypes(typeof(MainWindowViewModel))
            .AsSelf()
            .SingleInstance();
    }

    private static void RegisterViews(ContainerBuilder builder, Assembly[] types)
    {
        builder.RegisterAssemblyTypes(types)
            .Where(type => type.Name.EndsWith("View"))
            .Except<MainWindowView>()
            .AsSelf();

        builder.RegisterTypes(typeof(MainWindowView))
            .AsSelf()
            .As<MetroWindow>()
            .SingleInstance();
    }

    private static void RegisterServices(ContainerBuilder builder, Assembly[] types)
    {
        builder.RegisterAssemblyTypes(types)
            .Where(type => typeof(IAppService).IsAssignableFrom(type))
            .AsSelf()
            .InstancePerDependency();
    }
}