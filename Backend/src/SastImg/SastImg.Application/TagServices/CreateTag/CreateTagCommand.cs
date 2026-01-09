using Mediator;
using SastImg.Domain.AlbumTagEntity;

namespace SastImg.Application.TagServices.CreateTag;

public sealed record class CreateTagCommand(TagName Name) : ICommand<TagDto>;
