using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepo;

        public OrderService(IUnitOfWork unitOfWork,
        IBasketRepository basketRepo)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrder(string buyerEmail, int deliveryMethodId, string basketId, Address shippingToAddress)
        {
            // Get Basket from Repo
            // API only trust product id and quantity
            var basket = await basketRepo.GetCustomerBasket(basketId);

            // Get item from Repo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                var orderItem = new OrderItem
                {
                    ItemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl),
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                items.Add(orderItem);
            }

            // Get Delivery Method
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // Calc Subtotal
            var subTotal = items.Sum(i => i.Price * i.Quantity);

            // Create Order
            var order = new Order(buyerEmail, shippingToAddress, deliveryMethod, items, subTotal);
            unitOfWork.Repository<Order>().Add(order);

            // Save order into database
            var result = await unitOfWork.Complete();

            if (result <= 0) return null;

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod()
        {
            return await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderById(int id, string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(id, buyerEmail);

            return await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUser(string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);

            return await unitOfWork.Repository<Order>().GetAllWithSpec(spec);
        }
    }
}