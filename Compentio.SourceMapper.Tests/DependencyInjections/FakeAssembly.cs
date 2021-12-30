using System;
using System.Reflection;

namespace Compentio.SourceMapper.Tests.DependencyInjections
{
    public class FakeAssembly : Assembly
    {
        private readonly string _assemblyName;
        private readonly Version _assemblyVersion;

        public FakeAssembly(string assemblyName, Version assemblyVersion = null)
        {
            _assemblyName = assemblyName;
            _assemblyVersion = assemblyVersion;
        }

        public override AssemblyName GetName(bool copiedName)
        {
            var assemblyName = new AssemblyName(_assemblyName);
            
            if (_assemblyVersion != null)
                assemblyName.Version = _assemblyVersion;

            return assemblyName;
        }
    }
}