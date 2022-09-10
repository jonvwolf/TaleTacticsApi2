using HorrorTacticsApi2.Data.Entities;
using HorrorTacticsApi2.Domain.Models.Users;

namespace HorrorTacticsApi2.Domain.Handlers
{
    public class UserModelEntityHandler
    {
        readonly PasswordHelper _passwordHelper;
        public UserModelEntityHandler(PasswordHelper passwordHelper)
        {
            _passwordHelper = passwordHelper;
        }
        public ReadUserModel CreateReadModel(UserEntity entity)
        {
            return new ReadUserModel(entity.Id, entity.UserName, entity.Role);
        }

        public UserEntity CreateEntity(CreateUserModel model)
        {
            var salt = _passwordHelper.GenerateSalt();
            var password = _passwordHelper.GenerateHash(model.Password, salt);

            return new UserEntity(model.Username, password, salt);
        }

        public void Update(UpdateUserModel model, UserEntity entity)
        {
            if (!string.IsNullOrEmpty(model.Password))
            {
                var salt = _passwordHelper.GenerateSalt();
                var password = _passwordHelper.GenerateHash(model.Password, salt);
                entity.Password = password;
                entity.Salt = salt;
            }
        }

        public void UpdateLastLogin(UserEntity entity)
        {
            entity.LastLogin = DateTime.UtcNow;
        }
    }
}
