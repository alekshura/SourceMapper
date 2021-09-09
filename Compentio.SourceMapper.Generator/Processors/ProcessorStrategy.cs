using Compentio.SourceMapper.Metadata;
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
            var methodBody = $"var target = new {metadata.ReturnType.FullName}();";
            var sourceMembers = metadata.Parameters.First().Properties;
            var targetMemebers = metadata.ReturnType.Properties;

            foreach (var targetMember in targetMemebers)
            {
                var matchedAttribute = metadata.MappingAttributes.FirstOrDefault(attribute => attribute?.Target == targetMember?.Name);                
                var matchedSourceMember = sourceMembers.FirstOrDefault(symbol => symbol?.Name == matchedAttribute?.Source);
                var matchedTargetMember = targetMemebers.FirstOrDefault(symbol => symbol?.Name == matchedAttribute?.Target);

                if (matchedSourceMember is null || matchedTargetMember is null)
                {
                    matchedSourceMember = sourceMembers.FirstOrDefault(p => p?.Name == targetMember?.Name);
                    if (matchedSourceMember is null)
                    {
                        continue;                     
                    }

                    matchedTargetMember = matchedSourceMember;
                }

                if (!IsClass(matchedSourceMember))
                {
                    methodBody += "\n";
                    methodBody += $"target.{matchedTargetMember?.Name} = source.{matchedSourceMember.Name};";
                }
                else
                {
                    //methodBody += "\n";
                    //methodBody += $"target.{matchedTargetMember?.Name} = MapMethodName({matchedSourceMember.Name});";
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
