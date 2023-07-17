using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{

    public void Delay(float t, Action f) => StartCoroutine(DelayTime(t, f));

    IEnumerator DelayTime(float time, Action fuction)
    {
        if (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
        fuction?.Invoke();
    }
}
