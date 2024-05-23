

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification.OrderSpec;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUniteOfWork _uniteOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepository;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepository;
        //private readonly IGenericRepository<Order> _orderRepository;

        public OrderService(IBasketRepository basketRepository,
            IUniteOfWork uniteOfWork , IPaymentService paymentService )
            //IGenericRepository<Product> productRepository,
        //    IGenericRepository<DeliveryMethod> deliveryMethodRepository , IGenericRepository<Order> OrderRepository)
        {
            _basketRepository = basketRepository;
            _uniteOfWork = uniteOfWork;
            _paymentService = paymentService;
            //_productRepository = productRepository;
            //_deliveryMethodRepository = deliveryMethodRepository;
            //_orderRepository = OrderRepository;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1- get Basket from BasketRepository
            var basket =await _basketRepository.GetBasketAsync(basketId);
            //2- Get Selected Item At Basket From Product Repo (Create OrderItem Obj)
            var OrderItems = new List<OrderItem>();
            if(basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _uniteOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);
                    var OrderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);

                }
            }
            //3- Get Subtotal Of Order
            var subTotal = OrderItems.Sum(orderitem => orderitem.Price * orderitem.Quantity);
            //4- Get DeliveryMethod From DeliveryMethodRepo
            var deliveryMethod = await _uniteOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var orderRepo = _uniteOfWork.Repository<Order>();
            var orderSpec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);
            var existingOrder = await orderRepo.GetByIdSpecAsync(orderSpec);
            if (existingOrder != null)
            {
                orderRepo.Delete(existingOrder);
              
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            // 5- Create Order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, OrderItems, subTotal,basket.PaymentIntentId);
         await orderRepo.AddAsync(order);
            //6- Save To DateBase
            var result = await _uniteOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;

        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = _uniteOfWork.Repository<Order>();
            var spec = new OrderSpecifications(buyerEmail);
            var orders =await orderRepo.GetAllWithSpecAsync(spec);
            return orders;     
        }

        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
            => await _uniteOfWork.Repository<Order>().GetByIdSpecAsync(new OrderSpecifications(orderId, buyerEmail));

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
       => await  _uniteOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}
