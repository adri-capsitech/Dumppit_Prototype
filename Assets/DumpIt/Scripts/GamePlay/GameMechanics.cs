using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMechanics : MonoBehaviour
{
    public float car1Offset = 0f;
    public float car2Offset = 0;
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
    public AudioClip releaseSound;
    public AudioClip gamePlayMusic;
    public AudioClip highScorePop;
    public float smoothTime = 0.5f; // duration of animation
    public float currentOrthoSize;
    public float zoomDuration = 0.8f;
    private Coroutine zoomCoroutine;
    public float edgeMargin = 100f; // margin from screen edges to ignore input
    public float upwardBias = 0.6f; // tweak between 0.5 - 0.8

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

        currentOrthoSize = mainCamera.orthographicSize;

    }
    public void StartGame()
    {
        AnalyticsLogger.LogGameStart();
        Time.timeScale = 1f;
        ResetGameState();
        isGameOver = false;
        SpawnCars();
        SwingMotion.Instance.stopSwing = false;
        mainCamera = Camera.main;
        if (AudioController.Instance != null)
            AudioController.Instance.PlayMusic(gamePlayMusic);
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
            // CameraControl.Instance.PlayBump();
            ReleaseRustic();
            isIdle = false;
        }
    }
    bool IsInsideSafeArea(Vector2 pos)

    {

        return pos.x > edgeMargin &&

               pos.x < Screen.width - edgeMargin &&

               pos.y > edgeMargin &&

               pos.y < Screen.height - edgeMargin;

    }

    public void SpawnCars()
    {
        if (currentCar != null) return;

        SpawnCollider spawnCollider = FindFirstObjectByType<SpawnCollider>();
        if (spawnCollider != null)
            spawnCollider.ResetTrigger();

        int index = UnityEngine.Random.Range(0, Cars.Count);

        currentCar = Instantiate(Cars[index], SpawnPoint.transform.position, SpawnPoint.transform.rotation, AppManager.Instance.GameLogic.transform);
        Rigidbody rb = currentCar.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;



        DetectLanding landing = currentCar.GetComponent<DetectLanding>();
        if (landing != null)
        {
            landing.hasLanded = false;
        }
    }

    void ReleaseRustic()
    {

        if (currentCar == null) return;

        if (AudioController.Instance != null)
            AudioController.Instance.PlaySFX(releaseSound);

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

        currentCar = null;

    }

    // public void AdjustHeight()
    // {
    //     Vector3 pos = pendulum.transform.position;
    //     pendulum.transform.position = new Vector3(pos.x, pos.y + heightIncrease, pos.z);

    //     if (mainCamera.orthographic)
    //     {
    //         mainCamera.orthographicSize += heightIncrease;
    //     }

    //     //Debug.Log("Pendulum & Camera height increased!");
    // }

    public void AdjustHeight()
    {
        // move pendulum instantly
        Vector3 pos = pendulum.transform.position;
        pendulum.transform.position = new Vector3(pos.x, pos.y + heightIncrease, pos.z);

        if (mainCamera.orthographic)
        {
            float targetSize = mainCamera.orthographicSize + heightIncrease;

            //  calculate target camera Y
            float targetY = mainCamera.transform.position.y + (heightIncrease * upwardBias);

            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }

            zoomCoroutine = StartCoroutine(SmoothZoomAndMove(targetSize, targetY));
        }
    }
    IEnumerator SmoothZoomAndMove(float targetSize, float targetY)
    {
        float startSize = mainCamera.orthographicSize;
        float startY = mainCamera.transform.position.y;

        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            float t = elapsed / zoomDuration;
            t = Mathf.SmoothStep(0, 1, t);

            // Zoom
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            // Move camera upward
            float newY = Mathf.Lerp(startY, targetY, t);
            Vector3 camPos = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(camPos.x, newY, camPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final snap (important)
        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = new Vector3(
            mainCamera.transform.position.x,
            targetY,
            mainCamera.transform.position.z
        );

        currentOrthoSize = targetSize;
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
            if (AudioController.Instance != null)
                AudioController.Instance.StopMusic();
            SwingMotion.Instance.stopSwing = true;
            Time.timeScale = 0;
            ResetGameState();
            CameraControl.Instance.SwitchToDefaultCamera();
            GamePlayManager.Instance.GameOver();
        }
    }
    private void ShowCelebrationEffect()
    {
        AppStateManager.Instance.ShowOverlay("HighScorePopUp");
        if (AudioController.Instance != null)
            AudioController.Instance.PlaySFX(highScorePop);

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
        currentOrthoSize = cameraStartSize;
        SwingMotion.Instance.swingZ = false;
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
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = null;
        }

    }

}
