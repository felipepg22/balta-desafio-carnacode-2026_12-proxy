using DesignPatternChallenge.src.Interfaces;

namespace DesignPatternChallenge.src.Proxies;

public class AuthorizationDocumentServiceProxy : DocumentServiceProxyBase
{
    public AuthorizationDocumentServiceProxy(IDocumentService innerDocumentService)
        : base(innerDocumentService)
    {
    }

    public override ConfidentialDocument? ViewDocument(string documentId, User user)
    {
        var document = innerDocumentService.ViewDocument(documentId, user);
        if (document is null)
        {
            Console.WriteLine($"❌ Documento {documentId} não encontrado");
            return null;
        }

        if (user.ClearanceLevel < document.SecurityLevel)
        {
            Console.WriteLine($"❌ Acesso negado! Nível {user.ClearanceLevel} < Requerido {document.SecurityLevel}");
            return null;
        }

        Console.WriteLine($"✅ Acesso permitido ao documento: {document.Title}");
        return document;
    }

    public override void EditDocument(string documentId, User user, string newContent)
    {
        var document = innerDocumentService.ViewDocument(documentId, user);
        if (document is null)
        {
            Console.WriteLine($"❌ Documento {documentId} não encontrado");
            return;
        }

        if (user.ClearanceLevel < document.SecurityLevel)
        {
            Console.WriteLine($"❌ Operação não autorizada para {user.Username}");
            return;
        }

        innerDocumentService.EditDocument(documentId, user, newContent);
        Console.WriteLine("✅ Documento atualizado");
    }
}
