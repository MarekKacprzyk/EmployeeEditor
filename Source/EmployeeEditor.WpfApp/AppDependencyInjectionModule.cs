using System.Linq;
using System.Reflection;
using Autofac;
using Caliburn.Micro;
using EmployeeEditor.Database;
using EmployeeEditor.Domain;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.Domain.Interfaces;
using EmployeeEditor.WpfApp.ViewModels;
using EmployeeEditor.WpfApp.Views;
using FluentValidation;
using MahApps.Metro.Controls;
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
    }

    private static void RegisterViewModels(ContainerBuilder builder, Assembly[] asemblyTypes)
    {
        builder.RegisterAssemblyTypes(asemblyTypes)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .Except<MainWindowViewModel>()
            .Except<EditEmployeeViewModel>()
            .AsSelf()
            .InstancePerDependency();


        builder.RegisterTypes(typeof(MainWindowViewModel))
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<EditEmployeeViewModel>()
            .AsSelf()
            .InstancePerDependency();

        builder.RegisterType<EmployeeValidator>()
            .As<AbstractValidator<EmployeeDto>>()
            .AsSelf()
            .InstancePerDependency();
        //builder.Register((c, p) =>
        //    {
        //        var validator = c.Resolve<EmployeeValidator>();
        //        var employee = p.TypedAs<EmployeeDto>();
        //        return new EditEmployeeViewModel(employee, validator);
        //    })
        //    .AsSelf()
        //    .InstancePerRequest();
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