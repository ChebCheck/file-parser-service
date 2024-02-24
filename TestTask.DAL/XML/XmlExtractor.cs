using System.Text.RegularExpressions;

namespace TestTask.DAL.XML;

public static class XmlExtractor
{
    public static string GetValueFromNode(string xmlText, string nodeName)
    {
        Console.WriteLine($"[*] Try to extract {nodeName}");
        Regex regEx = new Regex($@"<{nodeName}>(\S+)</{nodeName}>");
        string nodeText = regEx.Match(xmlText).Value;
        int valueLength = nodeText.LastIndexOf('<') - 1 - nodeText.IndexOf('>');
        Console.WriteLine("[V] Success\n");
        return nodeText.Substring(nodeText.IndexOf('>') + 1 , valueLength);
    }
}
