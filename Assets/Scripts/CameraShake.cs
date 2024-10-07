using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool startRoutine = false;
    public float shakeDuration = 0.3f;
    public AnimationCurve curve;

    void Update()
    {
        if (startRoutine)
        {
            startRoutine = false;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 startPos = transform.position;
        float curTime = 0f;

        while (curTime < shakeDuration)
        {
            curTime += Time.deltaTime;
            float shakeStrength = curve.Evaluate(curTime / shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * shakeStrength;
            yield return null;
        }

        transform.position = startPos;
    }
}
