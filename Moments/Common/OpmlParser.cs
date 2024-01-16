using System.Collections;
using System.Xml;
using Moments.Api.Model;

namespace Moments.Common;

public static class OpmlParser {
    public static List<string> Parse(string s){
        var doc = new XmlDocument();
        doc.LoadXml(s);
        XmlNodeList feeds = doc.GetElementsByTagName("outline");
        List<string> result = new List<string>();
        for(int i = 0; i < feeds.Count; i++){
            result.Add(feeds[i].Attributes["xmlUrl"].Value);
        }
        return result;
    }
}