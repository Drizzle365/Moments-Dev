using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using Moments.Api.Model;

namespace Moments.Common;

public static class Crawler
{
    private static readonly HttpClient HttpClient = new();

    public static async Task<List<Read>?> CrawlAsync(Subscription subscription)
    {
        if (subscription.Type is SubscriptionType.RssOrAtom)
        {
            return await CrawlRssAtomAsync(subscription);
        }

        return null;
    }

    private static async Task<List<Read>?> CrawlRssAtomAsync(Subscription subscription)
    {
        try
        {
            var xmlContent = await HttpClient.GetStringAsync(subscription.SubscriptionUrl);
            using var xmlReader = XmlReader.Create(new StringReader(xmlContent));
            var feed = SyndicationFeed.Load(xmlReader);
            if (feed != null)
            {
                return feed.Items.Select(item => new Read
                    {
                        Title = item.Title?.Text,
                        Link = item.Links.FirstOrDefault()?.Uri?.AbsoluteUri,
                        PubDate = item.PublishDate.UtcDateTime,
                        Description = item.Summary?.Text.Replace("[&#8230;]", "..."),
                        Content = ReadTextContent(item.Content),
                        SubscriptionId = subscription.SubscriptionId
                    })
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return null;
    }

    private static string? ReadTextContent(SyndicationContent content)
    {
        var textContent = content as TextSyndicationContent;
        return textContent?.Text;
    }

    public static async Task<Subscription> CrawlWebsiteInfoAsync(string feedUrl)
    {
        using HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(10);
        string baseUrl = GetBaseUrl(feedUrl);
        string feedContent = await client.GetStringAsync(feedUrl);
        string htmlContent = await client.GetStringAsync(baseUrl);
        using XmlReader reader = XmlReader.Create(new StringReader(feedContent));
        SyndicationFeed? feed = null;
        feed = await Task.Run(() => {
            return SyndicationFeed.Load(reader);
        });

        return new Subscription
        {
            Name = feed.Title.Text,
            Introduction = feed.Description.Text,
            SubscriptionUrl = feedUrl,
            WebsiteUrl = ExtractWebsiteUrl(htmlContent, baseUrl),
            Avatar = ExtractAvatarUrl(htmlContent, baseUrl)
        };
    }

    private static string ExtractWebsiteUrl(string htmlContent, string baseUrl)
    {
        Regex regex =
            new Regex(
                "<link\\s+[^>]*rel=(?:\"canonical\"|'canonical'|\"alternate\"|'alternate')[^>]*href=(['\"])(.*?)\\1",
                RegexOptions.IgnoreCase);
        Match match = regex.Match(htmlContent);
        string websiteUrl = match.Success ? match.Groups[2].Value : baseUrl;
        if (!Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute))
        {
            websiteUrl = new Uri(new Uri(baseUrl), websiteUrl).AbsoluteUri;
        }

        return websiteUrl;
    }

    private static string ExtractAvatarUrl(string htmlContent, string baseUrl)
    {
        Regex regex = new Regex("<link\\s+[^>]*rel=(?:\"icon\"|'icon')[^>]*href=(['\"])(.*?)\\1",
            RegexOptions.IgnoreCase);
        Match match = regex.Match(htmlContent);

        string avatarUrl = match.Success ? match.Groups[2].Value : baseUrl + "/favicon.ico";

        if (!Uri.IsWellFormedUriString(avatarUrl, UriKind.Absolute))
        {
            avatarUrl = new Uri(new Uri(baseUrl), avatarUrl).AbsoluteUri;
        }

        return avatarUrl;
    }

    private static string GetBaseUrl(string url)
    {
        Uri uri = new Uri(url);
        return $"{uri.Scheme}://{uri.Host}";
    }
}