namespace Storage.Clients
{
    public interface IStorageClientFactory
    {
        public ImageClient GetImageClient();

        public HeaderClient GetHeaderClient();

        public AvatarClient GetAvatarClient();
    }
}
