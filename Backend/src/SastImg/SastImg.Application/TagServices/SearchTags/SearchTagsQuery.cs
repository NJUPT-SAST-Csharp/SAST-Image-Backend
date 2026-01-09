using Mediator;

namespace SastImg.Application.TagServices.SearchTags;

public sealed record class SearchTagsQuery(string SearchName) : IQuery<IEnumerable<TagDto>>;
