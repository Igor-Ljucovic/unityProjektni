using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Rigidbody2D rb;

    private CinemachineFramingTransposer transposer;


    //      CAMERA NUMBERS:


    private float fallingSpeedThresholdToLowerCamera = -14f;
    private float maxSmoothing = 30f;
    private float normalSmoothing = 7f;
    private float cameraVerticalUnitsLoweredPerFrame = 0.01f;
    private float cameraVerticalUnitsLiftedPerFrame = 0.01f;
    private float cameraMainGameOrthographicSize = 6.5f;


    private void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        if (Game.Paused())
            return;
        
        LowerOrLiftCameraCheck(); // ove 2 funkcije ustvari vise ne rade ja msm
        SmoothDashCameraCheck();
    }

    public void ResetSettings()
    {
        transposer.m_TrackedObjectOffset = new Vector3(0, 0, -10f);   // idk why, but it has to be "-10", because otherwise, some objects become invisible
        transposer.m_ScreenY = 0.5f;
        transposer.m_ScreenX = 0.5f;
        transposer.m_SoftZoneWidth = 0.4f;
        transposer.m_SoftZoneHeight = 0.13f;
        Invoke("ResetLookAheadTime", 0.05f);
        virtualCamera.Follow = rb.transform;
        virtualCamera.LookAt = rb.transform;
        virtualCamera.m_Lens.OrthographicSize = cameraMainGameOrthographicSize;
    }

    // fixes the problem of the camera still following the player after the "MainGame" has loaded
    private void ResetLookAheadTime() => transposer.m_LookaheadTime = 0.22f;
    
    private void SmoothDashCameraCheck() => 
        transposer.m_LookaheadSmoothing = Time.time - playerMovement.dashStartTime < playerMovement.dashDuration ? maxSmoothing : normalSmoothing;
    
    private void LowerOrLiftCameraCheck()
    {
        if (playerMovement.GetPlayerFallingSpeed() < fallingSpeedThresholdToLowerCamera && transposer.m_ScreenY > 0.25f)
            transposer.m_ScreenY -= cameraVerticalUnitsLoweredPerFrame;
        else if (playerMovement.IsGrounded() && transposer.m_ScreenY < 0.5f)
            transposer.m_ScreenY += cameraVerticalUnitsLiftedPerFrame;
    }
}