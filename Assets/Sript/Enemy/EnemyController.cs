using System;
using UnityEngine;
using  UnityEngine.Pool;

public class EnemyController : Observer
{
    private MeshFilter targetMeshFilter;
    public Mesh[] enemyModel;
    public int _maxhealth = 100;
    private int _enemyhealth;
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
        TakeDamage(StatusManager.Instance.GetPlayerDamage());
    }
    private void Death()
    {
        _currentIndexModel++;

        // แก้บั๊ก Index เกิน: ใช้ >= เพราะ Array เริ่มที่ 0
        if (_currentIndexModel >= enemyModel.Length)
        {
            _currentIndexModel = 0;
        }

        // เพิ่มความยากและรีเซ็ตเลือด
        _maxhealth += (int)(_maxhealth * 0.1f);
        _enemyhealth = _maxhealth;

        // เปลี่ยนรูปทรงมอนสเตอร์
        ChangeMesh(_currentIndexModel);

        // แจ้งระบบว่าชนะตัวเดิมแล้ว
        GameEventBus.Publish(GameEventType.Defeated);
       

        Debug.Log($"Monster Died! Next Mesh Index: {_currentIndexModel}");
    }
    public void TakeDamage(int amount)
    {

        if (_enemyhealth <= 0) return;

        // ลดเลือดแบบกันติดลบ
        _enemyhealth = Mathf.Max(0, _enemyhealth - amount);

        // แจ้ง UI อัพเดทหลอดเลือด
        GameEventBus.Publish(GameEventType.EnemyDamaged);

        // เช็คความตายตรงนี้ที่เดียว!
        if (_enemyhealth <= 0)
        {
            Death();
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
}
