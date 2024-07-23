using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MobileApp : AppTargetPlatform
{
    private const string MAP_DRAWER_PREFAB_PATH = "Prefabs/MapDrawerGUI";
    private const string LINE_PREFAB_PATH = "Prefabs/Line";

    public MobileAppVisualDTO dto { get; private set; }

    public MobileApp(IMap map) : base(map)
    {
        dto = new MobileAppVisualDTO(map);
    }

    public override IMapInputHandler CreateInputHandler()
    {
        return new MapInputHandlerGUI(map, dto);
    }

    public override IMapDrawer CreateMapDrawer()
    {
        var prefab = Resources.Load<MapDrawerGUI>(MAP_DRAWER_PREFAB_PATH);
        var instance = Object.Instantiate(prefab);

        return instance;
    }

    public override IPathDrawer CreatePathDrawer()
    {
        var prefab = Resources.Load<LineRenderer>(LINE_PREFAB_PATH);
        var instance = Object.Instantiate(prefab);

        var camera = Camera.main;

        return new LinePathDrawer(instance, (cell) =>
        {
            var guiPosition = dto.GetGUIPosition(cell);
            guiPosition += new Vector2(dto.multiplier * 0.5f, +dto.multiplier * 0.5f);

            var screenPosition = new Vector3(guiPosition.x / Screen.width, 1 - guiPosition.y / Screen.height, 10);
            var world = camera.ViewportToWorldPoint(new Vector2(screenPosition.x, screenPosition.y));

            return world;
        });
    }
}
