using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer
{
    private MapDrawerData data;
    private IMap map;

    private int[,] templateMap;//it's used to know the right node template by its index
    private Matrix4x4 matrix;
    private MaterialPropertyBlock[] materialBlocks; // is set in the start; stores the templates' MaterialPropertyBlock
    private Dictionary<Vector2Int, MaterialPropertyBlock> AnimatedBlocks;//used to store block that are still being animated
    private Color clear = Color.clear;

    public MapDrawer(IMap map, MapDrawerData data)
    {
        this.data = data;
        this.map = map;
        AnimatedBlocks = new Dictionary<Vector2Int, MaterialPropertyBlock>();
        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(data.eulerRotation), Vector3.one * data.size);
        UpdateMapFromIMap();
        map.OnReset += (x,y) => UpdateMapFromIMap();
        SetMaterialPropertyBlocks();
    }

    public void ChangeTileTemplate (Vector2Int coordenate, VisualTemplateType template) 
    {
        templateMap[coordenate.x, coordenate.y] = (int)template;
    }

    public IEnumerator ChangeColorSmoothly(Vector2Int coordenate, VisualTemplateType template)
    {
        templateMap[coordenate.x, coordenate.y] = -1;
        var block = new MaterialPropertyBlock();
        AnimatedBlocks[coordenate] = block;

        yield return LoopUtility.Tween((t) => SetMaterialPropertyBlock(block, data.templates[(int)template], t), 0.23f);

        AnimatedBlocks.Remove(coordenate);

        if (templateMap[coordenate.x, coordenate.y] == -1)
        {
            templateMap[coordenate.x, coordenate.y] = (int)template;
        }
    }

    private void UpdateMapFromIMap() 
    {
        templateMap = new int[map.Width, map.Height];

        foreach (var cell in map)
        {
            cell.OnChangeStatus += (status) => ChangeTileTemplate(cell.Coordenate, (VisualTemplateType)(int)status);
        }
    }

    public void DrawMap()
    {
        MaterialPropertyBlock block;
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                matrix = matrix.SetPosition(map[x,y].Position);
                block = templateMap[x, y] == -1 ? AnimatedBlocks[new Vector2Int(x, y)] : materialBlocks[templateMap[x, y]];
                DrawNode(matrix, block);
            }
        }
    }

    private void SetMaterialPropertyBlocks()
    {
        materialBlocks = new MaterialPropertyBlock[data.templates.Length];
        for (int i = 0; i < materialBlocks.Length; i++)
        {
            materialBlocks[i] = new MaterialPropertyBlock();
            SetMaterialPropertyBlock(materialBlocks[i], data.templates[i]);
        }
    }

    private void SetMaterialPropertyBlock(MaterialPropertyBlock block, TileVisualTemplate template, float normalizedTime)
    {
        foreach (var change in template.changes)
        {
            block.SetColor(change.colorName, Color.Lerp(clear, change.MyColor, normalizedTime));
        }
    }

    private void SetMaterialPropertyBlock(MaterialPropertyBlock block, TileVisualTemplate template)
    {
        foreach (var change in template.changes)
        {
            block.SetColor(change.colorName, change.MyColor);
        }
    }

    private void DrawNode(Matrix4x4 matrix, MaterialPropertyBlock block)
    {
        Graphics.DrawMesh(data.hexagonMesh, matrix, data.hexagonMaterial, 0, null, 0, block, false, false, false);
    }
}
