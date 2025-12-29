using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMechanics : MonoBehaviour
{
    public static GameMechanics Instance { get; private set; }

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

    public bool isGameOver = false;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        ResetGameState();
        isGameOver = false;
        SpawnCars();
        SwingMotion.Instance.stopSwing = false;
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            ReleaseRustic();
        }
    }
    public void SpawnCars()
    {
        if (currentCar != null) return;

        Debug.Log("Spawning Car ..");
        int index = UnityEngine.Random.Range(0, Cars.Count);
        currentCar = Instantiate(Cars[index], SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        currentCar.transform.SetParent(SpawnPoint.transform);
        DetectLanding landing = currentCar.GetComponent<DetectLanding>();
        if (landing != null)
        {
            landing.hasLanded = false;
        }
    }

    // void ReleaseRustic()
    // {
    //     if (currentCar == null)
    //     {
    //         Debug.Log("No car ..");
    //         return;
    //     }
    //     Debug.Log("Releasing Car ..");
    //     var rb = currentCar.GetComponent<Rigidbody>();
    //     float angleZ = pendulum.transform.localEulerAngles.z;
    //     float angleX = pendulum.transform.localEulerAngles.x;

    //     // if (angleZ > 180f)
    //     //     angleZ -= 360f;
    //     // if (angleX > 180f)
    //     //     angleX -= 360f;

    //     rb.isKinematic = false;
    //     rb.useGravity = true;
    //     rb.freezeRotation = false;
    //     currentCar.transform.SetParent(null);

    //     currentCar = null;


    //     // // Check which axis has larger swing
    //     // if (Math.Abs(angleZ) > Math.Abs(angleX))
    //     // {
    //     //     isStraight = angleZ >= 0;
    //     //     //StartCoroutine(CheckPos());
    //     // }
    //     // else
    //     // {
    //     //   //  StartCoroutine(CheckXRotation());
    //     // }
    // }

    // private IEnumerator CheckPos()
    // {
    //     x = pendulum.transform.localEulerAngles.z;
    //     yield return new WaitForSeconds(0.1f);
    //     y = pendulum.transform.localEulerAngles.z;

    //     var rb = currentCar.GetComponent<Rigidbody>();
    //     if (!isStraight)
    //     {
    //         if ((Math.Abs(x) - Math.Abs(y)) < 0)
    //         {
    //             rb.AddForce(forceStrength * Vector3.right, ForceMode.Impulse);
    //         }
    //         else
    //         {
    //             rb.AddForce(forceStrength * Vector3.left, ForceMode.Impulse);
    //         }
    //     }
    //     else
    //     {
    //         if ((x - y) > 0)
    //         {
    //             rb.AddForce(forceStrength * Vector3.left, ForceMode.Impulse);
    //         }
    //         else
    //         {
    //             rb.AddForce(forceStrength * Vector3.right, ForceMode.Impulse);
    //         }
    //     }
    //     currentCar = null;
    // }
    // public IEnumerator CheckXRotation()
    // {
    //     float xStart = pendulum.transform.localEulerAngles.x;
    //     yield return new WaitForSeconds(0.1f);
    //     float xEnd = pendulum.transform.localEulerAngles.x;

    //     var rb = currentCar.GetComponent<Rigidbody>();

    //     if ((xStart - xEnd) > 0)
    //     {
    //         rb.AddForce(forceStrength * Vector3.forward, ForceMode.Impulse);
    //     }
    //     else
    //     {
    //         rb.AddForce(forceStrength * Vector3.back, ForceMode.Impulse);
    //     }
    //     currentCar = null;
    // }

    void ReleaseRustic()
    {
        if (currentCar == null) return;

        Rigidbody rb = currentCar.GetComponent<Rigidbody>();


        currentCar.transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = false;


        float angle = pendulum.transform.localEulerAngles.x;
        if (angle > 180f)
            angle -= 360f;


        Vector3 forceDir = angle < 0 ? Vector3.right : Vector3.left;



        rb.AddForce(forceDir * forceStrength , ForceMode.Impulse);
        rb.AddForce(Vector3.down * 0.6f, ForceMode.Impulse);

        currentCar = null;
    }


    public void GameOver()
    {
        if (isGameOver == true)
        {
            SwingMotion.Instance.stopSwing = true;
            Time.timeScale = 0;
            Debug.Log("Game Over");
            ResetGameState();
            GamePlayManager.Instance.GameOver();
        }
    }
    public void ResetGameState()
    {
        PlatformExtender.Instance.ResetPlatform();
        if (currentCar != null)
        {
            Destroy(currentCar);
            currentCar = null;
        }
        var allCar = FindObjectsByType<DetectLanding>(FindObjectsSortMode.None);
        foreach (var r in allCar)
        {
            Destroy(r.gameObject);
        }
    }

}
