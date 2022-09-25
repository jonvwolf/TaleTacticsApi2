using HorrorTacticsApi2.Data;
using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Exceptions;
using HorrorTacticsApi2.Domain.Handlers;
using HorrorTacticsApi2.Domain.Models.Audio;
using HorrorTacticsApi2.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace HorrorTacticsApi2.Domain
{
    public class UserService
    {
        readonly IHorrorDbContext _context;
        readonly UserModelEntityHandler _handler;
        readonly PasswordHelper _passwordHelper;
        readonly DefaultStoryCreatorService _creatorService;
        readonly ILogger<UserService> _logger;

        public UserService(IHorrorDbContext context, UserModelEntityHandler handler, PasswordHelper passwordHelper, DefaultStoryCreatorService creatorService,
            ILogger<UserService> logger)
        {
            _context = context;
            _handler = handler;
            _passwordHelper = passwordHelper;
            _creatorService = creatorService;
            _logger = logger;
        }

        public async Task<ReadUserModel?> TryGetAsync(string password, string username, CancellationToken token, bool updateLastLogin = false)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username, token);
            if (user == default)
                return default;

            var pw = _passwordHelper.GenerateHash(password, user.Salt);

            if (user.Password.SequenceEqual(pw))
            {
                if (updateLastLogin)
                {
                    _handler.UpdateLastLogin(user);
                    await _context.SaveChangesWrappedAsync(token);
                }
                return _handler.CreateReadModel(user);
            }

            return default;
        }

        public async Task<ReadUserModel?> TryGetAsync(long id, CancellationToken token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);
            if (user == default)
                return default;

            return _handler.CreateReadModel(user);
        }

        public UserEntity GetReference(UserJwt user)
        {
            var entity = _handler.GetEntityForReference(user);
            _context.Users.Attach(entity);
            return entity;
        }

        public async Task<ReadUserModel> CreateAsync(CreateUserModel model, CancellationToken token)
        {
            var entity = _handler.CreateEntity(model);

            _context.Users.Add(entity);
            await _context.SaveChangesWrappedAsync(token);

            try
            {
                // Do not pass token as we don't want to cancel.
                // Do whatever it is possible to create the default story for the user
                await _creatorService.CreateAsync(entity, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't create default story for user: {userId}", entity.Id);
            }

            return _handler.CreateReadModel(entity);
        }

        public async Task<ReadUserModel> UpdateAsync(long id, UpdateUserModel model, CancellationToken token)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);
            if (entity == default)
                throw new HtNotFoundException($"User with Id {id} not found");

            _handler.Update(model, entity);
            await _context.SaveChangesWrappedAsync(token);

            return _handler.CreateReadModel(entity);
        }
    }
}
