using com.absence.memory.internals;

namespace com.absence.memory
{
    public static class MemoryManagement
    {
        public static void TakeSnapshot()
        {
            MemorySnapshotter snapshotter = new();
            snapshotter.Perform();
        }
    }

}