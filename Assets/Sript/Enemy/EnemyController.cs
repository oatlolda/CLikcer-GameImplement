using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int _enemyhealth = 50;
    public int EnemyHealth { get { return _enemyhealth; } set { _enemyhealth = value; } }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.Attacked, GotDamage);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Attacked, GotDamage);
    }
    private void GotDamage()
    {
        Debug.Log("GotDamage work");
        _enemyhealth -= 10;
        if (_enemyhealth <= 0)
        {
            gameObject.SetActive(false);
            GameEventBus.Publish(GameEventType.Defeated);
        }
    }
   
}
