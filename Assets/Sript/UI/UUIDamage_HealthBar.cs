using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDamage_HealthBar : MonoBehaviour
{
   public EnemyController enemyController;
    public Slider EnemyHealthBar;
    public TextMeshProUGUI EnemyHp;
    public TextMeshProUGUI EnemyCount;
    private void Start()
    {
        UpdateCount();
        UpdateUI();
    }
    private void OnEnable()
    {
        // ลงทะเบียนว่า "ถ้ามีการโจมตีเกิดขึ้น ให้ฉันอัปเดตตัวเลขด้วยนะ"
        GameEventBus.Subscribe(GameEventType.EnemyDamaged, UpdateUI);
        GameEventBus.Subscribe(GameEventType.Defeated, UpdateCount);
        GameEventBus.Subscribe(GameEventType.BossState, BossState);
        UpdateUI();

    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.EnemyDamaged, UpdateUI);
        GameEventBus.Unsubscribe(GameEventType.Defeated, UpdateCount);
        GameEventBus.Unsubscribe(GameEventType.BossState, BossState);
    }
    private void UpdateUI()
    {
        // หา Enemy ตัวที่ active อยู่ในฉากเพียงตัวเดียวให้เจอ
        EnemyController enemy = FindAnyObjectByType<EnemyController>();

        if (enemy == null) return;

        // 1. อัพเดทหลอดเลือด (ใช้ตัวแปร enemy ที่หาเจอเมื่อกี้)
        EnemyHealthBar.value = (float)enemy.EnemyHealth / enemy._maxhealth;

        // 2. อัพเดทตัวเลขเลือด (ใช้ตัวแปร enemy ตัวเดียวกัน!)
        if (EnemyHp != null)
        {
            if (enemy.EnemyHealth >= 1000) // ใช้ >= เพื่อความเป๊ะ
            {
                float enemyFloat = enemy.EnemyHealth / 1000f;
                EnemyHp.text = enemyFloat.ToString("F1") + "K";
            }
            else
            {
                EnemyHp.text = enemy.EnemyHealth.ToString("F0");
            }
        }
    }
    private void UpdateCount()
    {
        StatusManager.Instance.Updatecount();
        if (StatusManager.Instance.Enemycount != 0)
        {
            EnemyCount.text = StatusManager.Instance.Enemycount.ToString() + "/8";
        }
    }
    private void BossState()
    {
        EnemyCount.text = "Boss Stage";
      
    }

}
