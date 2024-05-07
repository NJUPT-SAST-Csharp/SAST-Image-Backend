﻿namespace Storage.Clients
{
    public interface IProcessClient
    {
        public Task<Uri> CompressImageAsync(
            string key,
            CancellationToken cancellationToken = default
        );

        public ValueTask<string> GetExtensionNameAsync(
            Stream file,
            CancellationToken cancellationToken = default
        );
    }
}
