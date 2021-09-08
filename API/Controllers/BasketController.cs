using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/basket")]
    public class BasketController: ControllerBase
    {
        private readonly IBasketRepository repo;

        public BasketController(IBasketRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket = await repo.GetCustomerBasket(id);

            return Ok(basket ?? new CustomerBasket());
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasket basket)
        {
            var updateBasket = await repo.UpdateCustomerBasket(basket);

            return Ok(updateBasket ?? new CustomerBasket());
        }

        [HttpDelete]
        public async Task DeleteCustomerBasket(string id)
        {
            await repo.DeleteCustomerBasket(id);
        }
    }
}