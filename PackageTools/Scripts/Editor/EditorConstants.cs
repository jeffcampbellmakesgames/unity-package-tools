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

		// Icons
		public const string EditorFolderIcon = "Folder Icon";
		public const string EditorFileIcon = "TextAsset Icon";

		// Inspector
		public const string PackageJsonHeader = "Package Json";
		public const string PackageContentHeader = "Package Content and Export";
		public const string PackageActionsHeader = "Actions";

		public const string SourcePathsHeaderLabel = "Source Paths";
		public const string SourcePathElementLabelFormat = "Path {0}:";

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
