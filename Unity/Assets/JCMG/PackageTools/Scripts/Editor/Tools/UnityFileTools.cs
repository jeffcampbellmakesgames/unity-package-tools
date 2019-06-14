using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for dealing with files/directories in the Unity Assets folder.
	/// </summary>
	internal static class UnityFileTools
	{
		/// <summary>
		/// Gather all of the files/folders for the package and export them to the pre-defined location
		/// at <see cref="PackageManifestConfig.legacyPackageDestinationPath"/>.
		/// </summary>
		/// <param name="config"></param>
		public static void CompileLegacyPackage(PackageManifestConfig config)
		{
			// Gathers all of the files/folders for export as a legacy package.
			var assetPaths = new List<string>(GetAllAssetPathsRecursively(config));
			var fileName = string.Format(
				EditorConstants.UnityPackageNameFormat,
				config.displayName.Replace(EditorConstants.EmptySpace, EditorConstants.Underscore),
				config.packageVersion);
			var finalFilePath = Path.GetFullPath(Path.Combine(
				Path.Combine(
					EditorConstants.ProjectPath,
					config.legacyPackageDestinationPath),
				fileName));

			// Show the UI and kick off the package export.
			EditorUtility.DisplayProgressBar(
				EditorConstants.ProgressBarTitleLegacy,
				EditorConstants.CompilingProgressMessage,
				0);
			AssetDatabase.ExportPackage(assetPaths.ToArray(), finalFilePath);
			EditorUtility.RevealInFinder(finalFilePath);
			EditorUtility.ClearProgressBar();
		}

		/// <summary>
		/// Returns a list of relative Unity asset paths contained in
		/// <see cref="PackageManifestConfig.packageSourcePaths"/> where each path begins with the Assets
		/// folder.
		/// </summary>
		/// <param name="packageManifest"></param>
		/// <returns></returns>
		private static List<string> GetAllAssetPathsRecursively(PackageManifestConfig packageManifest)
		{
			var assetPaths = new List<string>();
			foreach (var assetFolder in packageManifest.packageSourcePaths)
			{
				// If any of the paths we're looking at match the ignore paths from the user, skip them
				if (packageManifest.packageIgnorePaths.Any(x =>
					Path.GetFullPath(Path.Combine(EditorConstants.ProjectPath, assetFolder))
						.Contains(Path.GetFullPath(Path.Combine(EditorConstants.ProjectPath, x)))))
				{
					continue;
				}

				assetPaths.AddRange(GetAllAssetPathsRecursively(assetFolder, packageManifest));
			}

			return assetPaths;
		}

		/// <summary>
		/// Returns a list of relative Unity asset paths contained in <paramref name="assetFolder"/>
		/// where each path begins with the Assets folder.
		/// </summary>
		/// <param name="assetFolder"></param>
		/// <param name="packageManifest"></param>
		/// <returns></returns>
		private static List<string> GetAllAssetPathsRecursively(string assetFolder, PackageManifestConfig packageManifest)
		{
			var assetPaths = new List<string>
			{
				assetFolder,
				string.Format(EditorConstants.MetaFormat, assetFolder)
			};

			var fullProjectPath = Path.GetFullPath(EditorConstants.ProjectPath);

			var allFilesFullPaths = FileTools.GetAllFilesRecursively(assetFolder);
			foreach(var fileFullPath in allFilesFullPaths)
			{
				// If any of the paths we're looking at match the ignore paths from the user, skip them
				if (packageManifest.packageIgnorePaths.Any(x =>
					fileFullPath.Contains(Path.GetFullPath(Path.Combine(fullProjectPath, x)))))
				{
					continue;
				}

				var relativeAssetPath = fileFullPath.Replace(fullProjectPath, string.Empty);
				assetPaths.Add(relativeAssetPath);
			}

			return assetPaths;
		}
	}
}
