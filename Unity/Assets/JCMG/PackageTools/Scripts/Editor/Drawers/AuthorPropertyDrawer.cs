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
using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// A property drawer for drawing <see cref="PackageManifestConfig.Author"/>
	/// </summary>
	[CustomPropertyDrawer(typeof(PackageManifestConfig.Author))]
	internal sealed class AuthorPropertyDrawer : PropertyDrawer
	{
		private const string NAME_PROPERTY_NAME = "name";
		private const string EMAIL_PROPERTY_NAME = "email";
		private const string URL_PROPERTY_NAME = "url";

		private const string NAME_LABEL = "Author Name";
		private const string EMAIL_LABEL = "Author Email";
		private const string URL_LABEL = "Author URL";

		private readonly GUIContent _nameGUIContent;
		private readonly GUIContent _emailGUIContent;
		private readonly GUIContent _urlGUIContent;

		public AuthorPropertyDrawer()
		{
			_nameGUIContent = new GUIContent(NAME_LABEL);
			_emailGUIContent = new GUIContent(EMAIL_LABEL);
			_urlGUIContent = new GUIContent(URL_LABEL);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var nameRect = new Rect(position)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			var emailRect = new Rect(nameRect)
			{
				position = new Vector2(position.x, nameRect.y + nameRect.height + 2f)
			};

			var urlRect = new Rect(emailRect)
			{
				position = new Vector2(position.x, emailRect.y + emailRect.height + 2f)
			};

			EditorGUI.PropertyField(nameRect, property.FindPropertyRelative(NAME_PROPERTY_NAME), _nameGUIContent);
			EditorGUI.PropertyField(emailRect, property.FindPropertyRelative(EMAIL_PROPERTY_NAME), _emailGUIContent);
			EditorGUI.PropertyField(urlRect, property.FindPropertyRelative(URL_PROPERTY_NAME), _urlGUIContent);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 3f + 6f;
		}
	}
}
