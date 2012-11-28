using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtpSharp
{
    /// <summary>
    /// Some helper methods to perform common key functions
    /// </summary>
    internal class KeyUtilities
    {
        /// <summary>
        /// Concatinate two byte arrays together
        /// </summary>
        /// <param name="arrays">The two arrays to concatinate</param>
        /// <returns></returns>
        internal static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        /// <summary>
        /// Overwrite potentially sensetive data with random junk
        /// </summary>
        /// <remarks>
        /// Warning!
        /// 
        /// This isn't foolproof by any means.  The garbage collector could have moved the actual
        /// location in memory to another location during a collection cycle and left the old data in place
        /// simply marking it as available.  We can't control this or even detect it.
        /// This method is simply a good faith effort to limit the exposure of sensetive data in memory as much as possible
        /// </remarks>
        internal static void Destroy(byte[] sensetiveData)
        {
            if (sensetiveData == null)
                throw new ArgumentNullException("sensetiveData");
            new Random().NextBytes(sensetiveData);
        }
    }
}
