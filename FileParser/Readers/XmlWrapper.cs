using System.Xml;

namespace FileParser.Readers;

public static class XmlWrapper
{
    public static XmlNode SelectSingle(XmlNode context, string xPath)
    {
        Console.WriteLine($"[*] Trying to read {xPath} from {context.Name}");
        var result = context.SelectSingleNode(xPath);
        if(result == null)
        {
            throw new Exception();
        }
        Console.WriteLine("[V] Success\n");
        return result;
    }

    public static XmlNodeList SelectAny(XmlNode context, string xPath)
    {
        Console.WriteLine($"[*] Trying to read {xPath} from {context.Name}");
        var result = context.SelectNodes(xPath);
        if(result == null)
        {
            throw new Exception();
        }
        Console.WriteLine("[V] Success\n");
        return result;
    }
}
