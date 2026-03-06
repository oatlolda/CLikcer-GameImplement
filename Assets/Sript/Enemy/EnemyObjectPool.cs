using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    private List<EnemyController> pooledEnemies = new List<EnemyController>();
    private int _currentIndex = 0;

    // --- เพิ่มตัวแปรเหล่านี้ ---
    private float currentMaxHealth = 100f;
    private float currentBossHp = 500f;

    private void Start()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject go = Instantiate(enemyPrefabs[i], transform.position, Quaternion.identity);
            EnemyController controller = go.GetComponent<EnemyController>();
            go.SetActive(false);
            pooledEnemies.Add(controller);
        }
        SpawnEnemy();
    }

    
    public void PrepareNextEnemy()
    {
        if (StatusManager.Instance.Enemycount == 0) // บอสตาย
        {
            currentBossHp += currentBossHp * 0.2f;
            currentMaxHealth = currentBossHp * 0.1f;
        }
        else // มอนปกติระเบิด
        {
            currentMaxHealth += currentMaxHealth * 0.1f;
        }
    }

    public void SpawnEnemy()
    {
        // ปิดตัวปัจจุบันก่อนขยับ Index (เพื่อความชัวร์)
        pooledEnemies[_currentIndex].gameObject.SetActive(true);

        _currentIndex = (_currentIndex + 1) % pooledEnemies.Count;

        EnemyController nextEnemy = pooledEnemies[_currentIndex];

        // ส่งค่า HP ที่คำนวณไว้แล้วให้ตัวใหม่
      

        nextEnemy.gameObject.SetActive(true);
        nextEnemy.transform.position = transform.position;

        GameEventBus.Publish(GameEventType.EnemyDamaged);
    }

    private void OnEnable() => GameEventBus.Subscribe(GameEventType.Defeated, SpawnEnemy);
    private void OnDisable() => GameEventBus.Unsubscribe(GameEventType.Defeated, SpawnEnemy);

    // --- เพิ่มฟังก์ชัน Save/Load ข้อมูลเลือด ---
   

}
