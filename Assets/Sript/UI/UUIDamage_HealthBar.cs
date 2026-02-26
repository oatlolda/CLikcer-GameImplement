using System;
using TMPro;
using UnityEngine;

public class UIDamage_HealthBar : MonoBehaviour
{
   public EnemyController enemyController;

    public TextMeshProUGUI EnemyHp;
    private void Start()
    {

        UpdateUI();
    }
    private void OnEnable()
    {
        // ลงทะเบียนว่า "ถ้ามีการโจมตีเกิดขึ้น ให้ฉันอัปเดตตัวเลขด้วยนะ"
        GameEventBus.Subscribe(GameEventType.EnemyDamaged, UpdateUI);
        UpdateUI();
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.EnemyDamaged, UpdateUI);
    }
    private void UpdateUI()
    {
        // เปลี่ยนจาก StatusManager เป็นดึงจากตัว enemyController ที่เราลากใส่ไว้ตรงๆ
        if (enemyController != null && EnemyHp != null)
        {
            if(enemyController.EnemyHealth > 1000)
            {
                float enemyFloat = enemyController.EnemyHealth / 1000f;

                EnemyHp.text = enemyFloat.ToString("F1") + "K";
            }
            else
            {
                EnemyHp.text = enemyController.EnemyHealth.ToString();
            }
               
        }
    }
}
