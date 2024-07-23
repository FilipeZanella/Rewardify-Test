using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MapDrawerData/3d", order = 1)]
public class MapDrawerData_3D : MapDrawerDataBase
{
    [Header("Tile")]
    public Mesh hexagonMesh;
    public Vector3 eulerRotation;
    public float size;
    [Header("Material")]
    public Material hexagonMaterial;
    public CellVisualTemplate[] templates; //the templates' indexes are ordered by the HexagonVisualTemplateType enum
}
