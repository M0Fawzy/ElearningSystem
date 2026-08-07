using Microsoft.Extensions.Localization;
using ElearningSystem.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ElearningSystem.Web;

[Dependency(ReplaceServices = true)]
public class ElearningSystemBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ElearningSystemResource> _localizer;

    public ElearningSystemBrandingProvider(IStringLocalizer<ElearningSystemResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
