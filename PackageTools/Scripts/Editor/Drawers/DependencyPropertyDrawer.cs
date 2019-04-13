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
