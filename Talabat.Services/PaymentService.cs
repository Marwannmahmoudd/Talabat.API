using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification.OrderSpec;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUniteOfWork _uniteOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUniteOfWork uniteOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _uniteOfWork = uniteOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            
            StripeConfiguration.ApiKey = _configuration["StripSettings:SecretKey"];
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if(basket == null) 
                return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
              var deliveryMethod =  await _uniteOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }
            if(basket?.Items.Count() > 0) 
            {
                foreach (var item in basket.Items)
                {
                    var product = await _uniteOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret  = paymentIntent.ClientSecret;
            }
            else
            {
                var updatedOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,

                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId,updatedOptions);
            }
           var updatedbasket = await _basketRepository.UdpateBasketAsync(basket);
            return updatedbasket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentid, bool isSucceeded)
        {
            var spec = new OrderWithPaymentIntentSpec(paymentIntentid);
            var order = await _uniteOfWork.Repository<Order>().GetByIdSpecAsync(spec);
            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailure;
            await _uniteOfWork.CompleteAsync();
            return order;
        }
    }
}
