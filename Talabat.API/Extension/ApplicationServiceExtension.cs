using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Services;

namespace Talabat.API.Extension
{
    public static  class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService( this IServiceCollection services)
        {
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IUniteOfWork), typeof(UniteOfWork));
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                    .SelectMany(P => P.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                    var validationerrors = new ValidationErrorsResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationerrors);
                };
            });
            return services;
        }
    }
}
