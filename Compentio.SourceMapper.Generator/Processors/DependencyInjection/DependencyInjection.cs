using Compentio.SourceMapper.Resources;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class DependencyInjection
    {
        private const string AutofacAssemblyName = "Autofac.Extensions.DependencyInjection";
        private const string DotNetCoreAssemblyName = "Microsoft.Extensions.DependencyInjection";
        private const string StructureMapAssemblyName = "StructureMap.Microsoft.DependencyInjection";

        internal virtual DependencyInjectionType DependencyInjectionType { get; set; }
        internal string DependencyInjectionClassName { get; }  = AppStrings.DependencyInjectionClassName;

        internal DependencyInjection()
        {

        }

        internal DependencyInjection(IEnumerable<AssemblyIdentity> assemblies)
        {
            DependencyInjectionType = DependencyInjectionType.None;

            if (assemblies.Any(ai => ai.Name.Equals(DotNetCoreAssemblyName, StringComparison.OrdinalIgnoreCase)))
            {
                DependencyInjectionType = DependencyInjectionType.DotNetCore;
            }

            if (assemblies.Any(ai => ai.Name.Equals(AutofacAssemblyName, StringComparison.OrdinalIgnoreCase)))
            {
                DependencyInjectionType = DependencyInjectionType.Autofac;
            }

            if (assemblies.Any(ai => ai.Name.Equals(StructureMapAssemblyName, StringComparison.OrdinalIgnoreCase)))
            {
                DependencyInjectionType = DependencyInjectionType.StructureMap;
            }
        }
    }
}