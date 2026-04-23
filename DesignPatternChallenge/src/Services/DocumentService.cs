using System;
using DesignPatternChallenge.src.Interfaces;

namespace DesignPatternChallenge;

public class DocumentService : IDocumentService
{
    private readonly Lazy<DocumentRepository> _repository = new(() => new DocumentRepository());

    public ConfidentialDocument? ViewDocument(string documentId, User user)
        => _repository.Value.GetDocument(documentId);

    public void EditDocument(string documentId, User user, string newContent)
        => _repository.Value.UpdateDocument(documentId, newContent);

    public void ShowAuditLog()
        => Console.WriteLine("\n[Audit] Nenhum log no serviço real. Use proxy de auditoria.");
}
