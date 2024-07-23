using UnityEngine;

public class MobileAppVisualDTO
{
    public float multiplier;
    public Rect gridArea;
    public Rect gridRect;

    private IMap map;

    public MobileAppVisualDTO(IMap map)
    {
        this.map = map;

        map.OnReset += (x, y) => UpdateSize();

        UpdateSize();
    }

    public Vector2 GetGUIPosition(ICell cell)
    {
        return new Vector2(gridRect.x + cell.Position.x * multiplier, gridRect.y - (cell.Position.y + 1) * multiplier);
    }

    private void UpdateSize()
    {
        gridArea = new Rect((Screen.width - Screen.height) * 0.5f, 0, Screen.height, Screen.height);
        multiplier = Mathf.Min(gridArea.width / (map.Height + 0.5f), gridArea.height / (map.Width * 0.75f + 0.25f));
        gridRect = new Rect(Screen.width * 0.5f - map.Size.x * multiplier * 0.5f, Screen.height * 0.5f + map.Size.y * multiplier * 0.5f, map.Size.x, map.Size.y);
    }
}