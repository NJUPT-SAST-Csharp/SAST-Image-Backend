using Primitives.Rule;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Rules
{
    internal readonly struct ActionAllowedOnlyWhenNotArchivedRule(bool isArchived)
        : IDomainBusinessRule
    {
        public bool IsBroken { get; } = isArchived;

        public string Message { get; } = "Action is allowed only when album is not archived.";
    }
}
