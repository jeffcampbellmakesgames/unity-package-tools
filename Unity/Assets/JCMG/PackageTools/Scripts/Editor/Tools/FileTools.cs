/*
MIT License

Copyright (c) 2019 Jeff Campbell

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
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for dealing with files/directories
	/// </summary>
	internal static class FileTools
	{
		/// <summary>
		/// Converts <paramref name="fullFilePath"/> into a relative file path from <paramref name="referencePath"/>.
		/// </summary>
		/// <param name="fullFilePath"></param>
		/// <param name="referencePath"></param>
		/// <returns></returns>
		public static string ConvertToRelativePath(string fullFilePath, string referencePath)
		{
			var fileUri = new Uri(fullFilePath);
			var referenceUri = new Uri(referencePath);
			return referenceUri.MakeRelativeUri(fileUri).ToString();
		}

		/// <summary>
		/// Creates or updates the existing package contents of <see cref="PackageManifestConfig"/>
		/// <paramref name="packageManifest"/>.
		/// </summary>
		/// <param name="packageManifest"></param>
		public static void CreateOrUpdatePackageSource(PackageManifestConfig packageManifest)
		{
			#if UNITY_EDITOR
			EditorUtility.DisplayProgressBar(EditorConstants.ProgressBarTitle, string.Empty, 0f);
			#endif

			try
			{
				// Created the folders up to the package json path and then create/import the package.json file.
				// Its important to have the package.json in the Unity project so that it will have a meta file
				// Packages without meta files cause warnings/errors when imported.
				var packageManifestAssetPath = AssetDatabase.GetAssetPath(packageManifest);
				var parentManifestParentFolderAssetPath = packageManifestAssetPath
					.Replace(packageManifest.name, string.Empty)
					.Replace(EditorConstants.AssetExtension, string.Empty);
				var generatedFolderAssetPath = Path.Combine(parentManifestParentFolderAssetPath, EditorConstants.GeneratedFolderName);
				var fullGeneratedFolderAssetPath = Path.GetFullPath(generatedFolderAssetPath);

				if (!Directory.Exists(fullGeneratedFolderAssetPath))
				{
					Directory.CreateDirectory(fullGeneratedFolderAssetPath);
				}

				var packageJsonFolderAssetPath = Path.Combine(generatedFolderAssetPath, packageManifest.Id);
				var fullPackageJsonFolderAssetPath = Path.GetFullPath(packageJsonFolderAssetPath);
				if (!Directory.Exists(fullPackageJsonFolderAssetPath))
				{
					Directory.CreateDirectory(fullPackageJsonFolderAssetPath);
				}

				var packageJsonAssetPath = Path.Combine(packageJsonFolderAssetPath, EditorConstants.PackageJsonFilename);
				var fullPackageJsonAssetPath = Path.GetFullPath(packageJsonAssetPath);

				File.WriteAllText(fullPackageJsonAssetPath, packageManifest.GenerateJson());
				AssetDatabase.ImportAsset(packageJsonAssetPath, ImportAssetOptions.ForceUpdate);

				// If the directory exists, delete its contents or make the directory.
				if (Directory.Exists(packageManifest.packageDestinationPath))
				{
					RecursivelyDeleteDirectoryContents(new DirectoryInfo(packageManifest.packageDestinationPath));
				}
				else
				{
					Directory.CreateDirectory(packageManifest.packageDestinationPath);
				}

				// Copy over the package json and meta file
				var destinationPackageJsonPath =
					Path.Combine(packageManifest.packageDestinationPath, EditorConstants.PackageJsonFilename);
				File.Copy(fullPackageJsonAssetPath, destinationPackageJsonPath);

				var packageJsonMetaPath = string.Format(EditorConstants.MetaFormat, fullPackageJsonAssetPath);

				File.Copy(packageJsonMetaPath,
					Path.Combine(packageManifest.packageDestinationPath,
						string.Format(EditorConstants.MetaFormat, EditorConstants.PackageJsonFilename)));

				// Copy over all directory and file content from source to destination.
				var normalizedDestinationPath = Path.GetFullPath(packageManifest.packageDestinationPath);
				foreach (var sourcePath in packageManifest.packageSourcePaths)
				{
					// If its a file, copy over it and its meta file if it exists.
					var normalizedSourcePath = Path.GetFullPath(sourcePath);
					if (IsFile(normalizedSourcePath) && File.Exists(normalizedSourcePath))
					{
						var fileInfo = new FileInfo(normalizedSourcePath);
						if (fileInfo.Directory == null)
						{
							continue;
						}

						var parentDirectoryPath = fileInfo.Directory.FullName;
						var newPath = normalizedSourcePath.Replace(parentDirectoryPath, normalizedDestinationPath);

						File.Copy(normalizedSourcePath, newPath);

						var sourceMetaPath = string.Format(EditorConstants.MetaFormat, normalizedSourcePath);
						if (File.Exists(sourceMetaPath))
						{
							var newMetaPath = sourceMetaPath.Replace(parentDirectoryPath, normalizedDestinationPath);
							File.Copy(sourceMetaPath, newMetaPath);
						}
					}
					// Otherwise if this is a folder, copy it and all the contents over to the destination folder.
					else
					{
						RecursivelyCopyDirectoriesAndFiles(
							packageManifest,
							new DirectoryInfo(normalizedSourcePath),
							normalizedSourcePath,
							normalizedDestinationPath);
					}
				}

				Debug.LogFormat(EditorConstants.PackageUpdateSuccessFormat, packageManifest.packageName);

				#if UNITY_EDITOR
				EditorUtility.RevealInFinder(destinationPackageJsonPath);
				#endif
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat(EditorConstants.PackageUpdateErrorFormat, packageManifest.packageName);
				Debug.LogErrorFormat(packageManifest, ex.ToString());
			}
			finally
			{
				#if UNITY_EDITOR
				EditorUtility.DisplayProgressBar(EditorConstants.ProgressBarTitle, string.Empty, 1f);
				EditorUtility.ClearProgressBar();
				#endif
			}
		}

		/// <summary>
		/// Returns true if the <paramref name="path"/> is for a file, otherwise false.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private static bool IsFile(string path)
		{
			var attr = File.GetAttributes(path);
			return (attr & FileAttributes.Directory) != FileAttributes.Directory;
		}

		/// <summary>
		/// Recursively copies all sub-folders and files in <see cref="DirectoryInfo"/> <paramref name="directoryInfo"/>
		/// from parent folder <see cref="sourcePath"/> to <paramref name="destinationPath"/>.
		/// </summary>
		/// <param name="packageManifest"></param>
		/// <param name="directoryInfo"></param>
		/// <param name="sourcePath"></param>
		/// <param name="destinationPath"></param>
		private static void RecursivelyCopyDirectoriesAndFiles(
			PackageManifestConfig packageManifest,
			DirectoryInfo directoryInfo,
			string sourcePath,
			string destinationPath)
		{
			var normalizedSourcePath = Path.GetFullPath(sourcePath);
			var normalizedDestinationPath = Path.GetFullPath(destinationPath);
			var subDirectoryInfo = directoryInfo.GetDirectories(EditorConstants.WildcardFilter);
			foreach (var sdi in subDirectoryInfo)
			{
				// If any of the paths we're looking at match the ignore paths from the user, skip them
				if (packageManifest.packageIgnorePaths.Any(x =>
					sdi.FullName.Contains(Path.GetFullPath(Path.Combine(EditorConstants.ProjectPath, x)))))
				{
					continue;
				}

				Directory.CreateDirectory(sdi.FullName.Replace(normalizedSourcePath, normalizedDestinationPath));

				RecursivelyCopyDirectoriesAndFiles(packageManifest, sdi, normalizedSourcePath, normalizedDestinationPath);
			}

			var fileInfo = directoryInfo.GetFiles(EditorConstants.WildcardFilter);
			foreach (var fi in fileInfo)
			{
				// If any of the paths we're looking at match the ignore paths from the user, skip them
				if (packageManifest.packageIgnorePaths.Any(x =>
					fi.FullName.Contains(Path.GetFullPath(Path.Combine(EditorConstants.ProjectPath, x)))))
				{
					continue;
				}

				var newPath = Path.GetFullPath(fi.FullName).Replace(normalizedSourcePath, normalizedDestinationPath);
				File.Copy(fi.FullName, newPath);
			}
		}

		/// <summary>
		/// Recursively deletes all sub-folders (excluding hidden folders) and files in <see cref="DirectoryInfo"/>
		/// <paramref name="directoryInfo"/>.
		/// </summary>
		/// <param name="directoryInfo"></param>
		private static void RecursivelyDeleteDirectoryContents(DirectoryInfo directoryInfo)
		{
			var subDirectoryInfo = directoryInfo.GetDirectories(EditorConstants.WildcardFilter);
			foreach (var sdi in subDirectoryInfo)
			{
				if ((sdi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
				{
					sdi.Delete(true);
				}
			}

			var fileInfo = directoryInfo.GetFiles(EditorConstants.WildcardFilter);
			foreach (var fi in fileInfo)
			{
				fi.Delete();
			}
		}
	}
}
