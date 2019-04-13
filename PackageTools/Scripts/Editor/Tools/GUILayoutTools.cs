using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for Unity Editor GUI.
	/// </summary>
	internal static class GUILayoutTools
	{
		/// <summary>
		/// Draw a folder picker button in <see cref="Rect"/> <paramref name="rect"/> that allows setting a
		/// relative folder path value on <see cref="SerializedProperty"/> <paramref name="property"/>.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="property"></param>
		/// <param name="title"></param>
		public static void DrawFolderPicker(Rect rect, SerializedProperty property, string title)
		{
			if (GUI.Button(
				rect,
				EditorGUIUtility.IconContent(EditorConstants.EditorFolderIcon)))
			{
				var currentFolder = string.IsNullOrEmpty(property.stringValue)
					? string.Empty
					: property.stringValue;

				var path = EditorUtility.SaveFolderPanel(
					title,
					currentFolder,
					string.Empty);

				if (!string.IsNullOrEmpty(path))
				{
					var relativePath = FileTools.ConvertToRelativePath(path, Application.dataPath);
					property.stringValue = relativePath;
				}
			}
		}

		/// <summary>
		/// Draw a file picker button in <see cref="Rect"/> <paramref name="rect"/> that allows setting a
		/// relative file path value on <see cref="SerializedProperty"/> <paramref name="property"/>.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="property"></param>
		/// <param name="title"></param>
		public static void DrawFilePicker(Rect rect, SerializedProperty property, string title)
		{
			if (GUI.Button(
				rect,
				EditorGUIUtility.IconContent(EditorConstants.EditorFileIcon)))
			{
				var currentFile = string.IsNullOrEmpty(property.stringValue)
					? string.Empty
					: property.stringValue;

				var path = EditorUtility.OpenFilePanel(
					title,
					currentFile,
					string.Empty);

				if (!string.IsNullOrEmpty(path))
				{
					var relativePath = FileTools.ConvertToRelativePath(path, Application.dataPath);
					property.stringValue = relativePath;
				}
			}
		}

		/// <summary>
		/// Draw a folder picker button using <see cref="GUILayout"/> that allows setting a
		/// relative folder path value on <see cref="SerializedProperty"/> <paramref name="property"/>.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="title"></param>
		public static void DrawFolderPickerLayout(SerializedProperty property, string title)
		{
			if (GUILayout.Button(
				EditorGUIUtility.IconContent(EditorConstants.EditorFolderIcon),
				GUILayout.Width(EditorConstants.FolderPathPickerHeight),
				GUILayout.Height(EditorConstants.FolderPathPickerHeight)))
			{
				var currentFolder = string.IsNullOrEmpty(property.stringValue)
					? string.Empty
					: property.stringValue;

				var path = EditorUtility.SaveFolderPanel(
					title,
					currentFolder,
					string.Empty);

				if (!string.IsNullOrEmpty(path))
				{
					var relativePath = FileTools.ConvertToRelativePath(path, Application.dataPath);
					property.stringValue = relativePath;
				}
			}
		}
	}
}
