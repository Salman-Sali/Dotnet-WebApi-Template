using Microsoft.Extensions.Configuration;

namespace Application.Configurations
{
    public class AwsLambdaConfigurator
    {
        public static T Bind<T>(T configurationModal, IConfiguration configuration)
        {
            var modalName = nameof(T);
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                var value = configuration.GetValue(property.PropertyType, $"{modalName}_{property.Name}");
                property.SetValue(configurationModal, value);
            }
            return configurationModal;
        }
    }
}
