using ElearningSystem.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ElearningSystem.Permissions;

public class ElearningSystemPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ElearningSystemPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ElearningSystemPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ElearningSystemResource>(name);
    }
}
