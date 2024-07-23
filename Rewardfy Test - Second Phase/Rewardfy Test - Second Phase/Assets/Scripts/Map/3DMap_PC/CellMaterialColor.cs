using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CellMaterialColor
{
    [SerializeField] private string colorName;
    [SerializeField] private Color colorValue;

    public string ColorName => colorName;
    public Color ColorValue => colorValue;
}