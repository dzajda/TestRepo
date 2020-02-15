using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tesssst
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<StorageHealthCheck> logger;

        public StorageHealthCheck(ILogger<StorageHealthCheck> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var connectionString = configuration["StorageConnectionString"];
                var containerName = configuration["StorageBlobContainer"];

                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var blobContainer = blobClient.GetContainerReference(containerName);

                if (!await blobContainer.ExistsAsync())
                {
                    return HealthCheckResult.Unhealthy("dupa");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            logger.LogInformation("działa");
            return HealthCheckResult.Healthy("działa");
        }
    }
}
