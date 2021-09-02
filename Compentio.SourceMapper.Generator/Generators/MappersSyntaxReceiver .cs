using Compentio.SourceMapper.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compentio.SourceMapper.Generators
{
    public class MappersSyntaxReceiver : ISyntaxReceiver
    {
        public List<InterfaceDeclarationSyntax> Candidates { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not AttributeSyntax attribute)
                return;

            //var name = ExtractName(attribute.Name);

            //if (name != "Mapper" && name != typeof(MapperAttribute).Name)
            //    return;

            // "attribute.Parent" is "AttributeListSyntax"
            // "attribute.Parent.Parent" is a C# fragment the attribute is applied to
            if (attribute.Parent?.Parent is InterfaceDeclarationSyntax interfaceDeclaration)
                Candidates.Add(interfaceDeclaration);
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
