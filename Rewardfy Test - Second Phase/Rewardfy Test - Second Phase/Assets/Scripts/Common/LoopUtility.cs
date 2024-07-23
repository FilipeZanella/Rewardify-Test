using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoopUtility 
{
    private static MonoBehaviour starter;
    public static MonoBehaviour Starter 
    {
        set
        {
            if (!starter)
            {
                starter = value;
            }
        }
    }

    public static Coroutine Start(IEnumerator coroutine)
    {
        return starter.StartCoroutine(coroutine);
    }

    public static void Stop(Coroutine coroutine)
    {
        starter.StopCoroutine(coroutine);
    }

    public static Coroutine Loop(Action action) 
    {
        return starter.StartCoroutine(LoopCoroutine(action));
    }

    public static IEnumerator LoopCoroutine(Action action) 
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
