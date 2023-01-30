using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;

    private void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strenght = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strenght;
            yield return null;
        }
        transform.position = startPosition;

    }

}
