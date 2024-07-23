using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer3D : IMapDrawer
{
    private IMap map;
    private MapDrawerData_3D data;

    private int[,] templateMap;//it's used to know the right node template by its index
    private Matrix4x4 matrix;
    private MaterialPropertyBlock[] materialBlocks; // is set in the start; stores the templates' MaterialPropertyBlock
    private Dictionary<Vector2Int, MaterialPropertyBlock> AnimatedBlocks;//used to store block that are still being animated
    private Color clear = Color.clear;
    private Coroutine coroutine;

    public MapDrawer3D(MapDrawerData_3D data)
    {
        this.data = data;
        AnimatedBlocks = new Dictionary<Vector2Int, MaterialPropertyBlock>();
        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(data.eulerRotation), Vector3.one * data.size);
        SetMaterialPropertyBlocks();
    }

    public void StartBehaviour(IMap map)
    {
        this.map = map;
        templateMap = new int[map.Width, map.Height];
        map.OnReset += (x, y) => templateMap = new int[map.Width, map.Height];

        Toggle(true);
    }

    public void Toggle(bool toggle)
    {
        if (toggle != (coroutine != null))
        {
            coroutine = LoopUtility.Loop(Draw);
        }
        else
        {
            LoopUtility.Stop(coroutine);
        }
    }

    public void Draw()
    {
        MaterialPropertyBlock block;
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                matrix = matrix.SetPosition(map[x, y].Position);
                block = templateMap[x, y] == -1 ? AnimatedBlocks[new Vector2Int(x, y)] : materialBlocks[templateMap[x, y]];
                Graphics.DrawMesh(data.hexagonMesh, matrix, data.hexagonMaterial, 0, null, 0, block, false, false, false);
            }
        }
    }

    public void ChangeTileTemplate(Vector2Int coordenate, CellTemplateType template)
    {
        templateMap[coordenate.x, coordenate.y] = (int)template;
    }

    public IEnumerator ChangeCellColor(Vector2Int coordenate, CellTemplateType template)
    {
        float animDuration = template == CellTemplateType.Path_Reached ? 0.38f : 0.0f;
        templateMap[coordenate.x, coordenate.y] = -1;
        var block = new MaterialPropertyBlock();
        AnimatedBlocks[coordenate] = block;

        yield return LoopUtility.Tween((t) => SetMaterialPropertyBlock(block, data.templates[(int)template], t), animDuration);

        AnimatedBlocks.Remove(coordenate);

        if (templateMap[coordenate.x, coordenate.y] == -1)
        {
            templateMap[coordenate.x, coordenate.y] = (int)template;
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

    private void SetMaterialPropertyBlock(MaterialPropertyBlock block, CellVisualTemplate template, float normalizedTime)
    {
        foreach (var change in template.changes)
        {
            block.SetColor(change.ColorName, Color.Lerp(clear, change.ColorValue, normalizedTime));
        }
    }

    private void SetMaterialPropertyBlock(MaterialPropertyBlock block, CellVisualTemplate template)
    {
        foreach (var change in template.changes)
        {
            block.SetColor(change.ColorName, change.ColorValue);
        }
    }
}
