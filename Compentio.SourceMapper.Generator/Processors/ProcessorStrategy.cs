using Compentio.SourceMapper.Attributes;
using Compentio.SourceMapper.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors
{
    interface IProcessorStrategy
    {
        void Initialize(ISourceMetadata sourceMetadata);
        string GenerateCode();           
    }

    internal abstract class AbstractProcessorStrategy : IProcessorStrategy
    {
        protected ISourceMetadata? _sourceMetadata;

        public void Initialize(ISourceMetadata sourceMetadata)
        {
            _sourceMetadata = sourceMetadata;
        }

        public abstract string GenerateCode();

        protected virtual string GenerateMethods(ISourceMetadata sourceMetadata)
        {
            var methods = string.Empty;

            foreach (var methodMetadata in sourceMetadata.MethodsMetadata)
            {
                methods += @$"{GenerateMethod(methodMetadata)}";
            }

            return methods;
        }

        private string GenerateMethod(IMethodMetadata methodMetadata)
        {
            return @$"public virtual {methodMetadata.MethodFullName}
                {{
                    var target = new {methodMetadata.ReturnType.FullName}();
                    
                    {GenerateMappings(methodMetadata.Parameters.First().Properties, methodMetadata.ReturnType.Properties, methodMetadata.MappingAttributes)}
                    
                    return target;
                }}";

        }

        private string GenerateMappings(IEnumerable<IPropertyMetadata> sourceMembers, IEnumerable<IPropertyMetadata> targetMemebers, IEnumerable<MappingAttribute> mappingAttributes)
        {
            var mappings = string.Empty;

            foreach (var targetMember in targetMemebers)
            {
                var matchedAttribute = mappingAttributes.FirstOrDefault(attribute => attribute?.Target == targetMember?.Name);                
                var matchedSourceMember = sourceMembers.FirstOrDefault(symbol => symbol?.Name == matchedAttribute?.Source);
                var matchedTargetMember = targetMemebers.FirstOrDefault(symbol => symbol?.Name == matchedAttribute?.Target);

                if (matchedSourceMember is null || matchedTargetMember is null)
                {
                    matchedSourceMember = sourceMembers.FirstOrDefault(p => p?.Name == targetMember?.Name);
                    if (matchedSourceMember is null)
                    {
                        continue;                     
                    }

                    matchedTargetMember = targetMember;
                }

                if (!matchedSourceMember.IsClass && !matchedTargetMember.IsClass)
                {
                    mappings += "\n";
                    mappings += $"target.{matchedTargetMember?.Name} = source.{matchedSourceMember?.Name};";
                }
                
                if (matchedSourceMember.IsClass && matchedTargetMember.IsClass)
                {
                    var methodName = GetDefinedMethodName(matchedSourceMember, matchedTargetMember);
                    if (!string.IsNullOrWhiteSpace(methodName))
                    {
                        mappings += "\n";
                        mappings += $"target.{matchedTargetMember?.Name} = {methodName}(source.{matchedSourceMember.Name});";
                    }                    
                }
            }

            return mappings;
        }

        private string GetDefinedMethodName(IPropertyMetadata source, IPropertyMetadata target)
        {
            var method = _sourceMetadata?.MethodsMetadata.FirstOrDefault(m => 
                m.ReturnType.FullName == target.Type.FullName && m.Parameters.FirstOrDefault().FullName == source.Type.FullName);


            return method?.MethodName ?? string.Empty;
        }
    }
}
