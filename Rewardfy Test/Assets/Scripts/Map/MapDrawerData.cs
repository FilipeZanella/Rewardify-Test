using UnityEngine;

[System.Serializable]
public struct MapDrawerData 
{
    [Header("Tile")]
    public Mesh hexagonMesh;
    public Vector3 eulerRotation;
    public float size;
    [Header("Material")]
    public Material hexagonMaterial;
    public TileVisualTemplate[] templates; //the templates' indexes are ordered by the HexagonVisualTemplateType enum
}

[System.Serializable]
public struct TileVisualTemplate
{
    public TileColorChange[] changes;
}
