using System;
using TMPro;
using UnityEngine;

public class UIDamage_HealthBar : MonoBehaviour
{
   public EnemyController enemyController;

    public TextMeshProUGUI EnemyHp;
    private void Start()
    {
        // โชว์เลือดครั้งแรกตอนเริ่มเกม
        UpdateUI();
    }
    private void OnEnable()
    {
        // ลงทะเบียนว่า "ถ้ามีการโจมตีเกิดขึ้น ให้ฉันอัปเดตตัวเลขด้วยนะ"
        GameEventBus.Subscribe(GameEventType.Attacked, UpdateUI);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Attacked, UpdateUI);
    }
    private void UpdateUI()
    {
        if (enemyController != null && EnemyHp != null)
        {
            EnemyHp.text = enemyController.EnemyHealth.ToString();
        }
    }
}
