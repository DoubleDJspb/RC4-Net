namespace DD.Cryptography
{
    /// <summary>
    /// Array Extension Class.
    /// </summary>
    static class SwapExt
    {
        /// <summary>
        /// Swaps two array elements.
        /// </summary>
        /// <typeparam name="T">Array type.</typeparam>
        /// <param name="array">Target array.</param>
        /// <param name="index1">The first element of the array.</param>
        /// <param name="index2">The second element of the array.</param>
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }    
}