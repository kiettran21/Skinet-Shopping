using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
using System;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            this.database = redis.GetDatabase();
        }

        public async Task<bool> DeleteCustomerBasket(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetCustomerBasket(string basketId)
        {
            var data = await database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateCustomerBasket(CustomerBasket basket)
        {
            var created = await database.StringSetAsync(basket.Id,
            JsonSerializer.Serialize(basket), TimeSpan.FromDays(7));

            if (!created) return null;

            return await GetCustomerBasket(basket.Id);
        }
    }
}