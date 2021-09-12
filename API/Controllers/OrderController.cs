using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User.FindFirstValue(ClaimValueTypes.Email);

            // Map AddressDto to Address
            var address = mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            // Create order
            var order = await orderService.CreateOrder(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            return Ok(order);
        }

        [HttpGet]
        [Route("deliverymethod")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethod()
        {
            var deliveryMethods = await orderService.GetDeliveryMethod();

            return Ok(deliveryMethods);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var email = User.FindFirstValue(ClaimValueTypes.Email);

            var order = await orderService.GetOrderById(id, email);

            return order;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersByUser()
        {
            var email = User.FindFirstValue(ClaimValueTypes.Email);

            var orders = await orderService.GetOrdersByUser(email);

            return Ok(orders);
        }
    }
}