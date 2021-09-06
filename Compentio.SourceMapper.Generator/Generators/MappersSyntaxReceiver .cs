using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Generators
{
    public class MappersSyntaxReceiver : ISyntaxReceiver
    {
        public List<TypeDeclarationSyntax> Candidates { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not AttributeSyntax attribute)
                return;

            var name = ExtractName(attribute.Name);

            if ($"{name}Attribute" != nameof(MapperAttribute))
                return;

            if (attribute.Parent?.Parent is InterfaceDeclarationSyntax interfaceDeclaration)
                Candidates.Add(interfaceDeclaration);

            if (attribute.Parent?.Parent is ClassDeclarationSyntax classDeclaration)
                Candidates.Add(classDeclaration);
        }

        private static string? ExtractName(TypeSyntax type)
        {
            while (type != null)
            {
                switch (type)
                {
                    case IdentifierNameSyntax ins:
                        return ins.Identifier.Text;

                    case QualifiedNameSyntax qns:
                        type = qns.Right;
                        break;

                    default:
                        return null;
                }
            }

            return null;
        }
    }
}
