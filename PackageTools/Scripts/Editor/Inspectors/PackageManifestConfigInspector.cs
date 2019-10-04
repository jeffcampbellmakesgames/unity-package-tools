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
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	[CustomEditor(typeof(PackageManifestConfig))]
	internal sealed class PackageManifestConfigInspector : UnityEditor.Editor
	{
		private ReorderableList _sourcePathsReorderableList;
		private ReorderableList _excludePathsReorderableList;
		private ReorderableList _keywordReorderableList;
		private ReorderableList _dependenciesReorderableList;

		private const string SOURCE_PATHS_PROPERTY_NAME = "packageSourcePaths";
		private const string EXCLUDE_PATHS_PROPERTY_NAME = "packageIgnorePaths";
		private const string DESTINATION_PATH_PROPERTY_NAME = "packageDestinationPath";
		private const string LEGACY_PACKAGE_PATH_PROPERTY_NAME = "legacyPackageDestinationPath";
		private const string NAME_PROPERTY_NAME = "packageName";
		private const string DISPLAY_NAME_PROPERTY = "displayName";
		private const string PACKAGE_VERSION_PROPERTY_NAME = "packageVersion";
		private const string UNITY_VERSION_PROPERTY_NAME = "unityVersion";
		private const string DESCRIPTION_PROPERTY_NAME = "description";
		private const string CATEGORY_PROPERTY_NAME = "category";
		private const string KEYWORDS_PROPERTY_NAME = "keywords";
		private const string DEPENDENCIES_PROPERTY_NAME = "dependencies";
		private const string ID_PROPERTY_NAME = "_id";

		private void OnEnable()
		{
			_sourcePathsReorderableList = new ReorderableList(
				serializedObject,
				serializedObject.FindProperty(SOURCE_PATHS_PROPERTY_NAME))
			{
				drawHeaderCallback = DrawSourcePathHeader,
				drawElementCallback = DrawSourcePathElement,
				elementHeight = EditorConstants.FOLDER_PATH_PICKER_HEIGHT
			};

			_excludePathsReorderableList = new ReorderableList(
				serializedObject,
				serializedObject.FindProperty(EXCLUDE_PATHS_PROPERTY_NAME))
			{
				drawHeaderCallback = DrawExcludePathHeader,
				drawElementCallback = DrawExcludePathElement,
				elementHeight = EditorConstants.FOLDER_PATH_PICKER_HEIGHT
			};

			_keywordReorderableList =new ReorderableList(
				serializedObject,
				serializedObject.FindProperty(KEYWORDS_PROPERTY_NAME))
			{
				drawHeaderCallback = DrawKeywordsHeader,
				drawElementCallback = DrawKeywordElement
			};

			var dependencyProp = serializedObject.FindProperty(DEPENDENCIES_PROPERTY_NAME);
			_dependenciesReorderableList = new ReorderableList(
				serializedObject,
				dependencyProp)
			{
				drawHeaderCallback = DrawDependencyHeader,
				drawElementCallback = DrawDependencyElement,
				elementHeight = EditorGUIUtility.singleLineHeight * 2f
			};
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.LabelField(EditorConstants.PACKAGE_JSON_HEADER, EditorStyles.boldLabel);
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty(ID_PROPERTY_NAME));
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(serializedObject.FindProperty(NAME_PROPERTY_NAME));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(DISPLAY_NAME_PROPERTY));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(PACKAGE_VERSION_PROPERTY_NAME));

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty(UNITY_VERSION_PROPERTY_NAME));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(serializedObject.FindProperty(DESCRIPTION_PROPERTY_NAME));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(CATEGORY_PROPERTY_NAME));

			_keywordReorderableList.DoLayoutList();
			_dependenciesReorderableList.DoLayoutList();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(EditorConstants.PACKAGE_CONTENT_HEADER, EditorStyles.boldLabel);

			_sourcePathsReorderableList.DoLayoutList();
			_excludePathsReorderableList.DoLayoutList();

			// Package Source Export
			EditorGUILayout.BeginHorizontal();
			var destinationPathProperty = serializedObject.FindProperty(DESTINATION_PATH_PROPERTY_NAME);
			EditorGUILayout.PropertyField(
				destinationPathProperty,
				GUILayout.Height(EditorConstants.FOLDER_PATH_PICKER_HEIGHT));
			GUILayoutTools.DrawFolderPickerLayout(
				destinationPathProperty,
				EditorConstants.SELECT_PACKAGE_EXPORT_PATH_PICKER_TITLE);
			EditorGUILayout.EndHorizontal();

			// Legacy Package Export
			EditorGUILayout.BeginHorizontal();
			var legacyPackagePathProperty = serializedObject.FindProperty(LEGACY_PACKAGE_PATH_PROPERTY_NAME);
			EditorGUILayout.PropertyField(
				legacyPackagePathProperty,
				GUILayout.Height(EditorConstants.FOLDER_PATH_PICKER_HEIGHT));
			GUILayoutTools.DrawFolderPickerLayout(
				legacyPackagePathProperty,
				EditorConstants.SELECT_PACKAGE_EXPORT_PATH_PICKER_TITLE);
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(EditorConstants.PACKAGE_ACTIONS_HEADER, EditorStyles.boldLabel);

			if (GUILayout.Button(EditorConstants.UPDATE_PACKAGE_BUTTON_TEXT))
			{
				FileTools.CreateOrUpdatePackageSource((PackageManifestConfig)target);
			}

			if (GUILayout.Button(EditorConstants.EXPORT_LEGACY_PACKAGE_BUTTON_TEXT))
			{
				UnityFileTools.CompileLegacyPackage((PackageManifestConfig)target);
			}
		}

		#region Source Paths ReorderableList

		private void DrawSourcePathHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, EditorConstants.SOURCE_PATHS_HEADER_LABEL, EditorStyles.boldLabel);
		}

		private void DrawSourcePathElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			DrawPathElement(SOURCE_PATHS_PROPERTY_NAME, rect, index, isActive, isFocused);
		}

		private void DrawExcludePathHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, EditorConstants.IGNORE_PATHS_HEADER_LABEL, EditorStyles.boldLabel);
		}

		private void DrawExcludePathElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			DrawPathElement(EXCLUDE_PATHS_PROPERTY_NAME, rect, index, isActive, isFocused);
		}

		private void DrawPathElement(string propertyName, Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.width -= EditorConstants.FOLDER_PATH_PICKER_HEIGHT * 2;
			rect.height = EditorConstants.FOLDER_PATH_PICKER_HEIGHT;
			var sourcePathRect = new Rect(rect);

			var sourcePathProperty =
				serializedObject.FindProperty(propertyName).GetArrayElementAtIndex(index);
			EditorGUI.PropertyField(
				sourcePathRect,
				sourcePathProperty,
				new GUIContent(string.Format(EditorConstants.SOURCE_PATH_ELEMENT_LABEL_FORMAT, index)));

			var filePickerRect = new Rect {
				position = new Vector2(
					sourcePathRect.width + EditorConstants.FOLDER_PATH_PICKER_BUFFER,
					sourcePathRect.position.y),
				width = EditorConstants.FOLDER_PATH_PICKER_HEIGHT,
				height = EditorConstants.FOLDER_PATH_PICKER_HEIGHT,
			};

			var folderPickerRect = new Rect {
				position = new Vector2(
					sourcePathRect.width + EditorConstants.FOLDER_PATH_PICKER_HEIGHT + EditorConstants.FOLDER_PATH_PICKER_BUFFER,
					sourcePathRect.position.y),
				width = EditorConstants.FOLDER_PATH_PICKER_HEIGHT,
				height = EditorConstants.FOLDER_PATH_PICKER_HEIGHT,
			};

			GUILayoutTools.DrawFilePicker(
				filePickerRect,
				sourcePathProperty,
				EditorConstants.SELECT_SOURCE_PATH_FILE_PICKER_TITLE);

			GUILayoutTools.DrawFolderPicker(
				folderPickerRect,
				sourcePathProperty,
				EditorConstants.SELECT_SOURCE_PATH_PICKER_FOLDER_TITLE);
		}

		#endregion

		#region Keywords ReorderableList

		private void DrawKeywordsHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, EditorConstants.KEYWORDS_HEADER_LABEL, EditorStyles.boldLabel);
		}

		private void DrawKeywordElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			EditorGUI.PropertyField(
				rect,
				serializedObject.FindProperty(KEYWORDS_PROPERTY_NAME).GetArrayElementAtIndex(index),
				new GUIContent(string.Format(EditorConstants.KEYWORD_ELEMENT_LABEL_FORMAT, index)));
		}

		#endregion

		#region Dependencies ReorderableList

		private void DrawDependencyHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, EditorConstants.DEPENDENCY_HEADER_LABEL, EditorStyles.boldLabel);
		}

		private void DrawDependencyElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			// TODO This needs to be replaced or a custom drawer made
			EditorGUI.PropertyField(
				rect,
				serializedObject.FindProperty(DEPENDENCIES_PROPERTY_NAME).GetArrayElementAtIndex(index),
				new GUIContent(string.Format(EditorConstants.DEPENDENCY_ELEMENT_LABEL_FORMAT, index))
				);
		}

		#endregion
	}
}
