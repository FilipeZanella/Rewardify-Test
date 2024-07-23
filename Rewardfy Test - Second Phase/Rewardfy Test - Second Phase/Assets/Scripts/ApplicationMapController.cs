using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class ApplicationMapController : MonoBehaviour, IAppController
{
    public event Action<Vector2Int, Vector2Int, int> OnSelectPath;

    [SerializeField] private LineRenderer line;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip obstacleSound;
    [SerializeField] private AudioClip pathSound;
    [SerializeField] private float pathSoundInitialPitch;
    [SerializeField] private float pathSoundPitchIncrement;

    private IMap map;
    private IMapInputHandler inputHandler;
    private IAppInteractionHandler appController;
    private IMapDrawer drawer;
    private float lastPitch;

    private Coroutine coroutine;
    private Vector2Int? initialValue;
    IPathFinder pathFinder;
    IList<ICell> path;

    IPathDrawer pathDrawer;
    IPathDrawer detailsDrawer;
    HUD.PathMode pathMode;

    public bool IsAnimating { get => RightPathDrawer.IsAnimating; }

    private IPathDrawer RightPathDrawer => pathMode == HUD.PathMode.DisplayPath ? pathDrawer : detailsDrawer;  

    private void Start()
    {
        UIManager.instance.GetPanel<HUD>().OnResetButtonClicked += ResetBehaviour;
        UIManager.instance.GetPanel<HUD>().OnSelectMapSize += (x, y) => ResetBehaviour();
        UIManager.instance.GetPanel<HUD>().OnChangePathMode += (mode) =>
        {
            RightPathDrawer.CleanPath();
            pathMode = mode;
        };
    }

    public void MyStart(IMap map, AppTargetPlatform platform, IAppInteractionHandler controller)
    {
        this.map = map;
        appController = controller;

        if (drawer != null)
        {
            StopCoroutine(coroutine);
        }

        pathDrawer = platform.CreatePathDrawer();
        pathFinder = new DetailPathDrawer(this, pathDrawer);
        detailsDrawer = pathFinder as DetailPathDrawer;
        
        inputHandler = platform.CreateInputHandler();
        drawer = platform.CreateMapDrawer();
        drawer.StartBehaviour(map);
        coroutine = StartCoroutine(appController.Behaviour(this, inputHandler));
    }

    private void ResetBehaviour()
    {
        initialValue = null;
        path = null;
    }

    public void AddObstacle(Vector2Int index)
    {
        if (IsAnimating)
            return;
        map[index.x, index.y].Status = CellStatus.Obstacle;
        StartCoroutine(drawer.ChangeCellColor(index, CellTemplateType.Obstacle));
        audioSource.clip = obstacleSound;
        audioSource.Play();
    }

    public void FreeCell(Vector2Int index)
    {
        map[index.x, index.y].Status = CellStatus.Empty;
        StartCoroutine(drawer.ChangeCellColor(index, CellTemplateType.Default));
    }

    public void SelectCell(Vector2Int index)
    {
        if (!map.IsObstacle(index.x, index.y))
        {
            if (initialValue != null)
            {
                if (!RightPathDrawer.IsAnimating)
                {
                    path = pathFinder.FindPathOnMap(map[initialValue.Value.x, initialValue.Value.y], map[index.x, index.y], map);

                    if (path.Count != 0)
                    {
                        audioSource.clip = pathSound;
                        lastPitch = audioSource.pitch;
                        audioSource.pitch = pathSoundInitialPitch;

                        StartCoroutine(RightPathDrawer.Start(path, this));

                        OnSelectPath?.Invoke(initialValue.Value, index, path.Count);
                        
                        initialValue = index;
                    }
                }
            }
            else
            {
                initialValue = index;
                StartCoroutine(drawer.ChangeCellColor(index, CellTemplateType.Path_Reached));
            }
        }
    }

    public void RemoveObstacle(Vector2Int index)
    {
        FreeCell(index);
        audioSource.clip = obstacleSound;
        audioSource.Play();
    }

    public void SelectPathCell(Vector2Int index)
    {
        StartCoroutine(drawer.ChangeCellColor(index, CellTemplateType.Path_Reached));

        if (path[path.Count - 1].Coordenate == index) 
        {
            audioSource.pitch = lastPitch;
        }
        else
        {
            audioSource.pitch += pathSoundPitchIncrement;
        }

        audioSource.Play();
    }

    public void PaintPathfinder(Vector2Int index, CellTemplateType type)
    {
        if ((int)type > 3) 
        {
            StartCoroutine(drawer.ChangeCellColor(index, type));
        }
    }
}