using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(string buyerEmail, int deliveryMethodId, string basketId, Address shippingToAddress);
        Task<IReadOnlyList<Order>> GetOrdersByUser(string buyerEmail);
        Task<Order> GetOrderById(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod();
    }
}