using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : SingletonBase<GUIController>
{
    public event Action onGUI;

    private void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            onGUI?.Invoke();
        }
    }
}
