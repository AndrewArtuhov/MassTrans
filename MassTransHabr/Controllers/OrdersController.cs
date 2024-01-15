using MassTransHabr.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SharedModels;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDistributedCache _cache;

    public OrdersController(IPublishEndpoint publishEndpoint, IDistributedCache distributedCache)
    {
        _publishEndpoint = publishEndpoint;
        _cache = distributedCache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromQuery] OrderDto orderDto)
    {
        OrderDto? orderChash = null;
        var userString = await _cache.GetStringAsync(orderDto.ProductName.ToString());

        if (userString != null) 
            orderChash = JsonSerializer.Deserialize<OrderDto>(userString);

        if (orderChash == null)
        {
            await _cache.SetStringAsync(orderDto.ProductName.ToString(), JsonSerializer.Serialize(orderDto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
        }

        await _publishEndpoint.Publish<IOrderCreated>(new
        {
            Id = 1,
            orderDto.ProductName,
            orderDto.Quantity,
            orderDto.Price
        });

        return Ok();
    }
}
