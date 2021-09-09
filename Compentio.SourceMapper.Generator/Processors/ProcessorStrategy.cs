using Microsoft.CodeAnalysis;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    interface IProcessorStrategy
    {
        string GenerateCode(ISourceMetadata sourceMetadata);           
    }

    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        public abstract string GenerateCode(ISourceMetadata sourceMetadata);

        protected virtual string GenerateMethods(ISourceMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methods += @$"public virtual {methodMetadata.MethodFullName}
                    {{
                        {GenerateMappingBody(methodMetadata)}
                    }}";
            }

            return methods;
        }

        private string GenerateMappingBody(IMethodMetadata metadata)
        {
            var methodBody = $"var target = new {metadata.ReturnType}();";
            var sourceMembers = metadata.Parameters.First().GetMembers().Select(member => member as IPropertySymbol);
            var targetMemebers = metadata.ReturnType.GetMembers().Where(member => member.Kind == SymbolKind.Property && !member.IsStatic);

            foreach (var targetMember in targetMemebers)
            {
                var matchedAttribute = metadata.MappingAttributes.FirstOrDefault(attribute => attribute?.Target == targetMember.Name);
                if (matchedAttribute is not null && 
                    sourceMembers.Any(symbol => symbol?.Name == matchedAttribute.Source) && 
                    targetMemebers.Any(symbol => symbol?.Name == matchedAttribute.Target))
                {
                    methodBody += "\n";
                    methodBody += $"target.{matchedAttribute.Target} = source.{matchedAttribute.Source};";
                    continue;
                }

                var matchedField = sourceMembers.FirstOrDefault(p => p?.Name == targetMember.Name);
                if (matchedField is not null)
                {
                    if (!IsClass(matchedField))
                    {
                        methodBody += "\n";
                        methodBody += $"target.{matchedField.Name} = source.{matchedField.Name};";
                    } 

                    // TODO Nested types
                }
            }

            methodBody += "\n";
            methodBody += $"return target;";
            return methodBody;
        }

        private bool IsClass(IPropertySymbol propertySymbol)
        {
            return propertySymbol.Type.SpecialType == SpecialType.None && propertySymbol.Type.TypeKind == TypeKind.Class;
        }
    }
}
