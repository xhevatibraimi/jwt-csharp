using Microsoft.Extensions.DependencyInjection;

namespace JwtDemo.WebDemo.Helpers
{
    public static class DataSeedExtensions
    {
        public static void SeedData(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var dataSeedService = serviceProvider.GetRequiredService<IDataSeedService>();
            dataSeedService.SeedDataAsync().GetAwaiter().GetResult();
        }
    }
}
