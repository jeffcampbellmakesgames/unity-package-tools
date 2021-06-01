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
using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Continuous-integration API for package tools.
	/// </summary>
	public static class PackageToolsCI
	{
		// Command-line arguments
		private const string ID_ARG_KEY = "id";
		private const string VERSION_ARG_KEY = "version";
		private const string GENERATE_VERSION_CONSTANTS_ARG_KEY = "generateversionconstants";

		// Logs
		private const string LOG_PREFIX = "[Package Tools] ";
		private const string CI_STARTING = LOG_PREFIX + "Starting Package Tools CI...";
		private const string CI_COMPLETE = LOG_PREFIX + "Package Tools CI has completed!";
		private const string CI_COMMAND_LINE_ARGS_PARSED_FORMAT = LOG_PREFIX + "Parsed CI Command Line Args.\n\n{0}";
		private const string CI_USING_ONLY_CONFIGS_FROM_ARG =
			LOG_PREFIX + "Using configs matching IDs passed in 'ID' argument";
		private const string CI_FOUND_CONFIGS = LOG_PREFIX + "Found [{0}] configs in project.";
		private const string CI_USING_ALL_CONFIGS = LOG_PREFIX + "Using all configs in project.";
		private const string CI_GENERATION_STARTING = LOG_PREFIX + "Starting to generate packages...";
		private const string CI_PACKAGE_NOT_FOUND_FORMAT = LOG_PREFIX + "Could not find a package for ID: [{0}], skipping it.";
		private const string CI_PACKAGE_FOUND_FORMAT = LOG_PREFIX + "Package [{0}] found for ID: [{1}]";
		private const string CI_GENERATING_LEGACY_PACKAGE_FORMAT = LOG_PREFIX + "Generating Legacy Package for Config [{0}] with ID: [{1}]";
		private const string CI_SKIPPING_LEGACY_PACKAGE_FORMAT = LOG_PREFIX + "Skipping Legacy Package for Config [{0}] with ID: [{1}] as no output path is present in config.";
		private const string CI_GENERATING_PACKAGE_SOURCE_FORMAT = LOG_PREFIX + "Generating Package Source for Config [{0}] with ID: [{1}].";
		private const string CI_SKIPPING_PACKAGE_SOURCE_FORMAT = LOG_PREFIX + "Skipping Package Source for Config [{0}] with ID: [{1}] as no output path is present in config.";

		private static readonly StringBuilder SB;

		static PackageToolsCI()
		{
			SB = new StringBuilder(8192);
		}

		/// <summary>
		/// Attempts to use zero or more <see cref="PackageManifestConfig"/> assets to generate legacy Unity packages
		/// and Unity source.
		/// </summary>
		public static void Generate()
		{
			Debug.Log(CI_STARTING);

			try
			{
				EditorApplication.LockReloadAssemblies();
				AssetDatabase.StartAssetEditing();

				// Get command line args and log them
				var commandLineArgs = CommandLineTools.GetKVPCommandLineArguments();

				SB.Clear();
				const string CLI_ARG_FORMAT = "{0} => {1}";
				foreach (var commandLineArg in commandLineArgs)
				{
					SB.AppendFormat(CLI_ARG_FORMAT, commandLineArg.Key, commandLineArg.Value);
					SB.AppendLine();
				}

				Debug.LogFormat(CI_COMMAND_LINE_ARGS_PARSED_FORMAT, SB.ToString());

				// Attempt to parse CLI-passed version, if present.
				string version = null;
				if (commandLineArgs.TryGetValue(VERSION_ARG_KEY, out var versionRawValue))
				{
					version = (string)versionRawValue;
				}

				// See if we should generate version constants
				var generateVersionConstants = false;
				if (commandLineArgs.TryGetValue(
					GENERATE_VERSION_CONSTANTS_ARG_KEY,
					out var generateVersionConstantsRawValue))
				{
					generateVersionConstants = bool.Parse(generateVersionConstantsRawValue.ToString());
				}

				// Get all package manifests in project
				var allPackageManifestConfigs = PackageManifestTools.GetAllConfigs();

				Debug.LogFormat(CI_FOUND_CONFIGS, allPackageManifestConfigs.Length);

				// Check to see if any IDs have been passed for specific configs
				string[] configIds;
				if (commandLineArgs.ContainsKey(ID_ARG_KEY))
				{
					Debug.Log(CI_USING_ONLY_CONFIGS_FROM_ARG);

					const char COMMA_CHAR = ',';
					var idArgValue = commandLineArgs[ID_ARG_KEY].ToString();
					configIds = idArgValue.Split(COMMA_CHAR);
				}
				// Otherwise generate all package manifest configs in project.
				else
				{
					Debug.Log(CI_USING_ALL_CONFIGS);

					configIds = allPackageManifestConfigs.Select(x => x.Id).ToArray();
				}

				// For each matching config ID, find the matching package manifest config and generate any relevant packages.
				Debug.Log(CI_GENERATION_STARTING);
				foreach (var configId in configIds)
				{
					var matchingConfig = allPackageManifestConfigs.FirstOrDefault(x =>
						string.Compare(x.Id, configId, StringComparison.OrdinalIgnoreCase) == 0);

					// If a config cannot be found matching config id, skip it and continue.
					if (matchingConfig == null)
					{
						Debug.LogWarningFormat(CI_PACKAGE_NOT_FOUND_FORMAT, configId);

						continue;
					}

					var configName = matchingConfig.name;

					Debug.LogFormat(CI_PACKAGE_FOUND_FORMAT, configName, configId);

					// Set the CLI passed version if present, otherwise default to checked in version number.
					if (!string.IsNullOrEmpty(version))
					{
						matchingConfig.packageVersion = version;

						EditorUtility.SetDirty(matchingConfig);
					}

					// If set to generate version constants, do so.
					if (generateVersionConstants)
					{
						CodeGenTools.GenerateVersionConstants(matchingConfig);
					}

					// Otherwise generate the corresponding legacy unity package and package source if their output paths
					// have been defined
					if (!string.IsNullOrEmpty(matchingConfig.legacyPackageDestinationPath))
					{
						Debug.LogFormat(CI_GENERATING_LEGACY_PACKAGE_FORMAT, configName, configId);

						UnityFileTools.CompileLegacyPackage(matchingConfig);
					}
					else
					{
						Debug.LogFormat(CI_SKIPPING_LEGACY_PACKAGE_FORMAT, configName, configId);
					}

					if (!string.IsNullOrEmpty(matchingConfig.packageDestinationPath))
					{
						Debug.LogFormat(CI_GENERATING_PACKAGE_SOURCE_FORMAT, configName, configId);

						FileTools.CreateOrUpdatePackageSource(matchingConfig);
					}
					else
					{
						Debug.LogFormat(CI_SKIPPING_PACKAGE_SOURCE_FORMAT, configName, configId);
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogErrorFormat("An unexpected error occured during package generation:\n\n{0}", e);
			}
			finally
			{
				AssetDatabase.StopAssetEditing();
				EditorApplication.UnlockReloadAssemblies();
			}

			Debug.Log(CI_COMPLETE);
		}
	}
}
