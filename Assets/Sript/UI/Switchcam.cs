using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Switchcam : MonoBehaviour
{
    public CinemachineCamera normalCam;
    public CinemachineCamera combatCam;
    public Volume volume;
    private DepthOfField Blurcam;

    public void SwitchToCombat()
    {
        if(volume.profile.TryGet<DepthOfField>(out Blurcam))
        {
            Blurcam.active = true;

            Blurcam.focusDistance.value = 3.71f;   // ระยะโฟกัส
            Blurcam.aperture.value = 1f;       // ความเบลอ (ยิ่งน้อยยิ่งเบลอ)
            Blurcam.focalLength.value = 28f;
        }
        normalCam.Priority = 0;
        combatCam.Priority = 10;
    }

    public void SwitchToNormal()
    {
        if (volume.profile.TryGet<DepthOfField>(out Blurcam))
        {
            Blurcam.active = true;

            Blurcam.focusDistance.value = 12.76f;   // ระยะโฟกัส
            Blurcam.aperture.value = 9f;       // ความเบลอ (ยิ่งน้อยยิ่งเบลอ)
            Blurcam.focalLength.value = 300f;
        }
        normalCam.Priority = 10;
        combatCam.Priority = 0;
    }
}
