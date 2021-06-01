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
				EditorGUIUtility.IconContent(EditorConstants.EDITOR_FOLDER_ICON)))
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
				EditorGUIUtility.IconContent(EditorConstants.EDITOR_FILE_ICON)))
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
				EditorGUIUtility.IconContent(EditorConstants.EDITOR_FOLDER_ICON),
				GUILayout.Width(EditorConstants.FOLDER_PATH_PICKER_HEIGHT),
				GUILayout.Height(EditorConstants.FOLDER_PATH_PICKER_HEIGHT)))
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
