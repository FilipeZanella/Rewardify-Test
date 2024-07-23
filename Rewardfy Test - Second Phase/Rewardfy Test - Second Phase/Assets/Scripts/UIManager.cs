using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    public Action<int, int> OnUpdateGridSize;
    public Action OnResetButtonPressed;

    [SerializeField] private int initialGridWidth = 10;
    [SerializeField] private int initialGridHeight = 10;

    [SerializeField] private List<UIPanel> panels;

    public T GetPanel <T>() where T : UIPanel
    {
        foreach (var panel in panels) 
        {
            if (panel is T converted) 
            {
                return converted;
            }
        }

        return null;
    }
}
