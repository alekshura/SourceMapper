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
        private const string NinjectAssemblyName = "Ninject.Web.AspNetCore";

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

            if (assemblies.Any(ai => ai.Name.Equals(NinjectAssemblyName, StringComparison.OrdinalIgnoreCase)) && SupportNinjectVersion(assemblies))
            {
                DependencyInjectionType = DependencyInjectionType.Ninject;
            }
        }

        /// <summary>
        /// Method checks that Ninject assemby is in supported version
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private bool SupportNinjectVersion(IEnumerable<AssemblyIdentity> assemblies)
        {
            int notSupportedMajorVersion = 4;

            return assemblies.Any(ai => ai.Name.Equals("Ninject", StringComparison.OrdinalIgnoreCase) && ai.Version.Major < notSupportedMajorVersion);
        }
    }
}