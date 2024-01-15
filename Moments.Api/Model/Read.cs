using FreeSql.DataAnnotations;

namespace Moments.Api.Model;

[Index("uk_Link", "Link", true)]
public class Read
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int ArticleId { get; set; }

    public string? Title { get; set; }
    public string? Link { get; set; }
    public DateTime PubDate { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }

    public int SubscriptionId { get; set; }
}