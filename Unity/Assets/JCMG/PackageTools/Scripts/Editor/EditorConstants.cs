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

using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Internal constants and readonly fields for package tools usage.
	/// </summary>
	internal static class EditorConstants
	{
		// Package Manifest
		public const string DefaultPackageVersion = "1.0.0";
		public const string DefaultUnityVersion = "2018.1";

		// File
		public const string PackageJsonFilename = "package.json";
		public const string WildcardFilter = "*";
		public const string AssetExtension = ".asset";
		public const string MetaFormat = "{0}.meta";
		public const string GeneratedFolderName = "Generated";
		public static readonly string ProjectPath =
			Application.dataPath.Remove(Application.dataPath.Length - 6, 6);

		// Icons
		public const string EditorFolderIcon = "Folder Icon";
		public const string EditorFileIcon = "TextAsset Icon";

		// Inspector
		public const string PackageJsonHeader = "Package Json";
		public const string PackageContentHeader = "Package Content and Export";
		public const string PackageActionsHeader = "Actions";

		public const string SourcePathsHeaderLabel = "Source Paths";
		public const string SourcePathElementLabelFormat = "Path {0}:";

		public const string IgnorePathsHeaderLabel = "Exclude Paths";

		public const string KeywordsHeaderLabel = "Keywords";
		public const string KeywordElementLabelFormat = "Keyword {0}:";

		public const string DependencyHeaderLabel = "Dependencies";
		public const string DependencyElementLabelFormat = "Dependency {0}:";

		public const string UpdatePackageButtonText = "Export Package Source";

		public static string SelectSourcePathFilePickerTitle = "Select Source Asset Path";
		public const string SelectSourcePathPickerFolderTitle = "Select Source Folder Path";
		public const string SelectPackageExportPathPickerTitle = "Select Package Export Folder";
		public const string ProgressBarTitle = "Exporting Package Source";

		public const float FolderPathPickerHeight = 26f;
		public const float FolderPathPickerBuffer = 36f;

		// Logging
		public const string PackageUpdateErrorFormat =
			"[Package Tools] Failed to update package source for [{0}].";

		public const string PackageUpdateSuccessFormat =
			"[Package Tools] Successfully updated package source for [{0}].";

	}
}
