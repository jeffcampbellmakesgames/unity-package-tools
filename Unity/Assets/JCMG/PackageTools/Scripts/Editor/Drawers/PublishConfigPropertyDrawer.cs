using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// A custom property drawer for <see cref="PackageManifestConfig.PublishConfig"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(PackageManifestConfig.PublishConfig))]
	internal sealed class PublishConfigPropertyDrawer : PropertyDrawer
	{
		private const string PUBLISH_CONFIG_REGISTRY_PROPERTY_NAME = "registry";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var titleRect = new Rect(position)
			{
				height = EditorGUIUtility.singleLineHeight
			};

			var registryRect = new Rect(titleRect)
			{
				position = new Vector2(position.x, titleRect.y + titleRect.height)
			};

			EditorGUI.LabelField(titleRect, nameof(PackageManifestConfig.PublishConfig), EditorStyles.boldLabel);
			EditorGUI.PropertyField(registryRect, property.FindPropertyRelative(PUBLISH_CONFIG_REGISTRY_PROPERTY_NAME));
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 2f;
		}
	}
}
