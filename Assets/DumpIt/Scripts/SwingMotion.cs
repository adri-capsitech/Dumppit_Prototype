using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    public static SwingMotion Instance { get; private set; }
    [Header("Pendulum Settings")]
    public float speed = 1.5f;
    public float limit = 75f;
    public bool randomStart = false;
    private float randomOffset = 0f;
    public bool swingZ = false;
    public bool stopSwing = false;
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private bool zCameraApplied = false;


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
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }
    public void ResetSwing()
    {
        stopSwing = false;

        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;

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
        {
            transform.localRotation = initialLocalRotation * Quaternion.Euler(0f, 0f, angle);
            Debug.Log("Swinging in Z axis" + angle);

            if (!zCameraApplied)
            {
                CameraControl.Instance.SwitchToZAxisCamera();
                zCameraApplied = true;
            }
        }

        else
        {
            transform.localRotation = initialLocalRotation * Quaternion.Euler(angle, 0f, 0f);
            Debug.Log("Swinging in X axis" + angle);

            if (zCameraApplied)
            {
                CameraControl.Instance.SwitchToDefaultCamera();
                zCameraApplied = false;
            }
        }

    }
}