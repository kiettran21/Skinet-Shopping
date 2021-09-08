using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetCustomerBasket(string basketId);
        Task<CustomerBasket> UpdateCustomerBasket(CustomerBasket basket);
        Task<bool> DeleteCustomerBasket(string basketId);
    }
}