using Oje.Infrastructure.Interfac;
using System.Linq;

namespace Oje.Infrastructure.Services
{
    public static class MultiLevelForCreateUser
    {
        public static IQueryable<T> getWhereCreateUserMultiLevelForUserOwnerShip<T, User>(this IQueryable<T> input, long? loginUserId, bool CanSeeAllItems) where User : EntityWithParent<User> where T : EntityWithCreateUser<User, long>
        {
            if (CanSeeAllItems == true)
                return input;

            return input.Where(t =>
                    t.CreateUserId == loginUserId ||
                    t.CreateUser.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId);
        }

        public static IQueryable<T> getWhereCreateByUserMultiLevelForUserOwnerShip<T, User>(this IQueryable<T> input, long? loginUserId, bool CanSeeAllItems) where User : EntityWithParent<User> where T : EntityWithCreateByUser<User, long>
        {
            if (CanSeeAllItems == true)
                return input;

            return input.Where(t =>
                    t.CreateByUserId == loginUserId ||
                    t.CreateByUser.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.CreateByUser.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId);
        }

        public static IQueryable<T> getWhereIdMultiLevelForUserOwnerShip<T, User>(this IQueryable<T> input, long? loginUserId, bool CanSeeAllItems) where User : EntityWithParent<User> where T : IEntityWithId<User, long>
        {
            if (CanSeeAllItems == true)
                return input;

            return input.Where(t =>
                    t.Id == loginUserId ||
                    t.Parent.Id == loginUserId ||
                    t.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId);
        }

        public static IQueryable<T> getWhereUserIdMultiLevelForUserOwnerShip<T, User>(this IQueryable<T> input, long? loginUserId, bool CanSeeAllItems) where User : EntityWithParent<User> where T : IEntityWithUserId<User, long>
        {
            if (CanSeeAllItems == true)
                return input;

            return input.Where(t =>
                    t.UserId == loginUserId ||
                    t.User.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId ||
                    t.User.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Id == loginUserId);
        }
    }
}