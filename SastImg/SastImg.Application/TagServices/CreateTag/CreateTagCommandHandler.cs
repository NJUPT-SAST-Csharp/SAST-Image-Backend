using Primitives.Command;
using SastImg.Domain.TagEntity;

namespace SastImg.Application.TagServices.CreateTag
{
    public sealed class CreateTagCommandHandler(ITagRepository repository)
        : ICommandRequestHandler<CreateTagCommand, TagDto>
    {
        private readonly ITagRepository _repository = repository;

        public async Task<TagDto> Handle(
            CreateTagCommand request,
            CancellationToken cancellationToken
        )
        {
            var tag = Tag.CreateNewTag(request.Name);
            var id = await _repository.AddTagAsync(tag, cancellationToken);
            return new() { Id = id.Value, Name = request.Name };
        }
    }
}
