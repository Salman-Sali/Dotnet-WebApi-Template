using Microsoft.Extensions.Configuration;

namespace Application.Configurations
{
    public class AwsLambdaConfigurator
    {
        public static void Bind<T>(T configurationModal, IConfiguration configuration)
        {
            var isLocal = configuration.GetValue<bool?>("Local");
            if (isLocal.HasValue && isLocal.Value)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = configuration.GetValue(property.PropertyType, $"{nameof(T)}_{property.Name}");
                    property.SetValue(configurationModal, value);
                }
            }
            else
            {
                configuration.GetSection(nameof(T)).Bind(configurationModal);
            }
        }
    }
}
