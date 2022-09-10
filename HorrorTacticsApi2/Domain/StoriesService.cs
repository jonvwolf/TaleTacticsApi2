using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Dtos;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Stories;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class StoriesService
    {
        readonly IHorrorDbContext _context;
        readonly StoryModelEntityHandler _imeHandler;
        
        public StoriesService(IHorrorDbContext context, StoryModelEntityHandler handler)
        {
            _context = context;
            _imeHandler = handler;
        }

        public async Task<IList<ReadStoryModel>> GetAllStoriesAsync(CancellationToken token)
        {
            var list = new List<ReadStoryModel>();
            var images = await GetQuery(true).ToListAsync(token);
            images.ForEach(image => { list.Add(_imeHandler.CreateReadModel(image)); });

            return list;
        }

        public async Task<ReadStoryModel?> TryGetAsync(long id, CancellationToken token)
        {
            var entity = await TryFindStoryAsync(id, true, token);
            return entity == default ? default : _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStoryModel> CreateStoryAsync(UserJwt user, CreateStoryModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);

            var entity = _imeHandler.CreateEntity(user, model);
            _context.Stories.Add(entity);
            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task<ReadStoryModel> UpdateStoryAsync(long id, UpdateStoryModel model, bool basicValidated, CancellationToken token)
        {
            _imeHandler.Validate(model, basicValidated);
            // TODO: improve includeAll performance (does it really need to include all references when Put?)
            // - Even better yet... why return data when updating?
            var entity = await TryFindStoryAsync(id, true, token);
            if (entity == default)
                throw new HtNotFoundException($"Story with Id {id} not found");

            _imeHandler.UpdateEntity(model, entity);

            await _context.SaveChangesWrappedAsync(token);

            return _imeHandler.CreateReadModel(entity);
        }

        public async Task DeleteStoryAsync(long id, CancellationToken token)
        {
            var entity = await TryFindStoryAsync(id, false, token);
            if (entity == default)
                throw new HtNotFoundException($"Story with Id {id} not found");

            _context.Stories.Remove(entity);
            await _context.SaveChangesWrappedAsync(token);
        }

        public async Task<StoryEntity?> TryFindStoryAsync(long id, bool includeAll, CancellationToken token)
        {
            var entity = await GetQuery(includeAll).SingleOrDefaultAsync(x => x.Id == id, token);

            return entity;
        }

        IQueryable<StoryEntity> GetQuery(bool includeAll = true)
        {
            IQueryable<StoryEntity> query = _context.Stories;

            if (includeAll)
            {
                // TODO: this should be organized (code)
                query = query
                        .Include(x => x.Scenes)
                            .ThenInclude(x=>x.Commands)
                                .ThenInclude(x => x.Audios)
                                .ThenInclude(x => x.File)
                        .Include(x => x.Scenes)
                            .ThenInclude(x => x.Commands)
                                .ThenInclude(x => x.Images)
                                .ThenInclude(x => x.File)
                    ;
            }
            return query;
        }
    }
}
