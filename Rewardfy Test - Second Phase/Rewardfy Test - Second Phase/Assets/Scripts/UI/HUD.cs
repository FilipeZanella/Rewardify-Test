using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UIPanel
{
    public enum PathMode { DisplayPath, DisplayPathFinderDetails }

    public Action OnResetButtonClicked;
    public event Action<int, int> OnSelectMapSize;
    public event Action<PathMode> OnChangePathMode;

    [SerializeField] private Button reset;
    [SerializeField] private TMP_Text printLabel;
    [Header("Map Size Panel")]
    [SerializeField] private Button widthPlus;
    [SerializeField] private Button widthMinus;
    [SerializeField] private Button heightPlus;
    [SerializeField] private Button heightMinus;
    [SerializeField] private TMP_Text widthLabel;
    [SerializeField] private TMP_Text heightLabel;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button pathModeButton;
    [SerializeField] private TMP_Text pathLabel;
    [SerializeField] private Color simplePathModeColor;
    [SerializeField] private Color pathFinderPathModeColor;

    private int width = 7;
    private int height = 7;

    private bool isSimpleMode=true;

    IEnumerator Start()
    {
        widthPlus.onClick.AddListener(() => UpdateValue(ref width, 1, widthLabel));
        widthMinus.onClick.AddListener(() => UpdateValue(ref width, -1, widthLabel));
        heightPlus.onClick.AddListener(() => UpdateValue(ref height, 1, heightLabel));
        heightMinus.onClick.AddListener(() => UpdateValue(ref height, -1, heightLabel));

        reset.onClick.AddListener(() => OnResetButtonClicked?.Invoke());
        submitButton.onClick.AddListener(() => OnSelectMapSize?.Invoke(width, height));
        pathModeButton.onClick.AddListener(() => 
        {
            if (GameManager.instance.App.IsAnimating)
                return;

            isSimpleMode = !isSimpleMode;

            OnChangePathMode?.Invoke(isSimpleMode ? PathMode.DisplayPath : PathMode.DisplayPathFinderDetails);

            pathModeButton.image.color = isSimpleMode ? simplePathModeColor : pathFinderPathModeColor;
            pathLabel.color = isSimpleMode ? simplePathModeColor : pathFinderPathModeColor;
            pathLabel.text = isSimpleMode ? "Default" : "Details";
        });

        GameManager.instance.App.OnSelectPath += (ini, fin, length) =>
        {
            Print("Initial Cell: " + ini + ";    Final Cell: " + fin + ";    Path Length: "+ length);
        };

        yield return new WaitForEndOfFrame();

        OnSelectMapSize?.Invoke(width,height);
    }

    public void Print(string message) 
    {
        printLabel.text = message;
    }

    private void UpdateValue(ref int value, int inc, TMP_Text label)
    {
        value += inc;
        label.text = value.ToString();
    }
}
