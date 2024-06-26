using System;
using System.IO;
using Unity.Profiling;
using Unity.Profiling.Memory;
using UnityEngine;

namespace com.absence.memory.internals
{
    public class MemorySnapshotter
    {
        private string m_snapshotFolderPath = string.Empty;

        public MemorySnapshotter()
        {
            string projectPath = Directory.GetParent(Application.dataPath)?.FullName;
            m_snapshotFolderPath = Path.Combine(projectPath, Constants.SNAPSHOT_FOLDER_NAME);

            if (!Directory.Exists(m_snapshotFolderPath)) Directory.CreateDirectory(m_snapshotFolderPath);
        }
        public void Perform()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string snapshotPath = Path.Combine(m_snapshotFolderPath, $"{Constants.SNAPSHOT_PREFIX}{timestamp}{Constants.SNAPSHOT_FILE_EXTENSION}");

            MemoryProfiler.TakeSnapshot(snapshotPath, OnSnapshotFinished, OnScreenshotCaptured);
        }

        private void OnSnapshotFinished(string path, bool result)
        {
            if (!result)
            {
                Debug.LogError("Snapshot failed.");
                return;
            }

            Debug.Log($"Snapshot taken, path: {path}");
        }
        private void OnScreenshotCaptured(string path, bool result, DebugScreenCapture screenCapture)
        {
            if (!result)
            {
                Debug.LogError("Screenshot failed.");
                return;
            }

            CreateScreenshotFileAndWrite(path, screenCapture);
        }

        private void CreateScreenshotFileAndWrite(string path, DebugScreenCapture screenCapture)
        {
            string screenshotPath = Path.ChangeExtension(path, Constants.SCREENSHOT_FILE_EXTENSION);

            Debug.Log($"Creating screenshot file... ({screenCapture.RawImageDataReference.Length} bytes.)");

            byte[] managedImageData = Helpers.CopyDataToManagedArray(screenCapture.RawImageDataReference);

            Texture2D screenshotCreated = new Texture2D(screenCapture.Width, screenCapture.Height, screenCapture.ImageFormat, false);
            screenshotCreated.LoadRawTextureData(managedImageData);
            screenshotCreated.Apply();

            byte[] pngData = screenshotCreated.EncodeToPNG();
            File.WriteAllBytes(screenshotPath, pngData);

            UnityEngine.Object.DestroyImmediate(screenshotCreated);

            Debug.Log($"Screenshot created, path: {path}");
        }
    }
}