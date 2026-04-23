using System;

namespace DesignPatternChallenge.src.Interfaces;

public interface IDocumentService
{
    public ConfidentialDocument? ViewDocument(string documentId, User user);

    public void EditDocument(string documentId, User user, string newContent);

    public void ShowAuditLog();
}
