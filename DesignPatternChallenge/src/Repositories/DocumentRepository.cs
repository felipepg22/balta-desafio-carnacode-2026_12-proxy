using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatternChallenge;

public class DocumentRepository
{
    private readonly Dictionary<string, ConfidentialDocument> _database;

    public DocumentRepository()
    {
        Console.WriteLine("[Repository] Inicializando conexão com banco de dados...");
        Thread.Sleep(1000);

        _database = new Dictionary<string, ConfidentialDocument>
        {
            ["DOC001"] = new ConfidentialDocument(
                "DOC001",
                "Relatório Financeiro Q4",
                "Conteúdo confidencial do relatório financeiro... (10 MB)",
                3
            ),
            ["DOC002"] = new ConfidentialDocument(
                "DOC002",
                "Estratégia de Mercado 2025",
                "Planos estratégicos confidenciais... (50 MB)",
                5
            ),
            ["DOC003"] = new ConfidentialDocument(
                "DOC003",
                "Manual do Funcionário",
                "Políticas e procedimentos... (2 MB)",
                1
            )
        };
    }

    public ConfidentialDocument? GetDocument(string documentId)
    {
        Console.WriteLine($"[Repository] Carregando documento {documentId} do banco...");
        Thread.Sleep(500);

        if (_database.ContainsKey(documentId))
        {
            var doc = _database[documentId];
            Console.WriteLine($"[Repository] Documento carregado: {doc.SizeInBytes / (1024 * 1024)} MB");
            return doc;
        }

        return null;
    }

    public void UpdateDocument(string documentId, string newContent)
    {
        Console.WriteLine($"[Repository] Atualizando documento {documentId}...");
        Thread.Sleep(300);

        if (_database.ContainsKey(documentId))
        {
            _database[documentId].Content = newContent;
        }
    }
}
