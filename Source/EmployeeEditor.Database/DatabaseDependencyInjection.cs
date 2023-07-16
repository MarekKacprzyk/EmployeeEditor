using Autofac;
using AutoMapper;
using EmployeeEditor.Database.Map;
using EmployeeEditor.Database.Repository;
using EmployeeEditor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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

            builder.Register(context =>
                {
                    var dbContextFactory = context.Resolve<IDesignTimeDbContextFactory<EmployeeDbContext>>();
                    var dbContext = dbContextFactory.CreateDbContext(null);
                    dbContext.Database.Migrate();

                    return dbContext;
                })
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Register(ctx => new MapperConfiguration(cfg => cfg.AddProfile(new MappingEmployee())))
                .AsSelf()
                .SingleInstance();

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>()
                .CreateMapper(ctx.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}
