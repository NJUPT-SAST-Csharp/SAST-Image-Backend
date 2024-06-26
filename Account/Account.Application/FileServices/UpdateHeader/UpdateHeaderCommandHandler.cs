﻿using Account.Domain.UserEntity.Services;
using Primitives;
using Primitives.Command;

namespace Account.Application.FileServices.UpdateHeader
{
    internal class UpdateHeaderCommandHandler(
        IUserRepository repository,
        IHeaderStorageRepository storage,
        IUnitOfWork unit
    ) : ICommandRequestHandler<UpdateHeaderCommand>
    {
        private readonly IUnitOfWork _unit = unit;
        private readonly IUserRepository _repository = repository;
        private readonly IHeaderStorageRepository _storage = storage;

        public async Task Handle(UpdateHeaderCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(request.Requester.Id, cancellationToken);

            if (request.Header is not null)
            {
                var url = await _storage.UploadHeaderAsync(
                    user.Id,
                    request.Header,
                    cancellationToken
                );

                user.UpdateHeader(url);
            }
            else
            {
                user.UpdateHeader(null);
            }

            await _unit.CommitChangesAsync(cancellationToken);
        }
    }
}
