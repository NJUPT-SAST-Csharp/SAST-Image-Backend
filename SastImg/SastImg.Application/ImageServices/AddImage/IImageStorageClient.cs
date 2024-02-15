﻿using Microsoft.AspNetCore.Http;

namespace SastImg.Application.ImageServices.AddImage
{
    public interface IImageStorageClient
    {
        public Task<Uri> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        );
    }
}
