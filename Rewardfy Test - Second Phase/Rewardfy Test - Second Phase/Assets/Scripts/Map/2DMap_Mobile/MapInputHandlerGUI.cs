using UnityEngine;

public class MapInputHandlerGUI : IMapInputHandler
{
    private IMap map;
    private MobileAppVisualDTO dto;

    public MapInputHandlerGUI(IMap map, MobileAppVisualDTO dto)
    {
        this.map = map;
        this.dto = dto;
    }

    ICell IMapInputHandler.GetSelectedCell()
    {
        var transformedPoint = new Vector2((Input.mousePosition.x - dto.gridRect.x) / dto.multiplier -0.5f, (Input.mousePosition.y - (Screen.height - dto.gridRect.y)) / dto.multiplier - 0.5f);

        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                var cell = map[x, y];
                

                if (cell.Shape.IsPointInside(cell, transformedPoint))
                {
                    return cell;
                }
            }
        }

        return null;
    }
}