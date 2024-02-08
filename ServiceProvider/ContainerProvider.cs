using Microsoft.Extensions.DependencyInjection;

namespace StresslessUI.ServiceProvider
{
    public static class ContainerProvider
    {
        private static IServiceProvider _serviceProvider;

        public static void SetContainerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T Resolve<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
