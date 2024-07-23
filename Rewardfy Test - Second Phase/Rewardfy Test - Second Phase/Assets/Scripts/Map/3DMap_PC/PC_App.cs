using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PC_App : AppTargetPlatform
{
    private const string LINE_PREFAB_PATH = "Prefabs/Line";
    protected MapDrawerData_3D data;

    public PC_App(IMap map, MapDrawerData_3D data) : base(map)
    {
        this.data = data;
    }

    public override IMapInputHandler CreateInputHandler()
    {
        return new MapRaycaster(Camera.main, map);
    }

    public override IMapDrawer CreateMapDrawer()
    {
        return new MapDrawer3D(data);
    }

    public override IPathDrawer CreatePathDrawer()
    {
        var prefab = Resources.Load<LineRenderer>(LINE_PREFAB_PATH);
        var instance = Object.Instantiate(prefab);

        return new LinePathDrawer(instance);
    }
}
