﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Storage.Clients;
using Storage.Options;

namespace Shared.Storage.Configurations
{
    public static class StorageConfiguration
    {
        public static IServiceCollection AddStorageClient(
            this IServiceCollection services,
            Action<StorageOptions>? storageOptionsBuilder = null
        )
        {
            StorageOptions options = new();
            storageOptionsBuilder?.Invoke(options);

            services.TryAddSingleton(options);
            services.TryAddSingleton<IProcessClient, ProcessClient>();
            services.TryAddSingleton<IStorageClient, StorageClient>();

            return services;
        }
    }
}
