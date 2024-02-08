using Primitives.Rule;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Rules
{
    internal readonly struct RestoreImageOnlyWhenAlbumNotRemovedRule(bool isAlbumRemoved)
        : IDomainBusinessRule
    {
        public bool IsBroken => isAlbumRemoved == true;

        public string Message => "Couldn't restore image when its album is removed.";
    }
}
