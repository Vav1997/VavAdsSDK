using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vav.Ads.Editor
{
    public static class BuildRelease
    {
        private const string ReleaseRepository =
            "/Users/elen/Development/VavAdsSDK";

        [MenuItem("Tools/Vav SDK/Build Release")]
        public static void Build()
        {
            string projectRoot = Directory.GetParent(Application.dataPath)!.FullName;

            string packageSource = Path.Combine(
                projectRoot,
                "Packages",
                "com.vav.ads");

            if (!Directory.Exists(packageSource))
            {
                Debug.LogError($"Package folder not found:\n{packageSource}");
                return;
            }

            if (!Directory.Exists(ReleaseRepository))
            {
                Debug.LogError($"Release repository not found:\n{ReleaseRepository}");
                return;
            }

            // Delete everything except .git
            foreach (string directory in Directory.GetDirectories(ReleaseRepository))
            {
                if (Path.GetFileName(directory) == ".git")
                    continue;

                Directory.Delete(directory, true);
            }

            foreach (string file in Directory.GetFiles(ReleaseRepository))
            {
                File.Delete(file);
            }

            // Copy the entire package
            CopyDirectory(packageSource, ReleaseRepository);

            Debug.Log("Package copied to release repository.");
        }

        private static void CopyDirectory(string source, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (string file in Directory.GetFiles(source))
            {
                string destinationFile = Path.Combine(
                    destination,
                    Path.GetFileName(file));

                File.Copy(file, destinationFile, true);
            }

            foreach (string directory in Directory.GetDirectories(source))
            {
                string destinationDirectory = Path.Combine(
                    destination,
                    Path.GetFileName(directory));

                CopyDirectory(directory, destinationDirectory);
            }
        }
    }
}