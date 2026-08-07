using ElearningSystem.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ElearningSystem.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ElearningSystemEntityFrameworkCoreModule),
    typeof(ElearningSystemApplicationContractsModule)
    )]
public class ElearningSystemDbMigratorModule : AbpModule
{
}
