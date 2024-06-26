using UnityEditor;

namespace com.absence.memory.editor
{
    internal static class EditorJobsHelper
    {
        [MenuItem("absencee_/absent-memory/Take Quick Snapshot")]
        internal static void TakeSnapshot_Editor()
        {
            MemoryManagement.TakeSnapshot();
        }
    }
}