using System.Reflection;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class FakeAssembly : Assembly
    {
        private readonly string _assemblyName;

        public FakeAssembly(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        public override AssemblyName GetName(bool copiedName)
        {
            return new AssemblyName(_assemblyName);
        }
    }
}