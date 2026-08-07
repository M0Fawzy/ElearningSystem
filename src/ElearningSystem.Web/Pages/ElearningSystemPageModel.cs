using ElearningSystem.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace ElearningSystem.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ElearningSystemPageModel : AbpPageModel
{
    protected ElearningSystemPageModel()
    {
        LocalizationResourceType = typeof(ElearningSystemResource);
    }
}
