
namespace Oje.Infrastructure.Interfac
{
    public interface EntityWithParent<T>
    {
        public long Id { get; set; }
        public T Parent { get; set; }
    }
}
