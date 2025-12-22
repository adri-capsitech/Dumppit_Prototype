using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanics : MonoBehaviour
{
    public List<GameObject> Cars;
    public GameObject SpawnPoint;
    public GameObject currentCar;
    public GameObject pendulum;
    public float forceStrength = 3f;
    public float highestStackY = 18f;
    public float craneOffset = 2.5f;
    public float craneMoveSpeed = 2f;
    public float x, y;
    private bool isStraight = false;


    public static GameMechanics Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        SpawnCars();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ReleaseRustic();
        }
    }
    public void SpawnCars()
    {
        if (currentCar != null) return;

        int index = UnityEngine.Random.Range(0, Cars.Count);
        currentCar = Instantiate(Cars[index], SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        currentCar.transform.SetParent(SpawnPoint.transform);
    }
    void ReleaseRustic()
    {
        if (currentCar == null)
        {
            Debug.Log("No car ..");
            return;
        }
        var rb = currentCar.GetComponent<Rigidbody>();
        float angle = pendulum.transform.localEulerAngles.z;

        if (angle > 180f)
            angle -= 360f;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = false;
        currentCar.transform.SetParent(null);

        if (angle < 0)
        {
            isStraight = false;
            StartCoroutine(CheckPos());
        }

        else
        {
            isStraight = true;
            StartCoroutine(CheckPos());
        }
    }

    private IEnumerator CheckPos()
    {
        x = pendulum.transform.localEulerAngles.z;
        Debug.Log("-->x" + x);

        yield return new WaitForSeconds(0.1f);
        y = pendulum.transform.localEulerAngles.z;
        Debug.Log("-->y" + y);

        var rb = currentCar.GetComponent<Rigidbody>();
        if (!isStraight)
        {
            if ((Math.Abs(x) - Math.Abs(y)) < 0)
            {
                Debug.Log("Right to left");
                rb.AddForce(forceStrength * Vector3.right, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("Left to Right");
                rb.AddForce(forceStrength * Vector3.left, ForceMode.Impulse);
            }
        }
        else
        {
            if ((x - y) > 0)
            {
                Debug.Log("right to left");
                rb.AddForce(forceStrength * Vector3.left, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("Left to Right");
                rb.AddForce(forceStrength * Vector3.right, ForceMode.Impulse);
            }
        }
        currentCar = null;
    }


    // public void OnCarLanded(GameObject landedCar)
    // {
    //     float carTopY = landedCar.transform.position.y +
    //                     landedCar.GetComponent<Renderer>().bounds.extents.y;

    //     if (carTopY > highestStackY)
    //         highestStackY = carTopY;

    //     UpdateCraneAndCamera();
    // }
    // void UpdateCraneAndCamera()
    // {
    //     float targetY = highestStackY + craneOffset;

    //     Vector3 cranePos = SpawnPoint.transform.position;
    //     cranePos.y = Mathf.Lerp(
    //         cranePos.y,
    //         targetY,
    //         Time.deltaTime * craneMoveSpeed
    //     );

    //     SpawnPoint.transform.position = cranePos;
    // }
}
