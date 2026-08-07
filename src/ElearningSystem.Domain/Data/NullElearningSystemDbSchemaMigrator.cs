using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ElearningSystem.Data;

/* This is used if database provider does't define
 * IElearningSystemDbSchemaMigrator implementation.
 */
public class NullElearningSystemDbSchemaMigrator : IElearningSystemDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
