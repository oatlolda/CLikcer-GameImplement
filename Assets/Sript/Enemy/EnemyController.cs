using UnityEngine;
using  UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    private int _maxhealth = 100;
    private int _enemyhealth;
    public int EnemyHealth { get { return _enemyhealth; } set { _enemyhealth = value; } }

   public IObjectPool<EnemyController> Pool { get;set; }

   
    private void Start()
    {
       
    }
    private void OnEnable()
    {
        _enemyhealth = _maxhealth;
        GameEventBus.Subscribe(GameEventType.Attacked, GotDamage);
    }
    private void OnDisable()
    {
        ResetHealth();
        GameEventBus.Unsubscribe(GameEventType.Attacked, GotDamage);
    }

    private void ResetHealth()
    {
        _enemyhealth = _maxhealth  ;
    }

    private void ReturnPool()
    {
        Pool.Release(this);
    }
    private void GotDamage()
    {
        Debug.Log("GotDamage work");
        _enemyhealth -= 3;
        if (_enemyhealth <= 0)
        {
            ResetHealth() ;
          gameObject.SetActive(false);
            GameEventBus.Publish(GameEventType.Defeated);

        }
    }
   
}
