using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MapDrawerGUI : MonoBehaviour, IMapDrawer
{
    [SerializeField] private Texture texture;
    [SerializeField] private Material[] materials;

    private IMap map;
    private int[,] colorMap;
    //private Rect gridRect;
    public float zoom = 1;

    private MobileAppVisualDTO dto;

    public void StartBehaviour(IMap map)
    {
        this.map = map;
        dto = new MobileAppVisualDTO(map);
        //int height = Screen.height;
        //gridRect = new Rect(Screen.width * 0.5f - map.Size.x * dto.multiplier * 0.5f, height * 0.5f + map.Size.y * dto.multiplier * 0.5f, map.Size.x, map.Size.y);

        map.OnReset += (x, y) => colorMap = new int[x, y]; 

        colorMap = new int[map.Width, map.Height];
    }

    private void OnGUI()
    {
        if (Event.current.type == EventType.Repaint && map != null)
        {
            DrawHexGrid(map.Width, map.Height);
        }
    }
    private void DrawHexGrid(int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var guiPosition = dto.GetGUIPosition(map[x,y]);

                Rect rect = new Rect(guiPosition.x, guiPosition.y, dto.multiplier, dto.multiplier);
                Graphics.DrawTexture(rect, texture, materials[colorMap[x, y]]);
            }
        }
    }

    public IEnumerator ChangeCellColor(Vector2Int coordenate, CellTemplateType template)
    {
        colorMap[coordenate.x, coordenate.y] = (int)template;

        yield break;
    }
}
