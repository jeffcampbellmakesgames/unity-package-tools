using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// A property drawer for <see cref="PackageManifestConfig.Repository"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(PackageManifestConfig.Repository))]
	internal sealed class RepositoryPropertyDrawer : PropertyDrawer
	{
		private const string REPOSITORY_TYPE_PROPERTY_NAME = "type";
		private const string REPOSITORY_URL_PROPERTY_NAME = "url";
		private const string REPOSITORY_DIRECTORY_PROPERTY_NAME = "directory";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var titleRect = new Rect(position)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			var typeRect = new Rect(titleRect)
			{
				position = new Vector2(position.x, titleRect.y + titleRect.height)
			};

			var urlRect = new Rect(typeRect)
			{
				position = new Vector2(position.x, typeRect.y + typeRect.height)
			};

			var directoryRect = new Rect(typeRect)
			{
				position = new Vector2(position.x, urlRect.y + urlRect.height)
			};

			EditorGUI.LabelField(titleRect, nameof(PackageManifestConfig.Repository), EditorStyles.boldLabel);
			EditorGUI.PropertyField(typeRect, property.FindPropertyRelative(REPOSITORY_TYPE_PROPERTY_NAME));
			EditorGUI.PropertyField(urlRect, property.FindPropertyRelative(REPOSITORY_URL_PROPERTY_NAME));
			EditorGUI.PropertyField(directoryRect, property.FindPropertyRelative(REPOSITORY_DIRECTORY_PROPERTY_NAME));
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 4f;
		}
	}
}
