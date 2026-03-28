using System;
using System.Xml;
using System.Xml.Serialization;

namespace AbsoluteAlgorithm.Core.Serialization;

/// <summary>
/// Provides helper methods for XML serialization and deserialization.
/// </summary>
public static class Xml
{
    /// <summary>
    /// Serializes an object to an XML string.
    /// </summary>
    public static string SerializeToXml<T>(T obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var serializer = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }

    /// <summary>
    /// Deserializes an XML string to an object.
    /// </summary>
    public static T DeserializeFromXml<T>(string xml)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(xml);

        var serializer = new XmlSerializer(typeof(T));
        using var stringReader = new StringReader(xml);
        return (T)serializer.Deserialize(stringReader)!;
    }

    /// <summary>
    /// Formats an XML string with indentation.
    /// </summary>
    public static string FormatXml(string xml)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(xml);

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        using var stringWriter = new StringWriter();
        using var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
        doc.WriteTo(xmlTextWriter);
        return stringWriter.ToString();
    }
}