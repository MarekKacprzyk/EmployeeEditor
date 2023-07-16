using System.Reflection;
using Autofac;
using AutoMapper;
using EmployeeEditor.Database.Map;
using EmployeeEditor.Database.Repository;
using EmployeeEditor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Module = Autofac.Module;

namespace EmployeeEditor.Database
{
    public class DatabaseDependencyInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmployeeDbContextFactory>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<EmployeeRepository>()
                .As<IEmployeeRepository>()
                .InstancePerDependency();

            builder.RegisterType<TagRepository>()
                .As<ITagRepository>()
                .InstancePerDependency();

            builder.Register(context =>
                {
                    var dbContextFactory = context.Resolve<IDesignTimeDbContextFactory<EmployeeDbContext>>();
                    var dbContext = dbContextFactory.CreateDbContext(null);
                    dbContext.Database.Migrate();

                    return dbContext;
                })
                .AsSelf()
                .InstancePerLifetimeScope();

            var assembly = Assembly.GetAssembly(GetType())?.GetTypes();
            var profiles = assembly?.Where(t => typeof(Profile).IsAssignableFrom(t)).ToArray();

            builder.RegisterTypes(profiles)
                .AsSelf()
                .As<Profile>()
                .InstancePerDependency();

            builder.Register(ctx => new MapperConfiguration(cfg =>
                {
                    var resolveProfiles = ctx.Resolve<IEnumerable<Profile>>().ToList();
                    resolveProfiles.ForEach(cfg.AddProfile);
                }))
                .AsSelf()
                .SingleInstance();

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>()
                .CreateMapper(ctx.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}
