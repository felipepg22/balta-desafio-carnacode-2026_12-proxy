// DESAFIO: Sistema de Acesso a Documentos Confidenciais
// PROBLEMA: Uma aplicação corporativa precisa controlar acesso a documentos sensíveis,
// fazer cache de documentos pesados e registrar todas as operações. O código atual
// mistura lógica de negócio com controle de acesso, cache e logging

using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatternChallenge
{
    // Contexto: Sistema que gerencia documentos confidenciais com requisitos de:
    // - Controle de acesso baseado em permissões
    // - Cache para documentos pesados
    // - Auditoria de todas as operações
    
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
            SizeInBytes = content.Length * 2; // Simulando tamanho
        }
    }

    // Classe real que acessa documentos (recurso custoso)
    public class DocumentRepository
    {
        private Dictionary<string, ConfidentialDocument> _database;

        public DocumentRepository()
        {
            Console.WriteLine("[Repository] Inicializando conexão com banco de dados...");
            Thread.Sleep(1000); // Simulando conexão pesada
            
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

        public ConfidentialDocument GetDocument(string documentId)
        {
            Console.WriteLine($"[Repository] Carregando documento {documentId} do banco...");
            Thread.Sleep(500); // Simulando operação custosa
            
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

    public class User
    {
        public string Username { get; set; }
        public int ClearanceLevel { get; set; }

        public User(string username, int clearanceLevel)
        {
            Username = username;
            ClearanceLevel = clearanceLevel;
        }
    }

    // Problema: Cliente precisa implementar controle de acesso, cache e logging
    public class DocumentService
    {
        private DocumentRepository _repository;
        private Dictionary<string, ConfidentialDocument> _cache;
        private List<string> _auditLog;

        public DocumentService()
        {
            _repository = new DocumentRepository(); // Conexão custosa sempre criada
            _cache = new Dictionary<string, ConfidentialDocument>();
            _auditLog = new List<string>();
        }

        public ConfidentialDocument ViewDocument(string documentId, User user)
        {
            // Problema 1: Lógica de auditoria misturada
            var logEntry = $"[{DateTime.Now:HH:mm:ss}] {user.Username} tentou acessar {documentId}";
            _auditLog.Add(logEntry);
            Console.WriteLine($"[Audit] {logEntry}");

            // Problema 2: Verificação de acesso manual
            ConfidentialDocument doc;
            
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
                    _cache[documentId] = doc; // Cache manual
                }
            }

            if (doc == null)
            {
                Console.WriteLine($"❌ Documento {documentId} não encontrado");
                return null;
            }

            // Problema 3: Controle de acesso espalhado no código
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
            // Problema: Mesmo código de controle e auditoria repetido
            var logEntry = $"[{DateTime.Now:HH:mm:ss}] {user.Username} tentou editar {documentId}";
            _auditLog.Add(logEntry);
            Console.WriteLine($"[Audit] {logEntry}");

            var doc = _cache.ContainsKey(documentId) 
                ? _cache[documentId] 
                : _repository.GetDocument(documentId);

            if (doc == null || user.ClearanceLevel < doc.SecurityLevel)
            {
                Console.WriteLine($"❌ Operação não autorizada");
                return;
            }

            _repository.UpdateDocument(documentId, newContent);
            
            // Problema: Invalidar cache manualmente
            if (_cache.ContainsKey(documentId))
            {
                _cache.Remove(documentId);
            }

            Console.WriteLine($"✅ Documento atualizado");
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

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Documentos Confidenciais ===\n");

            // Problema: Inicialização custosa mesmo se não usar
            var service = new DocumentService();

            var manager = new User("joao.silva", 5);
            var employee = new User("maria.santos", 2);

            Console.WriteLine("\n--- Gerente acessando documento de alto nível ---");
            var doc1 = service.ViewDocument("DOC002", manager);

            Console.WriteLine("\n--- Funcionário tentando acessar mesmo documento ---");
            var doc2 = service.ViewDocument("DOC002", employee);

            Console.WriteLine("\n--- Gerente acessando novamente (deveria usar cache) ---");
            var doc3 = service.ViewDocument("DOC002", manager);

            Console.WriteLine("\n--- Funcionário acessando documento permitido ---");
            var doc4 = service.ViewDocument("DOC003", employee);

            service.ShowAuditLog();

            Console.WriteLine("\n=== PROBLEMAS ===");
            Console.WriteLine("✗ Lógica de controle de acesso misturada com lógica de negócio");
            Console.WriteLine("✗ Cache implementado manualmente em cada operação");
            Console.WriteLine("✗ Auditoria espalhada por todo o código");
            Console.WriteLine("✗ Repository sempre criado, mesmo se não usado (lazy loading impossível)");
            Console.WriteLine("✗ Difícil adicionar nova funcionalidade (ex: rate limiting)");
            Console.WriteLine("✗ Código duplicado entre ViewDocument e EditDocument");
            Console.WriteLine("✗ Cliente conhece muito sobre implementação interna");

            // Perguntas para reflexão:
            // - Como adicionar funcionalidade sem modificar a classe original?
            // - Como controlar acesso ao objeto real de forma transparente?
            // - Como implementar lazy loading (criação sob demanda)?
            // - Como adicionar cache, logging e segurança de forma desacoplada?
        }
    }
}
