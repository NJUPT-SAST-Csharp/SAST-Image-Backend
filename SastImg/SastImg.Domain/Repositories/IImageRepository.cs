using SastImg.Domain.Entities;

namespace SastImg.Domain.Repositories
{
    public interface IImageRepository
    {
        public Task<Image> GetImageByIdAsync(long id);

        public Task<int> DeleteAllMarkedImages();

        public Task RemoveImageByIdAsync(long id);

        public Task UpdateImageInfoById(long id);
    }
}
