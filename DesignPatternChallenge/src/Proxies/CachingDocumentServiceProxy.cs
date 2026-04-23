using DesignPatternChallenge.src.Interfaces;

namespace DesignPatternChallenge.src.Proxies;

public class CachingDocumentServiceProxy : DocumentServiceProxyBase
{
    private readonly Dictionary<string, ConfidentialDocument> _cache = new();

    public CachingDocumentServiceProxy(IDocumentService innerDocumentService)
        : base(innerDocumentService)
    {
    }

    public override ConfidentialDocument? ViewDocument(string documentId, User user)
    {
        if (_cache.TryGetValue(documentId, out var cachedDocument))
        {
            Console.WriteLine($"[Cache] Documento {documentId} encontrado no cache");
            return cachedDocument;
        }

        var document = innerDocumentService.ViewDocument(documentId, user);
        if (document is not null)
        {
            _cache[documentId] = document;
            Console.WriteLine($"[Cache] Documento {documentId} armazenado no cache");
        }

        return document;
    }

    public override void EditDocument(string documentId, User user, string newContent)
    {
        innerDocumentService.EditDocument(documentId, user, newContent);
        _cache.Remove(documentId);
        Console.WriteLine($"[Cache] Cache invalidado para {documentId}");
    }
}
