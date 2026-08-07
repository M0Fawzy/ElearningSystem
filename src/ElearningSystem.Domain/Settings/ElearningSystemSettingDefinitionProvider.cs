using Volo.Abp.Settings;

namespace ElearningSystem.Settings;

public class ElearningSystemSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ElearningSystemSettings.MySetting1));
    }
}
