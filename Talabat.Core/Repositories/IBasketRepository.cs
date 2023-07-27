
using Talabat.Core.Entities;

namespace Talabat.Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);

        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);

        Task<bool> DeleteBasket(string basketId);

    }
}
