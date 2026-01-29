using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMechanics : MonoBehaviour
{
    public float car1Offset = 0f;
    public float car2Offset = 0;
    private Vector3 spawnPointStartPos;



    public static GameMechanics Instance { get; private set; }

    public List<GameObject> Cars;
    public GameObject SpawnPoint;
    public GameObject currentCar;
    public GameObject pendulum;
    public GameObject handle;
    public float forceStrength = 3f;
    public float highestStackY = 18f;
    public float craneOffset = 2.5f;
    public float craneMoveSpeed = 2f;
    public float x, y;
    public bool isGameOver = false;
    public List<GameObject> totalcars;

    /*-----------*/
    [SerializeField] private float heightIncrease = 1.5f;
    public Vector3 pendulumStartPos;
    private Quaternion pendulumStartRot;

    private Vector3 cameraStartPos;
    public float cameraStartSize;
    float lastAngle = 0f;
    int swingDirection = 0; // +1 = right, -1 = left

    public Camera mainCamera;
    public bool isIdle = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        mainCamera = Camera.main;

        // Save camera default values ONCE
        cameraStartPos = mainCamera.transform.position;
        cameraStartSize = mainCamera.orthographicSize;

        // Save pendulum defaults
        pendulumStartPos = pendulum.transform.position;
        pendulumStartRot = pendulum.transform.rotation;

        spawnPointStartPos = SpawnPoint.transform.localPosition;
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        ResetGameState();
        isGameOver = false;
        SpawnCars();
        SwingMotion.Instance.stopSwing = false;
        mainCamera = Camera.main;

        // // SAVE INITIAL STATES
        // pendulumStartPos = pendulum.transform.position;
        // pendulumStartRot = pendulum.transform.rotation;

        // cameraStartPos = mainCamera.transform.position;
        // cameraStartSize = mainCamera.orthographicSize;
    }

    void LateUpdate()
    {
        if (currentCar != null)
        {
            currentCar.transform.position = SpawnPoint.transform.position;
            currentCar.transform.rotation = SpawnPoint.transform.rotation;
        }
    }

    void Update()
    {
        float currentAngle = pendulum.transform.localEulerAngles.x;

        if (currentAngle > 180f)
            currentAngle -= 360f;

        float delta = currentAngle - lastAngle;

        if (delta > 0.01f)
            swingDirection = 1;
        else if (delta < -0.01f)
            swingDirection = -1;

        lastAngle = currentAngle;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (currentCar == null) return;
            CameraControl.Instance.PlayBump();
            ReleaseRustic();
            isIdle = false;
        }
    }
    public void SpawnCars()
    {
        if (currentCar != null) return;

        Debug.Log("Spawning Car ..");
        int index = UnityEngine.Random.Range(0, Cars.Count);
        SpawnPoint.transform.localPosition = spawnPointStartPos;

        if (index == 0)
        {
            SpawnPoint.transform.localPosition += Vector3.up * car1Offset;
        }
        else if (index == 2)
        {
            SpawnPoint.transform.localPosition += Vector3.down * car2Offset;
        }

        currentCar = Instantiate(Cars[index], SpawnPoint.transform.position, SpawnPoint.transform.rotation, AppManager.Instance.GameLogic.transform);
        //  currentCar.transform.SetParent(SpawnPoint.transform);
        Rigidbody rb = currentCar.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;



        DetectLanding landing = currentCar.GetComponent<DetectLanding>();
        if (landing != null)
        {
            landing.hasLanded = false;
        }

        Debug.Log("Current Car after Spawning:" + currentCar);
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
    //     currentCar.transform.SetParent(null);
    //     // rb.isKinematic = false;
    //     rb.useGravity = true;
    //     rb.freezeRotation = false;
    //     currentCar = null;
    // }


    void ReleaseRustic()
    {

        if (currentCar == null) return;
        Rigidbody rb = currentCar.GetComponent<Rigidbody>();

        RopeController ropeController = currentCar.GetComponentInChildren<RopeController>();

        if (ropeController != null)
        {
            ropeController.DisableRope();
        }
        else
        {
            Debug.LogWarning("RopeController not found on currentCar!");
        }


        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = false;

        Debug.Log("------------->Before release velocity: " + rb.linearVelocity);

        Vector3 forceDir;

        if (swingDirection == 1)
        {

            forceDir = Vector3.right;
        }
        else
        {
            forceDir = Vector3.left;
        }

        rb.AddForce(forceDir * forceStrength, ForceMode.Impulse);
        // rb.AddForce(Vector3.down * 1.5f, ForceMode.Impulse);

        currentCar = null;
        Debug.Log("Current Car after release:" + currentCar);
    }

    public void AdjustHeight()
    {
        Vector3 pos = pendulum.transform.position;
        pendulum.transform.position = new Vector3(pos.x, pos.y + heightIncrease, pos.z);

        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize += heightIncrease;
        }

        Debug.Log("Pendulum & Camera height increased!");
    }

    public void UpdateScore()
    {
        if (DataManager.Instance) DataManager.Instance.UpdateScore();
        if (DataManager.Instance.HasHighScore()) ShowCelebrationEffect();
    }

    public void GameOver()
    {
        if (isGameOver == true)
        {
            SwingMotion.Instance.stopSwing = true;
            Time.timeScale = 0;
            Debug.Log("Game Over");
            ResetGameState();
            CameraControl.Instance.SwitchToDefaultCamera();
            // Destroy(r.gameObject);
            GamePlayManager.Instance.GameOver();
        }
    }
    private void ShowCelebrationEffect()
    {


        AppStateManager.Instance.ShowOverlay("HighScorePopUp");
        Debug.Log("-> New High Score Reached DURING GAMEPLAY");
        // if (celebrationEffect != null)
        // {
        //     var celebration = Instantiate(celebrationEffect, Camera.main.transform, false);
        //     celebration.transform.position = Vector3.up;
        //     celebration.Play();
        //     AppStateManager.Instance.ShowOverlay("NewHighScore");
        //     DOTween.Sequence().AppendInterval(2f).AppendCallback(() =>
        //     {
        //         celebration.Stop();
        //         Destroy(celebration.gameObject);
        //     });

    }
    public void ResetGameState()
    {
        if (PlatformExtender.Instance != null)
        {
            PlatformExtender.Instance.ResetPlatform();
        }

        // Reset pendulum
        pendulum.transform.position = pendulumStartPos;
        pendulum.transform.rotation = pendulumStartRot;

        // Reset camera
        mainCamera.transform.position = cameraStartPos;
        mainCamera.orthographicSize = cameraStartSize;

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
