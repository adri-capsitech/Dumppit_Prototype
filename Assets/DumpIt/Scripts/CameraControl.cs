using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

    private Vector3 originalPos;
    private Coroutine bumpRoutine;

    [Header("Bump Settings")]
    public float bumpAmount = 0.4f;   // how much camera goes down
    public float bumpupTime = 0.08f;     // how fast

    public float returnBackTime = 1f;  // how fast it returns

    private void Awake()
    {
        Instance = this;
        originalPos = transform.position;
    }

    public void PlayBump()
    {
        if (bumpRoutine != null)
            StopCoroutine(bumpRoutine);

        bumpRoutine = StartCoroutine(BumpRoutine());
    }

    IEnumerator BumpRoutine()
    {
        Vector3 upPos = originalPos + new Vector3(0, bumpAmount, 0);

        // Move down
        float t = 0f;
        while (t < bumpupTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(originalPos, upPos, t / bumpupTime);
            yield return null;
        }

        // Move back up
        t = 0f;
        while (t < returnBackTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(upPos, originalPos, t / returnBackTime);
            yield return null;
        }

        transform.position = originalPos;
    }
}
