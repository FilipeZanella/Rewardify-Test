using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private float zoomSensibility;

    [SerializeField] private AnimationCurve moveAnimationCurve;

    private Camera _camera;
    private IMap map;

    private float maxOrtographicSize;
    Coroutine inputCoroutine;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnStartProgram += (m) =>
        {
            map = m;
            //inputCoroutine = StartCoroutine(CheckInput());
        };

        UIManager.instance.GetPanel<HUD>().OnSelectMapSize += (x,y) => OnUpdateGridSize();
    }

    IEnumerator CheckInput() 
    {
        float scroll;
        yield return LoopUtility.LoopCoroutine(() => 
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");

            _camera.orthographicSize -= scroll * zoomSensibility * Time.deltaTime;

            if (_camera.orthographicSize > maxOrtographicSize) 
            {
                _camera.orthographicSize = maxOrtographicSize;
            }
        });
    }

    void OnUpdateGridSize() 
    {
        Vector2 size = map.Size;
        float mapSize = Mathf.Max(size.x, size.y);
        _camera.orthographicSize = mapSize / 2f;
        maxOrtographicSize = _camera.orthographicSize * 1.38f;
        Transform _transform = transform;
        Vector3 oldPosition = _transform.position;
        Vector3 newPosition = new Vector3(size.x * 0.5f, size.y * 0.5f, -10);
        StartCoroutine(LoopUtility.Tween((t) => _transform.position = Vector3.Lerp(oldPosition, newPosition, t), 0.62f, moveAnimationCurve));
    }
}

