using System.Linq.Expressions;
using Moments.Api.Model;
using Moments.Api.Service;
using Moments.Common;

namespace Moments.Service
{
    public class ReadService(IFreeSql db) : IReadService
    {
        public async Task CreateBySubscriptionAsync(Subscription subscription)
        {
            var res = await Crawler.CrawlAsync(subscription);
            await db.Insert(res).ExecuteAffrowsAsync();
        }

        public async Task CreateAllAsync(List<Subscription> subscriptions)
        {
            foreach (var item in subscriptions)
            {
                var res = await Crawler.CrawlAsync(item);
                await db.Insert(res).ExecuteAffrowsAsync();
            }
        }

        public async Task<List<Read>> GetReadByPageAsync(int page, int pageSize = 10)
        {
            if (page < 1)
                page = 1;

            var offset = (page - 1) * pageSize;

            return await db.Select<Read>()
                .OrderByDescending(x => x.PubDate)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Read>> GetReadByPageAsync(Expression<Func<Read, bool>> predicate, int page,
            int pageSize = 10)
        {
            if (page < 1)
                page = 1;
            var offset = (page - 1) * pageSize;
            return await db.Select<Read>()
                .Where(predicate)
                .OrderByDescending(x => x.PubDate)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            await db.Delete<Read>().Where(x => x.ArticleId == id).ExecuteAffrowsAsync();
        }

        public async Task DeleteAllAsync()
        {
            await db.Delete<Read>().Where(x => true).ExecuteAffrowsAsync();
        }
    }
}