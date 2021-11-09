using Compentio.SourceMapper.Resources;

namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class DependencyInjection
    {
        internal virtual DependencyInjectionType DependencyInjectionType { get; set; }
        internal string DependencyInjectionClassName { get; }  = AppStrings.DependencyInjectionClassName;
    }
}