using SastImg.Domain.Repositories;

namespace SastImg.Domain.Services
{
    public class SastImgDomainService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SastImgDomainService(
            IUnitOfWork unitOfWork,
            IImageRepository imageRepository,
            IAlbumRepository albumRepository
        )
        {
            _unitOfWork = unitOfWork;
            _imageRepository = imageRepository;
            _albumRepository = albumRepository;
        }
    }
}
