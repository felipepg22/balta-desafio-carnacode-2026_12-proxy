using System;

namespace DesignPatternChallenge;

internal class Challenge
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== Sistema de Documentos Confidenciais ===\n");

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
    }
}
