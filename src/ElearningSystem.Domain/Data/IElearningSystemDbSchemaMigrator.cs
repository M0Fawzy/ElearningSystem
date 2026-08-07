using System.Threading.Tasks;

namespace ElearningSystem.Data;

public interface IElearningSystemDbSchemaMigrator
{
    Task MigrateAsync();
}
