using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;
    private Coroutine bumpRoutine;
    private Vector3 defaultPos;
    private Quaternion defaultRot;
    private Vector3 currentBasePos;
    private Quaternion currentBaseRot;

    [Header("Bump Settings")]
    public float bumpAmount = 0.4f;   // how much camera goes down
    public float bumpupTime = 0.08f;     // how fast

    public float returnBackTime = 1f;  // how fast it returns

    [Header("Z Axis Camera Pose")]
    private Vector3 zAxisPos = new Vector3(10f, 7.5f, -8.5f);
    private Vector3 zAxisRot = new Vector3(22.173f, -50.35f, 0f);

    [Header("Camera Smooth Settings")]
    public float cameraMoveSpeed = 1f;

    public float defaultOrthoSize = 0f;

    private Coroutine cameraMoveRoutine;


    private void Awake()
    {
        Instance = this;
    }

    public void PlayBump()
    {
        if (bumpRoutine != null)
            StopCoroutine(bumpRoutine);

        bumpRoutine = StartCoroutine(BumpRoutine());
    }

    void Start()
    {
        defaultPos = transform.position;
        defaultRot = transform.rotation;

        currentBasePos = defaultPos;
        currentBaseRot = defaultRot;
        defaultOrthoSize = Camera.main.orthographicSize;

    }

    IEnumerator BumpRoutine()
    {
        Vector3 upPos = currentBasePos + new Vector3(0, bumpAmount, 0);

        // Move down
        float t = 0f;
        while (t < bumpupTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(currentBasePos, upPos, t / bumpupTime);
            yield return null;
        }

        // Move back up
        t = 0f;
        while (t < returnBackTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(upPos, currentBasePos, t / returnBackTime);
            yield return null;
        }

        transform.position = currentBasePos;
    }

    void StartCameraMove(Vector3 pos, Quaternion rot)
    {
        if (cameraMoveRoutine != null)
            StopCoroutine(cameraMoveRoutine);

        cameraMoveRoutine = StartCoroutine(SmoothMoveCamera(pos, rot));
    }


    IEnumerator SmoothMoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;
    }

    public void SwitchToDefaultCamera()
    {
        currentBasePos = defaultPos;
        currentBaseRot = defaultRot;
        Camera.main.orthographicSize = defaultOrthoSize;
        StartCameraMove(currentBasePos, currentBaseRot);
    }

    public void SwitchToZAxisCamera()
    {
        currentBasePos = zAxisPos;
        currentBaseRot = Quaternion.Euler(zAxisRot);

        StartCameraMove(currentBasePos, currentBaseRot);
    }


}
