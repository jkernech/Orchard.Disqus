
namespace Orchard.Disqus.Services
{
    public interface IDisqusSynchronizationService : IDependency
    {
        int ImportComments();
    }
}