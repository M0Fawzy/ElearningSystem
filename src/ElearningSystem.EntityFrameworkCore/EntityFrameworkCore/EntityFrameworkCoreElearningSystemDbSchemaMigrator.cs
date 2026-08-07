using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ElearningSystem.Data;
using Volo.Abp.DependencyInjection;

namespace ElearningSystem.EntityFrameworkCore;

public class EntityFrameworkCoreElearningSystemDbSchemaMigrator
    : IElearningSystemDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreElearningSystemDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the ElearningSystemDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ElearningSystemDbContext>()
            .Database
            .MigrateAsync();
    }
}
