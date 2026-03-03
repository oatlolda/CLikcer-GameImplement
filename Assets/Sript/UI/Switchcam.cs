using Unity.Cinemachine;
using UnityEngine;

public class Switchcam : MonoBehaviour
{
    public CinemachineCamera normalCam;
    public CinemachineCamera combatCam;

    public void SwitchToCombat()
    {
        normalCam.Priority = 0;
        combatCam.Priority = 10;
    }

    public void SwitchToNormal()
    {
        normalCam.Priority = 10;
        combatCam.Priority = 0;
    }
}
