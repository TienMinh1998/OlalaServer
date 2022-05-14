using System.Threading.Tasks;

namespace APIProject.Domain
{
    public interface IApplicationDbContextInitializer
    {
        bool EnsureCreated();
        void Migrate();
        Task Seed();
    }
}