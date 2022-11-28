using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OrderAppWebApi.RabbitMq;
using OrderWebApi.Models.Context;
using OrderWebApi.Models.Dtos;
using OrderWebApi.Models.Entities;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Text;


//using static OrderWebApi.Models.Results.ApiResponse<T>;

namespace OrderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly OrderContextDb _context;

        public OrderController(IMemoryCache memoryCache, OrderContextDb context, IMapper mapper)
        {
            _memoryCache = memoryCache;
            _context = context;
            _mapper = mapper;
        }
        #region Create Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderRequest)
        {
            Order order = _mapper.Map<Order>(createOrderRequest);
            List<OrderDetail> orderDetails = _mapper.Map<List<ProductDetailDto>, List<OrderDetail>>(createOrderRequest.ProductDetails) as List<OrderDetail>;
            order.TotalAmount = createOrderRequest.ProductDetails.Sum(p => p.Amount);
            order.OrderDetails = orderDetails;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Mail işlemi kaldı

            var datas = Encoding.UTF8.GetBytes(createOrderRequest.CustomerEmail);

            SetQueue.SendQueue(datas);

            return Ok(new ApiResponse<int>(StatusType.Success, order.Id));
        }
        #endregion
        #region Redis_Loglama
        [HttpGet]
        public async Task<ActionResult> Get(string? category)
        {
            // localhost 127.0.0.1
            var redisclient = new RedisClient("127.0.0.1", 6379);
            IRedisTypedClient<List<Product>> redisproducts = redisclient.As<List<Product>>();
            var result = new List<Product>();
            if (category is null)
            {
                result = redisclient.Get<List<Product>>("products");
                if (result is null)
                {
                    result = await _context.Products.ToListAsync();
                    redisclient.Set("products", result, TimeSpan.FromMinutes(10));

                }
            }
            else
            {
                result = redisclient.Get<List<Product>>($"products-{category}");
                if (result is null)
                {
                    result = await _context.Products.Where(p => p.Category == category).ToListAsync();
                    redisclient.Set($"products={category}", result, TimeSpan.FromMinutes(10));
                }

            }
            var productdtos = _mapper.Map<List<Product>, List<ProductDto>>(result);
            return Ok(new ApiResponse<List<ProductDto>>(StatusType.Success, productdtos));

        }
        #endregion
        //#region MemoryCache_Loglama
        //[HttpGet]
        //public async Task<ActionResult> Get(string? category)
        //{
        //    var result = new List<Product>();
        //    if (category is null)
        //    {
        //        result = _memoryCache.Get("products") as List<Product>;
        //        if (result is null)
        //        {
        //            result = await _context.Products.ToListAsync();
        //            _memoryCache.Set("products", result, TimeSpan.FromMinutes(10));
        //        }
        //    }
        //    else
        //    {
        //        result = _memoryCache.Get($"products=={category}") as List<Product>;
        //        if (result is null)
        //        {
        //            result = await _context.Products.Where(p => p.Category == category).ToListAsync();
        //            _memoryCache.Set($"products={category}", result, TimeSpan.FromMinutes(10));
        //        }

        //    }
        //    var productdtos = _mapper.Map<List<Product>, List<ProductDto>>(result);
        //    return Ok(new ApiResponse<List<ProductDto>>(StatusType.Success, productdtos));

        //}
        //#endregion

    }
}