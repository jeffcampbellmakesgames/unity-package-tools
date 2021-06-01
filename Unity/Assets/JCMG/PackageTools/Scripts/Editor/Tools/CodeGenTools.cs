/*
MIT License

Copyright (c) 2020 Jeff Campbell

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
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for code-gen.
	/// </summary>
	internal static class CodeGenTools
	{
		// Templates
		private const string VERSION_CONSTANTS_TEMPLATE_GUID = "68dda2110af2a7b43be0f611c4201653";

		// Filename
		private const string FILENAME = "VersionConstants.cs";

		// Logs
		private const string NO_PATH_SPECIFIED = "A path must be specified for the VersionConstants.cs file to be written " +
		                                         "to, otherwise this file will not be created.";

		private const string NO_TEMPLATE_SPECIFIED = "There was no template supplied for VersionConstants via the " +
		                                             "'versionTemplateGuid' field and the default template cannot be " +
		                                             "found.";

		private const string WRITE_FILE_LOG_FORMAT = "Attempting to generate version constants at path [{0}].";

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

			// Create file contents. If we have a user-supplied template
			string template;
			if (!string.IsNullOrEmpty(config.versionTemplateGuid))
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(config.versionTemplateGuid);
				template = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath).text;
			}
			else if(string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(VERSION_CONSTANTS_TEMPLATE_GUID)))
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(VERSION_CONSTANTS_TEMPLATE_GUID);
				template = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath).text;
			}
			else
			{
				throw new FileNotFoundException(NO_TEMPLATE_SPECIFIED);
			}

			var fileContents = template
				.Replace("${version}", config.packageVersion)
				.Replace("${git_branch}", GitTools.GetBranch())
				.Replace("${git_commit}", GitTools.GetLongHeadHash())
				.Replace("${publish_date}", DateTime.UtcNow.ToLongDateString())
				.Replace("${publish_utc_time}", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));

			Debug.LogFormat(WRITE_FILE_LOG_FORMAT, config.versionConstantsPath);

			File.WriteAllText(filePath, fileContents);

			var importPath = Path.Combine(config.versionConstantsPath, FILENAME);
			AssetDatabase.ImportAsset(importPath);
		}
	}
}
