using Moments.Api.Model;
using Moments.Api.Service;

namespace Moments.Service;

public class SubscriptionService(IFreeSql db) : ISubscriptionService
{
    public async Task<int> CreateSubscriptionAsync(Subscription subscription)
    {
        return (int)await db.Insert(subscription).ExecuteIdentityAsync();
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await db.Select<Subscription>().Where(s => s.SubscriptionId == subscriptionId).ToOneAsync();
    }

    public async Task<List<Subscription>> GetAllSubscriptionsAsync()
    {
        return await db.Select<Subscription>().ToListAsync();
    }

    public async Task UpdateSubscriptionAsync(Subscription subscription)
    {
        await db.Update<Subscription>().SetSource(subscription).ExecuteAffrowsAsync();
    }

    public async Task DeleteSubscriptionAsync(int subscriptionId)
    {
        await db.Delete<Subscription>().Where(s => s.SubscriptionId == subscriptionId).ExecuteAffrowsAsync();
    }

    public Subscription CreateSubscription(Subscription subscription)
    {
        throw new NotImplementedException();
    }

    public Subscription GetSubscriptionById(int subscriptionId)
    {
        throw new NotImplementedException();
    }

    public List<Subscription> GetAllSubscriptions()
    {
        throw new NotImplementedException();
    }

    public void UpdateSubscription(Subscription subscription)
    {
        throw new NotImplementedException();
    }

    public void DeleteSubscription(int subscriptionId)
    {
        throw new NotImplementedException();
    }
}