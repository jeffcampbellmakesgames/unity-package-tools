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
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	[CustomPropertyDrawer(typeof(PackageManifestConfig.Dependency))]
	internal sealed class DependencyPropertyDrawer : PropertyDrawer
	{
		private const string PackageNamePropertyName = "packageName";
		private const string PackageVersionPropertyName = "packageVersion";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var packageRect = new Rect(position)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			var packageVersionRect = new Rect(packageRect)
			{
				position = new Vector2(position.x, packageRect.y + packageRect.height)
			};

			EditorGUI.PropertyField(packageRect, property.FindPropertyRelative(PackageNamePropertyName));
			EditorGUI.PropertyField(packageVersionRect, property.FindPropertyRelative(PackageVersionPropertyName));
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 2f;
		}
	}
}
