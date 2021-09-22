namespace Compentio.SourceMapper.DependencyInjection
{
    internal class DependencyInjection
    {
        internal DependencyInjectionType DependencyInjectionType { get; set; }
        internal string DependencyInjectionClassName { get; }  = "MappersDependencyInjectionExtensions";
    }
}