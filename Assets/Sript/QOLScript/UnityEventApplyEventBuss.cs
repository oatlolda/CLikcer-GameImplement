using UnityEngine;
using UnityEngine.Events;

public class UnityEventApplyEventBuss : MonoBehaviour
{
    public UnityEvent BossEvent;
    public UnityEvent Defeat;
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.BossState, BossEventState);
        GameEventBus.Subscribe(GameEventType.Defeated,DefeatEvent);

    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.BossState, BossEventState);
        GameEventBus.Unsubscribe(GameEventType.Defeated,DefeatEvent);
    }
    private void BossEventState()
    {
        BossEvent.Invoke();
    }
    private void DefeatEvent()
    {
        Defeat.Invoke();
    }
    public void EnemyDamage()
    {
        GameEventBus.Publish(GameEventType.EnemyDamaged);
    }

}
