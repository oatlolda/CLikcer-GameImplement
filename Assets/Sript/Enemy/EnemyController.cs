using System;
using UnityEngine;
using  UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    private MeshFilter targetMeshFilter;
    public Mesh[] enemyModel;
    public int _maxhealth = 100;
    public int _enemyhealth;
    private int _currentIndexModel = 0;

    public int EnemyHealth { get { return _enemyhealth; } set { _enemyhealth = value; } }

    // public IObjectPool<EnemyController> Pool { get;set; }

    private void Start()
    {
        targetMeshFilter = GetComponent<MeshFilter>();
        ChangeMesh(0);
    }

    private void OnEnable()
    {
      
        _enemyhealth = _maxhealth;
        GameEventBus.Subscribe(GameEventType.Attacked, GotDamage);
       
    }
    private void OnDisable()
    {
        
        GameEventBus.Unsubscribe(GameEventType.Attacked, GotDamage);
    }

   
    public void ChangeMesh(int index)
    {
        if (index >= 0 && index < enemyModel.Length)
        {
            targetMeshFilter.mesh = enemyModel[index];
        }
    }
    private void GotDamage()
    {
        
        Debug.Log("GotDamage work");
        _enemyhealth -= StatusManager.Instance.GetPlayerDamage();
        _enemyhealth = Mathf.Max(_enemyhealth, 0);
        Debug.Log(EnemyHealth);
        if (_enemyhealth <= 0)
        {
            _currentIndexModel++;
            _maxhealth += (int)(_maxhealth * 0.1f);
            if (_currentIndexModel > enemyModel.Length)
            {
                _currentIndexModel = 0;
            }
           Debug.Log(_currentIndexModel);
            ChangeMesh(_currentIndexModel);

            GameEventBus.Publish(GameEventType.Defeated);
            _enemyhealth = _maxhealth;

        }
        GameEventBus.Publish(GameEventType.EnemyDamaged);
    }
   
}
