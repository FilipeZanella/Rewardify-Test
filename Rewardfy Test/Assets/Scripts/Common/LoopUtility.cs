using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoopUtility 
{
    public static IEnumerator Loop(Action action) 
    {
        while (true) 
        {
            action();

            yield return null;
        }
    }

    public static IEnumerator Tween(Action<float> action, float duration, AnimationCurve curve = null) 
    {
        float time = 0f;

        action(0);

        while ((time += Time.deltaTime) < duration) 
        {
            action(curve?.Evaluate(time / duration) ?? time / duration);
            
            yield return null;
        }

        action(1);
    }
}
