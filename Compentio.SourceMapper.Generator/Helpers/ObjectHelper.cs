namespace Compentio.SourceMapper.Helpers
{
    internal static class ObjectHelper
    {
        /// <summary>
        /// Swap two reference type objects
        /// </summary>
        internal static void Swap<T>(ref T source, ref T target)
        {
            var tempObject = source;
            source = target;
            target = tempObject;
        }
    }
}