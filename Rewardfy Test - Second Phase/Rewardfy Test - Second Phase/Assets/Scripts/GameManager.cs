using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : SingletonBase<GameManager>
{
    public event System.Action<IMap> OnStartProgram;

    [SerializeField] private MapDrawerDataBase drawerData;
    [SerializeField] private ApplicationMapController app;

    private IMap map;
    private AppTargetPlatform platform;
    private IAppInteractionHandler controller;

    public ApplicationMapController App => app;

    public void Start()
    {
        LoopUtility.Starter = this;

        UIManager.instance.GetPanel<HUD>().OnSelectMapSize += UpdateGridSize;
        UIManager.instance.GetPanel<HUD>().OnResetButtonClicked += () => map.Reset(map.Width, map.Height);
    }

    private void UpdateGridSize(int x, int y)
    {
        if (map == null) 
        {
            map = new HexagonMap(x,y);
            OnStartProgram?.Invoke(map);

            platform = new MobileApp(map);
            controller = new MobileAppInteractionRule();
            app.MyStart(map, platform, controller);
        }
        else 
        {
            map.Reset(x,y);
        }
    }
}
