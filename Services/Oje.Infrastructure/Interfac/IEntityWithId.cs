namespace Oje.Infrastructure.Interfac
{
    public interface IEntityWithId<User, CUT> where User : EntityWithParent<User> where CUT : struct
    {
        public CUT Id { get; set; }
        public User Parent { get; set; }
    }
}
