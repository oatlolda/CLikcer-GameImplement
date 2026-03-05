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
        if (enemyController == null) return;

        if (enemyController._maxhealth <= 0) return;

        float healthRatio = enemyController.EnemyHealth / enemyController._maxhealth;
        EnemyHealthBar.value = Mathf.Clamp01(healthRatio);

        if (EnemyHp != null)
        {
            if (enemyController.EnemyHealth >= 1000000000f)
            {
                // เช็คหลักล้านก่อน
                EnemyHp.text = (enemyController.EnemyHealth / 1000000000f).ToString("F1") + "B";
            }
            else if (enemyController.EnemyHealth >= 1000000)
            {
                // เช็คหลักล้านก่อน
                EnemyHp.text = (enemyController.EnemyHealth / 1000000f).ToString("F1") + "M";
            }
            else if (enemyController.EnemyHealth >= 1000)
            {
                // ถ้าไม่ถึงล้าน แต่ถึงพัน ให้ใช้ K
                EnemyHp.text = (enemyController.EnemyHealth / 1000f).ToString("F1") + "K";
            }
            
            else
        {
            EnemyHp.text = enemyController.EnemyHealth.ToString("F0");
        }
        }
    }
    private void UpdateCount()
    {
        StatusManager.Instance.Updatecount();

        int currentCount = StatusManager.Instance.Enemycount;

        if (currentCount == 0)
        {
            EnemyCount.text = "Boss Stage";
        }
        else
        {
            EnemyCount.text = currentCount.ToString() + "/8";
        }

        // --- เพิ่มบรรทัดนี้ ---
        // บังคับอัปเดต UI ทันทีหลังจากนับแต้มเสร็จ เพื่อดึงเลือดมอนตัวใหม่มาโชว์
        UpdateUI();
    }
    private void BossState()
    {
        EnemyCount.text = "Boss Stage";

        // ใช้ Invoke หรือรอเล็กน้อยเพื่อให้มั่นใจว่า EnemyController เซ็ตค่า BossHp เสร็จแล้วจริงๆ
        Invoke("UpdateUI", 0.05f);

    }

}
