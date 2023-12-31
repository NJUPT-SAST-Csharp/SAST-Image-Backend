namespace SastImg.Application.TagServices
{
    public sealed class TagDto
    {
        private TagDto() { }

        public required long Id { get; init; }
        public required string Name { get; init; }
    }
}
