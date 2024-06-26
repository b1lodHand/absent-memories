using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace com.absence.memory.internals
{
    public static class Helpers
    {
        public static byte[] CopyDataToManagedArray(NativeArray<byte> nativeArray)
        {
            byte[] managedArray = new byte[nativeArray.Length];
            unsafe
            {
                fixed (byte* destination = managedArray)
                {
                    Buffer.MemoryCopy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(nativeArray),
                        destination,
                        nativeArray.Length,
                        nativeArray.Length);
                }
            }

            return managedArray;
        }
    }
}