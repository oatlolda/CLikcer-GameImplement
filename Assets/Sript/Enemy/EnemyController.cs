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

    // เอาเรื่องรีเซ็ตเลือดออกจากตรงนี้ให้หมด! เพราะเราย้ายไปทำใน TakeDamage แล้ว
    ChangeMesh(_currentIndexModel);
    Debug.Log($"Monster Mesh Changed to: {_currentIndexModel}");
}
    public void TakeDamage(float amount)
    {
        // 1. ถ้าเลือดเป็น 0 หรือติดสถานะตายอยู่ ไม่รับดาเมจเพิ่ม
        if (_enemyhealth <= 0) return;

        _enemyhealth = Mathf.Max(0, _enemyhealth - amount);
        GameEventBus.Publish(GameEventType.EnemyDamaged);

        if (_enemyhealth <= 0)
        {
            _enemyhealth = 0; // ล็อคเลือดไว้ที่ 0 ทันที

            if (StatusManager.Instance.Enemycount == 0) // บอสตาย
            {
                BossHp += BossHp * 0.4f;

               
                StatusManager.Instance.Enemycount = 1;

                _maxhealth = BossHp * 0.6f;          // รีเซ็ตกลับไปมอนปกติ
                _enemyhealth = _maxhealth;

                GameEventBus.Publish(GameEventType.EnemyDamaged);
                GameEventBus.Publish(GameEventType.Defeated); // ให้ UI อัปเดตรอบใหม่
            }
            else // กรณีมอนสเตอร์ทั่วไปตาย
            {
                GameEventBus.Publish(GameEventType.Defeated);

                // 1. รีเซ็ตเลือดมอนสเตอร์ตัวใหม่
                _maxhealth += (int)(_maxhealth * 0.1f);
                _enemyhealth = _maxhealth;

                // 2. *** สำคัญมาก *** ต้อง Publish หลังบรรทัดข้างบน เพื่อให้ UI เห็นค่าใหม่ที่ไม่ใช่ 0
                GameEventBus.Publish(GameEventType.EnemyDamaged);
            }
        }
    }

    public override void Notify(Subject subject)
    {
        PlayerAttackState sm = subject as PlayerAttackState;
        if (sm != null)
        {
            TakeDamage(StatusManager.Instance.GetCriticalDamage());



        }
    }

    private void BossState()
    {
        BossHp = Mathf.Max(BossHp, 500); // กันบัคเลือดน้อย
        _maxhealth = BossHp;
        _enemyhealth = _maxhealth;

        Debug.Log($"Boss Reset: {_enemyhealth} / {_maxhealth}");

        // ต้องมั่นใจว่า Publish หลังจากเซ็ตค่าข้างบนเสร็จแล้วเท่านั้น!
        GameEventBus.Publish(GameEventType.EnemyDamaged);
    }
}
