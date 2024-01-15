using Moments.Api.Model;

namespace Moments.Api.Service
{
    /// <summary>
    /// 定义订阅服务的接口，包括创建、获取、更新和删除订阅的操作。
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// 创建新订阅。
        /// </summary>
        /// <param name="subscription">要创建的订阅对象。</param>
        /// <returns>创建后的订阅对象的自增值</returns>
        Task<int> CreateSubscriptionAsync(Subscription subscription);

        /// <summary>
        /// 通过订阅ID获取订阅信息。
        /// </summary>
        /// <param name="subscriptionId">要获取信息的订阅ID。</param>
        /// <returns>与指定ID相关联的订阅对象。</returns>
        Task<Subscription> GetSubscriptionByIdAsync(int subscriptionId);

        /// <summary>
        /// 获取所有订阅列表。
        /// </summary>
        /// <returns>包含所有订阅的列表。</returns>
        Task<List<Subscription>> GetAllSubscriptionsAsync();

        /// <summary>
        /// 更新订阅信息。
        /// </summary>
        /// <param name="subscription">要更新的订阅对象。</param>
        Task UpdateSubscriptionAsync(Subscription subscription);

        /// <summary>
        /// 删除指定ID的订阅。
        /// </summary>
        /// <param name="subscriptionId">要删除的订阅ID。</param>
        Task DeleteSubscriptionAsync(int subscriptionId);
    }
}