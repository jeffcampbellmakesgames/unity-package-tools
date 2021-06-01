/*
MIT License

Copyright (c) Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
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
				EditorConstants.UNITY_PACKAGE_NAME_FORMAT,
				config.displayName.Replace(EditorConstants.EMPTY_SPACE, EditorConstants.UNDERSCORE),
				config.packageVersion);
			var finalFilePath = Path.GetFullPath(Path.Combine(
				Path.Combine(
					EditorConstants.PROJECT_PATH,
					config.legacyPackageDestinationPath),
				fileName));

			// Show the UI and kick off the package export.
			if (!Application.isBatchMode)
			{
				EditorUtility.DisplayProgressBar(
					EditorConstants.PROGRESS_BAR_TITLE_LEGACY,
					EditorConstants.COMPILING_PROGRESS_MESSAGE,
					0);
			}

			AssetDatabase.ExportPackage(assetPaths.ToArray(), finalFilePath);

			if (!Application.isBatchMode)
			{
				EditorUtility.RevealInFinder(finalFilePath);
				EditorUtility.ClearProgressBar();
			}
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
					Path.GetFullPath(Path.Combine(EditorConstants.PROJECT_PATH, assetFolder))
						.Contains(Path.GetFullPath(Path.Combine(EditorConstants.PROJECT_PATH, x)))))
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
				string.Format(EditorConstants.META_FORMAT, assetFolder)
			};

			var fullProjectPath = Path.GetFullPath(EditorConstants.PROJECT_PATH);

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
