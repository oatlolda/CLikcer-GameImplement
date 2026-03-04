using System.Collections;
using Unity.Cinemachine;

using UnityEngine;


public class CameraShake : Observer
{
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeRoutine;

    [SerializeField] private float duration = 0.2f;
    [SerializeField]
    private float amplitude = 2f;
    private void Start()
    {
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void OnEnable()
    {
        PlayerAttackState sm = FindAnyObjectByType<PlayerAttackState>();
        if (sm != null) sm.Attach(this);
        GameEventBus.Subscribe(GameEventType.Attacked, Shakecam);
        //GameEventBus.Subscribe(GameEventType.Attacked, changepostion);
    }
    private void OnDisable()
    {

        GameEventBus.Unsubscribe(GameEventType.Attacked, Shakecam);
      
        //GameEventBus.Unsubscribe(GameEventType.Attacked, changepostion);
    }
    
    public override void Notify(Subject subject)
    {
        if (subject is PlayerAttackState)
        {
            amplitude = 50f;
           
        }
       
    }
    private void Shakecam()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(Shake());
        Handheld.Vibrate();
    }

    private IEnumerator Shake()
    {
        
        noise.AmplitudeGain = amplitude;

        yield return new WaitForSeconds(duration);

        noise.AmplitudeGain = 0f;
        shakeRoutine = null;
    }





}
