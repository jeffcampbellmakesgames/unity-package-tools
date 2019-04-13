using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	[CustomEditor(typeof(PackageManifestConfig))]
	internal sealed class PackageManifestConfigInspector : UnityEditor.Editor
	{
		private ReorderableList _sourcePathsReorderableList;
		private ReorderableList _keywordReorderableList;
		private ReorderableList _dependenciesReorderableList;

		private const string SourcePathsPropertyName = "packageSourcePaths";
		private const string DestinationPathPropertyName = "packageDestinationPath";
		private const string NamePropertyName = "packageName";
		private const string DisplayNameProperty = "displayName";
		private const string PackageVersionPropertyName = "packageVersion";
		private const string UnityVersionPropertyName = "unityVersion";
		private const string DescriptionPropertyName = "description";
		private const string CategoryPropertyName = "category";
		private const string KeywordsPropertyName = "keywords";
		private const string DependenciesPropertyName = "dependencies";
		private const string IdPropertyName = "_id";

		private void OnEnable()
		{
			_sourcePathsReorderableList = new ReorderableList(
				serializedObject,
				serializedObject.FindProperty(SourcePathsPropertyName))
			{
				drawHeaderCallback = DrawSourcePathHeader,
				drawElementCallback = DrawSourcePathElement,
				elementHeight = EditorConstants.FolderPathPickerHeight
			};

			_keywordReorderableList =new ReorderableList(
				serializedObject,
				serializedObject.FindProperty(KeywordsPropertyName))
			{
				drawHeaderCallback = DrawKeywordsHeader,
				drawElementCallback = DrawKeywordElement
			};

			var dependencyProp = serializedObject.FindProperty(DependenciesPropertyName);
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

			EditorGUILayout.LabelField(EditorConstants.PackageJsonHeader, EditorStyles.boldLabel);
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty(IdPropertyName));
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.PropertyField(serializedObject.FindProperty(NamePropertyName));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(DisplayNameProperty));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(PackageVersionPropertyName));

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(serializedObject.FindProperty(UnityVersionPropertyName));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(serializedObject.FindProperty(DescriptionPropertyName));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(CategoryPropertyName));

			_keywordReorderableList.DoLayoutList();
			_dependenciesReorderableList.DoLayoutList();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(EditorConstants.PackageContentHeader, EditorStyles.boldLabel);

			_sourcePathsReorderableList.DoLayoutList();

			EditorGUILayout.BeginHorizontal();
			var destinationPathProperty = serializedObject.FindProperty(DestinationPathPropertyName);
			EditorGUILayout.PropertyField(
				destinationPathProperty,
				GUILayout.Height(EditorConstants.FolderPathPickerHeight));
			GUILayoutTools.DrawFolderPickerLayout(
				destinationPathProperty,
				EditorConstants.SelectPackageExportPathPickerTitle);
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(EditorConstants.PackageActionsHeader, EditorStyles.boldLabel);

			if (GUILayout.Button(EditorConstants.UpdatePackageButtonText))
			{
				FileTools.CreateOrUpdatePackageSource((PackageManifestConfig)target);
			}
		}

		#region Source Paths ReorderableList

		private void DrawSourcePathHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, EditorConstants.SourcePathsHeaderLabel, EditorStyles.boldLabel);
		}

		private void DrawSourcePathElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.width -= EditorConstants.FolderPathPickerHeight * 2;
			rect.height = EditorConstants.FolderPathPickerHeight;
			var sourcePathRect = new Rect(rect);

			var sourcePathProperty =
				serializedObject.FindProperty(SourcePathsPropertyName).GetArrayElementAtIndex(index);
			EditorGUI.PropertyField(
				sourcePathRect,
				sourcePathProperty,
				new GUIContent(string.Format(EditorConstants.SourcePathElementLabelFormat, index)));

			var filePickerRect = new Rect
			{
				position = new Vector2(
					sourcePathRect.width + EditorConstants.FolderPathPickerBuffer,
					sourcePathRect.position.y),
				width = EditorConstants.FolderPathPickerHeight,
				height = EditorConstants.FolderPathPickerHeight,
			};

			var folderPickerRect = new Rect
			{
				position = new Vector2(
					sourcePathRect.width + EditorConstants.FolderPathPickerHeight + EditorConstants.FolderPathPickerBuffer,
					sourcePathRect.position.y),
				width = EditorConstants.FolderPathPickerHeight,
				height = EditorConstants.FolderPathPickerHeight,
			};

			GUILayoutTools.DrawFilePicker(
				filePickerRect,
				sourcePathProperty,
				EditorConstants.SelectSourcePathFilePickerTitle);

			GUILayoutTools.DrawFolderPicker(
				folderPickerRect,
				sourcePathProperty,
				EditorConstants.SelectSourcePathPickerFolderTitle);
		}

		#endregion

		#region Keywords ReorderableList

		private void DrawKeywordsHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, EditorConstants.KeywordsHeaderLabel, EditorStyles.boldLabel);
		}

		private void DrawKeywordElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			EditorGUI.PropertyField(
				rect,
				serializedObject.FindProperty(KeywordsPropertyName).GetArrayElementAtIndex(index),
				new GUIContent(string.Format(EditorConstants.KeywordElementLabelFormat, index)));
		}

		#endregion

		#region Dependencies ReorderableList

		private void DrawDependencyHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, EditorConstants.DependencyHeaderLabel, EditorStyles.boldLabel);
		}

		private void DrawDependencyElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			// TODO This needs to be replaced or a custom drawer made
			EditorGUI.PropertyField(
				rect,
				serializedObject.FindProperty(DependenciesPropertyName).GetArrayElementAtIndex(index),
				new GUIContent(string.Format(EditorConstants.DependencyElementLabelFormat, index))
				);
		}

		#endregion
	}
}
