using System;
using System.Collections.Generic;
using System.Text;
using ElearningSystem.Localization;
using Volo.Abp.Application.Services;

namespace ElearningSystem;

/* Inherit your application services from this class.
 */
public abstract class ElearningSystemAppService : ApplicationService
{
    protected ElearningSystemAppService()
    {
        LocalizationResource = typeof(ElearningSystemResource);
    }
}
