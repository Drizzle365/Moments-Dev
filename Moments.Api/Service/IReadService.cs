using System.Linq.Expressions;
using Moments.Api.Model;

namespace Moments.Api.Service
{
    public interface IReadService
    {
        // 增加新订阅的内容
        Task CreateBySubscriptionAsync(Subscription subscription);

        // 更新全部订阅
        Task CreateAllAsync(List<Subscription> subscriptions);

        // 获取分页文章
        Task<List<Read>> GetReadByPageAsync(int page, int pageSize = 10);

        // 获取分页文章
        Task<List<Read>> GetReadByPageAsync(Expression<Func<Read, bool>> predicate, int page, int pageSize = 10);

        // 根据ID删除
        Task DeleteByIdAsync(int id);

        // 全部删除
        Task DeleteAllAsync();
    }
}