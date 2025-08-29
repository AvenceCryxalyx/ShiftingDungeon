using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        Targeted = 0,
        Overview = 1,
    }
    [System.Serializable]
    public struct CameraModeSetting
    {
        public CameraMode Mode;
        public int ProjectionSize;
        public Vector2[] Offset;
    }

    #region Inspector Field
    [SerializeField]
    protected CameraModeSetting[] settings;
    [SerializeField]
    protected float viewChangeSpeed;
    [SerializeField]
    protected float cameraFollowSpeed = 2f;
    #endregion

    #region Fields
    protected Coroutine cameraViewActionCor;
    private Camera cam;
    private bool shouldFollowTarget = false;
    #endregion

    #region Properties
    public bool IsViewChanging { get; private set; }
    public GameObject TargetObject;
    public CameraMode Mode { get; private set; }
    #endregion

    private void Start()
    {
        cam = GetComponent<Camera>();
        ChangeCameraMode(CameraMode.Targeted, true);
        StartFollow();
    }

    #region Public Methods
    public void Initialize(GameObject target)
    {
        shouldFollowTarget = true;
        cam = Camera.main;
        TargetObject = target;
        StartFollow();
    }


    public void ChangeCameraMode(CameraMode newMode, bool instant = false)
    {
        if (cameraViewActionCor != null)
        {
            return;
        }
        IsViewChanging = true;
        Mode = newMode;
        CameraModeSetting selected = settings[(int)newMode];
        if (instant)
        {
            cam.orthographicSize = selected.ProjectionSize;
            IsViewChanging = false;
            return;
        }
        switch (selected.Mode)
        {
            case CameraMode.Targeted:
                cameraViewActionCor = StartCoroutine(UpdateCameraView(1, (Vector2)transform.position + settings[0].Offset[0]));
                break;
            case CameraMode.Overview:
                cameraViewActionCor = StartCoroutine(UpdateCameraView(5, (Vector2)transform.position + settings[1].Offset[0]));
                break;
            default:
                break;
        }
    }

    public void StartFollow()
    {
        if (cameraViewActionCor != null)
        {
            return;
        }
        if(TargetObject != null)
        {
            shouldFollowTarget = true;
            cameraViewActionCor = StartCoroutine(FollowTarget());
        }
    }

    public void StopFollow()
    {
        shouldFollowTarget = false;
        StopCoroutine(FollowTarget());
        cameraViewActionCor = null;
    }
    #endregion

    #region Private Methods
    private IEnumerator FollowTarget()
    {
        while (true)
        {
            Vector3 newPos = new Vector3(TargetObject.transform.position.x + settings[0].Offset[0].x, TargetObject.transform.position.y + settings[0].Offset[0].y, cam.transform.position.z);
            transform.position = Vector3.Slerp(transform.position, newPos, cameraFollowSpeed * Time.deltaTime);
            if (!shouldFollowTarget)
            {
                break;
            }
            yield return null;
        }
    }

    private IEnumerator UpdateCameraView(int projectionSize, Vector2 position)
    {
        while (true)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, projectionSize, viewChangeSpeed * Time.deltaTime);
            if (Mathf.Approximately(projectionSize, cam.orthographicSize))
            {
                cam.orthographicSize = projectionSize;
                break;
            }
            yield return null;
        }
        IsViewChanging = false;
        yield return null;
        cameraViewActionCor = null;
    }
    #endregion
}
