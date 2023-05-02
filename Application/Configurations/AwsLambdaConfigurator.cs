using Microsoft.Extensions.Configuration;

namespace Application.Configurations
{
    public class AwsLambdaConfigurator
    {
        public static T Bind<T>(T configurationModal, IConfiguration configuration)
        {
            var isLocal = configuration.GetValue<bool?>("Local");
            if (isLocal.HasValue && isLocal.Value)
            {
                configuration.GetSection(typeof(T).Name).Bind(configurationModal);
            }
            else
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = configuration.GetValue(property.PropertyType, $"{typeof(T).Name}_{property.Name}");
                    property.SetValue(configurationModal, value);
                }
            }
            return configurationModal;
        }
    }
}
