namespace Compentio.SourceMapper.Processors.DependencyInjection
{
    internal class DependencyInjection
    {
        internal virtual DependencyInjectionType DependencyInjectionType { get; set; }
        internal string DependencyInjectionClassName { get; }  = "MappersDependencyInjectionExtensions";
    }
}