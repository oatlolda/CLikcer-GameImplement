using System;
using UnityEngine;
using  UnityEngine.Pool;

public class EnemyController : Observer
{
    private MeshFilter targetMeshFilter;
    public Mesh[] enemyModel;
    [SerializeField] private float BossHp=500;
    public float _maxhealth = 100;
    private float _enemyhealth;
    private int _currentIndexModel = 0;

    public float EnemyHealth { get { return _enemyhealth; } set { _enemyhealth = value; } }

    // public IObjectPool<EnemyController> Pool { get;set; }

    private void Start()
    {
        
        targetMeshFilter = GetComponent<MeshFilter>();
        ChangeMesh(0);

      
    }

    private void OnEnable()
    {
        _enemyhealth = _maxhealth;
        GameEventBus.Subscribe(GameEventType.BossState, BossState);
        GameEventBus.Subscribe(GameEventType.Attacked, GotDamage);
        GameEventBus.Subscribe(GameEventType.Defeated, Death);

    }
    private void OnDisable()
    {
        
        GameEventBus.Unsubscribe(GameEventType.Attacked, GotDamage);
        GameEventBus.Unsubscribe(GameEventType.BossState, BossState);
        GameEventBus.Unsubscribe(GameEventType.Defeated, Death);
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
        TakeDamage(StatusManager.Instance.GetPlayerDamage());
    }
    private void Death()
    {
        _currentIndexModel++;

        
        if (_currentIndexModel >= enemyModel.Length)
        {
            _currentIndexModel = 0;
        }
      
        // เพิ่มความยากและรีเซ็ตเลือด
       
        if (StatusManager.Instance.Enemycount <= 8 && StatusManager.Instance.Enemycount != 0)
        {
            _maxhealth += (int)(_maxhealth * 0.1f);
            _enemyhealth = _maxhealth;
        }
        // เปลี่ยนรูปทรงมอนสเตอร์
        ChangeMesh(_currentIndexModel);

        // แจ้งระบบว่าชนะตัวเดิมแล้ว
      
       

        Debug.Log($"Monster Died! Next Mesh Index: {_currentIndexModel}");
    }
    public void TakeDamage(float amount)
    {

        if (_enemyhealth <= 0 && StatusManager.Instance.Enemycount != 0) return;

        _enemyhealth = Mathf.Max(0, _enemyhealth - amount);
        GameEventBus.Publish(GameEventType.EnemyDamaged);
      

        // เช็คความตายตรงนี้ที่เดียว!
        if (_enemyhealth <= 0)
        {
            GameEventBus.Publish(GameEventType.Defeated);
            GameEventBus.Publish(GameEventType.BossDefeated);
        }
    }

    public override void Notify(Subject subject)
    {
        SliderManager sm = subject as SliderManager;
        if (sm != null)
        {
            TakeDamage(StatusManager.Instance.GetCriticalDamage());



        }
    }

    private void BossState()
    {
        if (BossHp <= 0)
        {
            Debug.LogWarning("BossHp was 0! Resetting to default 500.");
            BossHp = 500f;
        }
        _maxhealth = BossHp;
        _enemyhealth = _maxhealth;
        Debug.Log($"{gameObject.name} -> Boss Spawned! MaxHP: {_maxhealth}, CurrentHP: {_enemyhealth}");
        // เตรียมเพิ่มพลังบอสสำหรับครั้งถัดไป
        BossHp += (BossHp * 1.5f);

        GameEventBus.Publish(GameEventType.EnemyDamaged);
    }
}
