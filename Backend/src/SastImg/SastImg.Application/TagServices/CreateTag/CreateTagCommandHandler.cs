using Mediator;
using SastImg.Domain.AlbumTagEntity;

namespace SastImg.Application.TagServices.CreateTag;

public sealed class CreateTagCommandHandler(ITagRepository repository)
    : ICommandHandler<CreateTagCommand, TagDto>
{
    private readonly ITagRepository _repository = repository;

    public async ValueTask<TagDto> Handle(
        CreateTagCommand request,
        CancellationToken cancellationToken
    )
    {
        var tag = ImageTag.CreateNewTag(request.Name);
        var id = await _repository.AddTagAsync(tag, cancellationToken);
        return new() { Id = id.Value, Name = request.Name.Value };
    }
}
