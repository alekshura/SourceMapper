namespace Compentio.SourceMapper.Helpers
{
    internal static class ObjectHelper
    {
        /// <summary>
        /// Method to swap two reference objects of type T
        /// </summary>
        internal static void Swap<T>(ref T source, ref T target)
        {
            var tempObject = source;
            source = target;
            target = tempObject;
        }
    }
}