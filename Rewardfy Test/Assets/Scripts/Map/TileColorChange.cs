using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct TileColorChange
{
    public string colorName;

    [SerializeField] private bool hdr;
    [ColorUsage(true, true)] [SerializeField] private Color HDRColor;
    [SerializeField] private Color color;

    public Color MyColor => hdr ? HDRColor : color;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TileColorChange))]
public class HexagonColorChangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //+30 in x and -30 in width is for a small bug in he inspector visual
        var colorNameRect = new Rect(position.x+30, position.y, position.width-30, EditorGUIUtility.singleLineHeight);
        var hdrRect = new Rect(position.x+30, position.y + EditorGUIUtility.singleLineHeight + 2, position.width-30, EditorGUIUtility.singleLineHeight);
        var colorRect = new Rect(position.x+30, position.y + (EditorGUIUtility.singleLineHeight + 2) * 2, position.width-30, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(colorNameRect, property.FindPropertyRelative("colorName"));
        EditorGUI.PropertyField(hdrRect, property.FindPropertyRelative("hdr"));

        var hdr = property.FindPropertyRelative("hdr").boolValue;
        if (hdr)
        {
            EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("HDRColor"));
        }
        else
        {
            EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("color"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Set the height of the property based on the number of fields
        return EditorGUIUtility.singleLineHeight * 3 + 4;
    }
}
#endif