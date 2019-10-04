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
		public const string DEFAULT_PACKAGE_VERSION = "1.0.0";
		public const string DEFAULT_UNITY_VERSION = "2018.1";

		// File
		public const string PACKAGE_JSON_FILENAME = "package.json";
		public const string WILDCARD_FILTER = "*";
		public const string ASSET_EXTENSION = ".asset";
		public const string META_FORMAT = "{0}.meta";
		public const string GENERATED_FOLDER_NAME = "Generated";
		public const string UNITY_PACKAGE_NAME_FORMAT = "{0}_v{1}.unityPackage";
		public const char EMPTY_SPACE = ' ';
		public const char UNDERSCORE = '_';
		public static readonly string PROJECT_PATH =
			Application.dataPath.Remove(Application.dataPath.Length - 6, 6);

		// Icons
		public const string EDITOR_FOLDER_ICON = "Folder Icon";
		public const string EDITOR_FILE_ICON = "TextAsset Icon";

		// Inspector
		public const string PACKAGE_JSON_HEADER = "Package Json";
		public const string PACKAGE_CONTENT_HEADER = "Package Content and Export";
		public const string PACKAGE_ACTIONS_HEADER = "Actions";

		public const string SOURCE_PATHS_HEADER_LABEL = "Source Paths";
		public const string SOURCE_PATH_ELEMENT_LABEL_FORMAT = "Path {0}:";

		public const string IGNORE_PATHS_HEADER_LABEL = "Exclude Paths";

		public const string KEYWORDS_HEADER_LABEL = "Keywords";
		public const string KEYWORD_ELEMENT_LABEL_FORMAT = "Keyword {0}:";

		public const string DEPENDENCY_HEADER_LABEL = "Dependencies";
		public const string DEPENDENCY_ELEMENT_LABEL_FORMAT = "Dependency {0}:";

		public const string UPDATE_PACKAGE_BUTTON_TEXT = "Export Package Source";
		public const string EXPORT_LEGACY_PACKAGE_BUTTON_TEXT = "Export as Legacy Package";

		public const string SELECT_SOURCE_PATH_FILE_PICKER_TITLE = "Select Source Asset Path";
		public const string SELECT_SOURCE_PATH_PICKER_FOLDER_TITLE = "Select Source Folder Path";
		public const string SELECT_PACKAGE_EXPORT_PATH_PICKER_TITLE = "Select Package Export Folder";
		public const string PROGRESS_BAR_TITLE = "Exporting Package Source";
		public const string PROGRESS_BAR_TITLE_LEGACY = "Exporting Legacy Package";
		public const string COMPILING_PROGRESS_MESSAGE = "Compiling legacy package contents...";

		public const float FOLDER_PATH_PICKER_HEIGHT = 26f;
		public const float FOLDER_PATH_PICKER_BUFFER = 36f;

		// Logging
		public const string PACKAGE_UPDATE_ERROR_FORMAT =
			"[Package Tools] Failed to update package source for [{0}].";

		public const string PACKAGE_UPDATE_SUCCESS_FORMAT =
			"[Package Tools] Successfully updated package source for [{0}].";
	}
}
