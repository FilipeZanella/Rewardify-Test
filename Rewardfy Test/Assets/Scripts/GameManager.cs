using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : SingletonBase<GameManager>
{
    public event System.Action<IMap> OnStartProgram;

    [SerializeField] private MapDrawerData drawerData;
    [SerializeField] private ApplicationMapController controller;

    private IMap map;
    private Camera mainCamera;
    private MapDrawer mapDrawer;
    private MapRaycaster raycaster;

    public void Start()
    {
        mainCamera = Camera.main;

        UIManager.instance.GetPanel<HUD>().OnSelectMapSize += UpdateGridSize;
        UIManager.instance.GetPanel<HUD>().OnResetButtonClicked += () => map.Reset(map.Width, map.Height);
    }

    private void UpdateGridSize(int x, int y)
    {
        if (map == null) 
        {
            map = new HexagonMap(x,y);
            OnStartProgram?.Invoke(map);

            raycaster = new MapRaycaster(mainCamera, map);
            mapDrawer = new MapDrawer(map, drawerData);
            controller.MyStart(map, mapDrawer, raycaster);
        }
        else 
        {
            map.Reset(x,y);
        }
    }
}
