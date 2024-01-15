using FreeSql.DataAnnotations;

namespace Moments.Api.Model;

public enum SubscriptionType
{
    RssOrAtom,
    Json,
    Regex
}

public class Subscription
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int SubscriptionId { get; set; }

    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Introduction { get; set; }
    public string WebsiteUrl { get; set; }
    public string SubscriptionUrl { get; set; }
    public string? Tags { get; set; }
    public string? Category { get; set; }

    public SubscriptionType Type { get; set; }
}