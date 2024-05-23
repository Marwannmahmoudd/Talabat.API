using StackExchange.Redis;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services.Contract;

namespace Talabat.Services
{
    public class ResponseCasheService : IResponseCasheService
    {
        private readonly IDatabase _database;
        public ResponseCasheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CasheResponseAsync(string casheKey, object response, TimeSpan timeToLive)
        {
            if (response == null) return;
            var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedJson = JsonSerializer.Serialize(response, serializeOptions);
            await _database.StringSetAsync(casheKey, serializedJson, timeToLive);
        }

        public async Task<string?> CasheResponseAsync(string casheKey)
        {
            var chashedResponse = await _database.StringGetAsync(casheKey);
            if (chashedResponse.IsNullOrEmpty) return null;
            return chashedResponse;
        }
    }
}
