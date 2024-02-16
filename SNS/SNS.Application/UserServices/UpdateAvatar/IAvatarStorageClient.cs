﻿using Microsoft.AspNetCore.Http;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateAvatar
{
    public interface IAvatarStorageClient
    {
        public Task<Uri> UploadAvatarAsync(
            UserId userId,
            IFormFile file,
            CancellationToken cancellationToken = default
        );
    }
}
