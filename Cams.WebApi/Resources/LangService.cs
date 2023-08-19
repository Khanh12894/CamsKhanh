using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace XichLip.WebApi.Resources
{
    public class LangService
    {
        private readonly IStringLocalizer _localizer;
        private IConfiguration _configuration;
        public LangService(IStringLocalizerFactory factory, IConfiguration configuration)
        {
            _configuration = configuration;
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString Text(string key)
        {
            return _localizer.WithCulture(new System.Globalization.CultureInfo(_configuration["AppSettings:Culture"]))[key];
        }
    }
}
