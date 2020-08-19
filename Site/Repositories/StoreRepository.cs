using Shared;
using System.Threading.Tasks;

namespace site.Repositories
{
    public class StoreRepository
    {
        public Task Save(Store store)
        {
            return Task.Delay(0);
        }
    }
}
