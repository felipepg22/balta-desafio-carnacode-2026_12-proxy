using DesignPatternChallenge.src.Interfaces;

namespace DesignPatternChallenge.src.Proxies;

public class AuditDocumentServiceProxy : DocumentServiceProxyBase
{
    private readonly List<string> _auditLog = new();

    public AuditDocumentServiceProxy(IDocumentService innerDocumentService)
        : base(innerDocumentService)
    {
    }

    public override ConfidentialDocument? ViewDocument(string documentId, User user)
    {
        AddLog($"{user.Username} tentou acessar {documentId}");

        var document = innerDocumentService.ViewDocument(documentId, user);
        AddLog(document is null
            ? $"Falha ao acessar {documentId} por {user.Username}"
            : $"Acesso concedido a {documentId} para {user.Username}");

        return document;
    }

    public override void EditDocument(string documentId, User user, string newContent)
    {
        AddLog($"{user.Username} tentou editar {documentId}");
        innerDocumentService.EditDocument(documentId, user, newContent);
        AddLog($"Finalizada tentativa de edição em {documentId} por {user.Username}");
    }

    public override void ShowAuditLog()
    {
        Console.WriteLine("\n=== Log de Auditoria ===");
        foreach (var entry in _auditLog)
        {
            Console.WriteLine(entry);
        }
    }

    private void AddLog(string message)
    {
        var logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
        _auditLog.Add(logEntry);
        Console.WriteLine($"[Audit] {logEntry}");
    }
}
