using System;
using System.Collections.Generic;

namespace DesignPatternChallenge;

public class DocumentService
{
    private readonly DocumentRepository _repository;
    private readonly Dictionary<string, ConfidentialDocument> _cache;
    private readonly List<string> _auditLog;

    public DocumentService()
    {
        _repository = new DocumentRepository();
        _cache = new Dictionary<string, ConfidentialDocument>();
        _auditLog = new List<string>();
    }

    public ConfidentialDocument? ViewDocument(string documentId, User user)
    {
        var logEntry = $"[{DateTime.Now:HH:mm:ss}] {user.Username} tentou acessar {documentId}";
        _auditLog.Add(logEntry);
        Console.WriteLine($"[Audit] {logEntry}");

        ConfidentialDocument? doc;

        if (_cache.ContainsKey(documentId))
        {
            Console.WriteLine($"[Cache] Documento {documentId} encontrado no cache");
            doc = _cache[documentId];
        }
        else
        {
            doc = _repository.GetDocument(documentId);
            if (doc != null)
            {
                _cache[documentId] = doc;
            }
        }

        if (doc == null)
        {
            Console.WriteLine($"❌ Documento {documentId} não encontrado");
            return null;
        }

        if (user.ClearanceLevel < doc.SecurityLevel)
        {
            Console.WriteLine($"❌ Acesso negado! Nível {user.ClearanceLevel} < Requerido {doc.SecurityLevel}");
            _auditLog.Add($"[{DateTime.Now:HH:mm:ss}] ACESSO NEGADO para {user.Username}");
            return null;
        }

        Console.WriteLine($"✅ Acesso permitido ao documento: {doc.Title}");
        return doc;
    }

    public void EditDocument(string documentId, User user, string newContent)
    {
        var logEntry = $"[{DateTime.Now:HH:mm:ss}] {user.Username} tentou editar {documentId}";
        _auditLog.Add(logEntry);
        Console.WriteLine($"[Audit] {logEntry}");

        var doc = _cache.ContainsKey(documentId)
            ? _cache[documentId]
            : _repository.GetDocument(documentId);

        if (doc == null || user.ClearanceLevel < doc.SecurityLevel)
        {
            Console.WriteLine("❌ Operação não autorizada");
            return;
        }

        _repository.UpdateDocument(documentId, newContent);

        if (_cache.ContainsKey(documentId))
        {
            _cache.Remove(documentId);
        }

        Console.WriteLine("✅ Documento atualizado");
    }

    public void ShowAuditLog()
    {
        Console.WriteLine("\n=== Log de Auditoria ===");
        foreach (var entry in _auditLog)
        {
            Console.WriteLine(entry);
        }
    }
}
