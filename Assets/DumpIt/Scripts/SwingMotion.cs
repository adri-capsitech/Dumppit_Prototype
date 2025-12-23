using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    public static SwingMotion Instance { get; private set; }
    [Header("Pendulum Settings")]
    public float speed = 1.5f;      // Swing speed
    public float limit = 75f;       // Max rotation angle (degrees)
    public bool randomStart = false;
    private float randomOffset = 0f;
    public bool swingZ = true;
    public bool stopSwing = false;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (randomStart)
        {
            randomOffset = Random.Range(0f, 2f * Mathf.PI);
        }
    }
    void FixedUpdate()
    {
        if (stopSwing)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            return;
        }
        float angle = limit * Mathf.Sin(Time.time * speed + randomOffset);

        if (swingZ == true)
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);

    }
}