using System;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for code-gen.
	/// </summary>
	internal static class CodeGenTools
	{
		private const string TEMPLATE =
@"namespace ${namespace}
{
	/// <summary>
	/// Version info for this library.
	/// </summary>
	internal static class VersionConstants
	{
		/// <summary>
		/// The semantic version
		/// </summary>
		public const string VERSION = ""${version}"";

		/// <summary>
		/// The branch of GIT this package was published from.
		/// </summary>
		public const string GIT_BRANCH = ""${git_branch}"";

		/// <summary>
		/// The current GIT commit hash this package was published on.
		/// </summary>
		public const string GIT_COMMIT = ""${git_commit}"";

		/// <summary>
		/// The UTC human-readable date this package was published at.
		/// </summary>
		public const string PUBLISH_DATE = ""${publish_date}"";

		/// <summary>
		/// The UTC time this package was published at.
		/// </summary>
		public const string PUBLISH_TIME = ""${publish_utc_time}"";
	}
}
";

		private const string GLOBAL_TEMPLATE =
@"/// <summary>
/// Version info for this library.
/// </summary>
internal static class VersionConstants
{
	/// <summary>
	/// The semantic version
	/// </summary>
	public const string VERSION = ""${version}"";

	/// <summary>
	/// The branch of GIT this package was published from.
	/// </summary>
	public const string GIT_BRANCH = ""${git_branch}"";

	/// <summary>
	/// The current GIT commit hash this package was published on.
	/// </summary>
	public const string GIT_COMMIT = ""${git_commit}"";

	/// <summary>
	/// The UTC human-readable date this package was published at.
	/// </summary>
	public const string PUBLISH_DATE = ""${publish_date}"";

	/// <summary>
	/// The UTC time this package was published at.
	/// </summary>
	public const string PUBLISH_TIME = ""${publish_utc_time}"";
}
";

		// Filename
		private const string FILENAME = "VersionConstants.cs";

		// Logs
		private const string NO_PATH_SPECIFIED = "A path must be specified for the VersionConstants.cs file to be written " +
		                                         "to, otherwise this file will not be created.";

		public static void GenerateVersionConstants(PackageManifestConfig config)
		{
			if(string.IsNullOrEmpty(config.versionConstantsPath))
			{
				Debug.LogWarning(NO_PATH_SPECIFIED);
				return;
			}

			// Create folder/file path info
			var folderPath = Path.GetFullPath(config.versionConstantsPath);
			var filePath = Path.Combine(folderPath, FILENAME);

			// Create file contents
			var template = string.IsNullOrEmpty(config.versionConstantsNamespace)
				? GLOBAL_TEMPLATE
				: TEMPLATE;

			var fileContents = template
				.Replace("${namespace}", config.versionConstantsNamespace)
				.Replace("${version}", config.packageVersion)
				.Replace("${git_branch}", GitTools.GetBranch())
				.Replace("${git_commit}", GitTools.GetLongHeadHash())
				.Replace("${publish_date}", DateTime.UtcNow.ToLongDateString())
				.Replace("${publish_utc_time}", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));

			File.WriteAllText(filePath, fileContents);

			var importPath = Path.Combine(config.versionConstantsPath, FILENAME);
			AssetDatabase.ImportAsset(importPath);
		}
	}
}
