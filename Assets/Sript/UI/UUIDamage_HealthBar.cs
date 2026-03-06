
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
            UpdateHPText(enemyController.EnemyHealth);
        }
    }
    public void UpdateHPText(float health) // เปลี่ยนรับค่าเป็น long
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" };
        int suffixIndex = 0;
        double displayHealth = health;

        // วนลูปหารทีละ 1000 จนกว่าค่าจะน้อยกว่า 1000 หรือหมด Array
        while (displayHealth >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            displayHealth /= 1000f;
            suffixIndex++;
        }

        // ถ้าไม่มีหน่วย (หลักหน่วย-ร้อย) ไม่ต้องมีทศนิยม, ถ้ามีหน่วยให้มีทศนิยม 1 ตำแหน่ง
        string format = (suffixIndex == 0) ? "F0" : "F1";
        EnemyHp.text = displayHealth.ToString(format) + suffixes[suffixIndex];
    }
    private void UpdateCount()
    {
        StatusManager.Instance.Updatecount();

        int currentCount = StatusManager.Instance.Enemycount;

        if (currentCount == 0)
        {
            EnemyCount.text = "Boss Stage";
            GameEventBus.Publish(GameEventType.BossState);
        }
        else
        {
            EnemyCount.text = currentCount.ToString() + "/8";
        }

      
        UpdateUI();
    }
    private void BossState()
    {
        EnemyCount.text = "Boss Stage";

       
        Invoke("UpdateUI", 0.05f);

    }
}
