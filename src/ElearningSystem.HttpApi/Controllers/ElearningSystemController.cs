using ElearningSystem.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ElearningSystem.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ElearningSystemController : AbpControllerBase
{
    protected ElearningSystemController()
    {
        LocalizationResource = typeof(ElearningSystemResource);
    }
}
