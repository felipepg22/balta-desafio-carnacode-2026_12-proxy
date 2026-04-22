using System;

namespace DesignPatternChallenge;

public class ConfidentialDocument
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int SecurityLevel { get; set; }
    public long SizeInBytes { get; set; }

    public ConfidentialDocument(string id, string title, string content, int securityLevel)
    {
        Id = id;
        Title = title;
        Content = content;
        SecurityLevel = securityLevel;
        SizeInBytes = content.Length * 2;
    }
}
