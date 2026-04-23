using DesignPatternChallenge.src.Interfaces;

namespace DesignPatternChallenge.src.Proxies;

public abstract class DocumentServiceProxyBase : IDocumentService
{
    protected readonly IDocumentService innerDocumentService;

    protected DocumentServiceProxyBase(IDocumentService innerDocumentService)
    {
        this.innerDocumentService = innerDocumentService;
    }

    public virtual ConfidentialDocument? ViewDocument(string documentId, User user)
        => innerDocumentService.ViewDocument(documentId, user);

    public virtual void EditDocument(string documentId, User user, string newContent)
        => innerDocumentService.EditDocument(documentId, user, newContent);

    public virtual void ShowAuditLog()
        => innerDocumentService.ShowAuditLog();
}
