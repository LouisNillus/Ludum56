using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpritePreviewAttribute))]
public class SpritePreviewDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        property.objectReferenceValue = EditorGUI.ObjectField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            label,
            property.objectReferenceValue,
            typeof(Sprite),
            allowSceneObjects: false
        );

        if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue is Sprite)
        {
            Sprite sprite = property.objectReferenceValue as Sprite;
            if (sprite != null)
            {
                Rect previewRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 5, 64, 64);

                EditorGUI.DrawPreviewTexture(previewRect, AssetPreview.GetAssetPreview(sprite));

                position.height = EditorGUIUtility.singleLineHeight + 70;
            }

        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue is Sprite)
        {
            return EditorGUIUtility.singleLineHeight + 70;
        }

        return EditorGUIUtility.singleLineHeight;
    }
}
