using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Misc : MonoBehaviour
{
    static Misc instance;

    public static void AssignInstance(Misc inst)
    {
        instance = inst;
    }
    public static float CalculateDistanceFromMouse(Transform transform)
    {
        return ((Vector2)(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition))).magnitude;
    }

    public static float roundTo(float number, float divisor)
    {
       return number - number % divisor + divisor * (number % divisor == 0 ? 1 : 0);
    }

    public static void Delay(float delayTime, Action Action) => instance.StartCoroutine(ExecuteDelay(delayTime, Action));


    static IEnumerator ExecuteDelay(float delayTime, Action Action)
    {
        yield return new WaitForSeconds(delayTime);
        Action();
    }
}
