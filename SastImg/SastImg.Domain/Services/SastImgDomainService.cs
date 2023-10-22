using SastImg.Domain.Repositories;

namespace SastImg.Domain.Services
{
    public class SastImgDomainService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unit;

        public SastImgDomainService(
            IUnitOfWork unit,
            IAlbumRepository albumRepository,
            ITagRepository tagRepository,
            ICategoryRepository categoryRepository
        )
        {
            _unit = unit;
            _albumRepository = albumRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }
    }
}
