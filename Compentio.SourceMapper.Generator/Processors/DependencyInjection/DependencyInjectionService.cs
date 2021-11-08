using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    /// <summary>
    /// Services methods for DependencyInjection class
    /// </summary>
    internal static class DependencyInjectionService
    {
        private const string AutofacAssemblyName = "Autofac.Extensions.DependencyInjection";
        private const string DotNetCoreAssemblyName = "Microsoft.Extensions.DependencyInjection";
        private const string StructureMapAssemblyName = "StructureMap.Microsoft.DependencyInjection";

        /// <summary>
        /// Return dependency injection container type based on assembies collection
        /// </summary>
        /// <param name="assemblies">Collection of assemblies</param>
        /// <returns></returns>
        internal static DependencyInjectionType GetDependencyInjectionType(IEnumerable<AssemblyIdentity> assemblies)
        {
            if (assemblies.Any(ai => ai.Name.Equals(AutofacAssemblyName, StringComparison.OrdinalIgnoreCase)))
            {
                return DependencyInjectionType.Autofac;
            }

            if (assemblies.Any(ai => ai.Name.Equals(DotNetCoreAssemblyName, StringComparison.OrdinalIgnoreCase)))
            {
                return DependencyInjectionType.DotNetCore;
            }

            if (assemblies.Any(ai => ai.Name.Equals(StructureMapAssemblyName, StringComparison.OrdinalIgnoreCase)))
            {
                return DependencyInjectionType.StructureMap;
            }

            return DependencyInjectionType.None;
        }
    }
}
