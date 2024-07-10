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
    public Action OnResetButtonClicked;
    public event Action<int, int> OnSelectMapSize;

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
   
    private int width = 10;
    private int height = 10;

    IEnumerator Start()
    {
        widthPlus.onClick.AddListener(() => UpdateValue(ref width, 1, widthLabel));
        widthMinus.onClick.AddListener(() => UpdateValue(ref width, -1, widthLabel));
        heightPlus.onClick.AddListener(() => UpdateValue(ref height, 1, heightLabel));
        heightMinus.onClick.AddListener(() => UpdateValue(ref height, -1, heightLabel));

        reset.onClick.AddListener(() => OnResetButtonClicked?.Invoke());

        submitButton.onClick.AddListener(() => OnSelectMapSize?.Invoke(width, height));

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
