using Microsoft.AspNetCore.Mvc;
using Moments.Api.Model;
using Moments.Api.Service;

namespace Moments.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> CreateSubscriptionAsync([FromBody] Subscription subscription)
    {
        var newSubscriptionId = await subscriptionService.CreateSubscriptionAsync(subscription);
        return Ok(newSubscriptionId);
    }

    [HttpGet("{subscriptionId}")]
    public async Task<ActionResult<Subscription>> GetSubscriptionByIdAsync(int subscriptionId)
    {
        var subscription = await subscriptionService.GetSubscriptionByIdAsync(subscriptionId);

        return Ok(subscription);
    }

    [HttpGet]
    public async Task<ActionResult<List<Subscription>>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await subscriptionService.GetAllSubscriptionsAsync();
        return Ok(subscriptions);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSubscriptionAsync([FromBody] Subscription subscription)
    {
        await subscriptionService.UpdateSubscriptionAsync(subscription);
        return Ok();
    }

    [HttpDelete("{subscriptionId}")]
    public async Task<IActionResult> DeleteSubscriptionAsync(int subscriptionId)
    {
        await subscriptionService.DeleteSubscriptionAsync(subscriptionId);
        return NoContent();
    }
}