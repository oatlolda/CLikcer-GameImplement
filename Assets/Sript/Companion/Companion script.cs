using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Companionscript : MonoBehaviour
{
    [SerializeField] private float DelayAttck;
    [SerializeField]private float _damage =2;
    public float Damage { get { return _damage; } set { _damage = value; } }
    private EnemyController enemyController;

    [SerializeField] private int Staterprice;
    public Button button;//companion upgrade
    public TextMeshProUGUI HUD;
    public TextMeshProUGUI CoinNeed;
    private bool isSpawned = false;
    private MeshRenderer mesh;
    private Coroutine attackRoutine;
    private bool Canupgrade = false;
    private int Checkcoin;
    private int _upgradeneed=10;
    private int Upgradecount = 0;

    float bonus = 10;
    private void Start()
    {
       
        mesh = GetComponent<MeshRenderer>();


        

        // ตรวจสอบสถานะการเกิด: ถ้ายังไม่เกิด ให้ซ่อน Mesh ทันที
        if (!isSpawned)
        {
            _upgradeneed = Staterprice;
            mesh.enabled = false;
            if (HUD != null) HUD.text = "";
        }
        else
        {
            mesh.enabled = true;
        }
        UpdateUI();
        UpdateCoin();
    }
    private void Update()
    {
        
        UpdateCoin();
    }

    protected virtual IEnumerator Autoattack()
    {
        while (isSpawned)
        {
            // ถ้าไม่มีเป้าหมาย หรือเป้าหมายเลือดหมด
            if (enemyController == null || enemyController.EnemyHealth <= 0)
            {
                enemyController = Object.FindAnyObjectByType<EnemyController>();

                if (enemyController == null || enemyController.EnemyHealth <= 0)
                {
                    yield return new WaitForSeconds(0.2f); // รอให้ระบบ Spawn เสร็จสมบูรณ์
                    continue;
                }
            }

            // โจมตี
            enemyController.TakeDamage(Damage);

            // รอคูลดาวน์ (ใช้ yield return นี้เพื่อให้ Loop ไม่ทำงานหนักเกินไป)
            yield return new WaitForSeconds(DelayAttck);
        }
    }
    private void UpdateCoin()
    {
        Checkcoin = StatusManager.Instance.GetCoin();
        Debug.Log($"Companion Check: Coin={Checkcoin}, Need={_upgradeneed}");
        if (Checkcoin >= _upgradeneed)
        {
            Canupgrade = true;
            button.interactable = true;
        }
        else
        {
            Canupgrade = false;
            button.interactable = false;
        }
    }
    private void UpdateUI()
    {
        UpdateCoin();

        if (HUD != null)
        {
            HUD.text = "DMG " + Damage.ToString("F1");
        }
        if (CoinNeed != null)
        {
            CoinNeed.text = _upgradeneed.ToString();
        }
    }
    public void Upgrade()
    {
        Checkcoin = StatusManager.Instance.GetCoin();
      
        if (Checkcoin >= _upgradeneed)
        {
            
            Upgradecount++;
            StatusManager.Instance.SetCoin(Checkcoin - _upgradeneed);

            if (!isSpawned)
            {
                isSpawned = true;
                mesh.enabled = true;
                if (attackRoutine != null) StopCoroutine(attackRoutine);
                attackRoutine = StartCoroutine(Autoattack());
                Debug.Log("Companion Purchased!");
            }
            else
            {
                if (Upgradecount  < 10)
                {
                  
                    Damage += bonus;
                }
                else
                {
                    bonus += 20;
                    Damage += bonus;
                    Upgradecount = 0;
                    Debug.Log(Upgradecount);
                }
              
            }

            // --- จุดที่แก้ไข ---
            // คำนวณราคาใหม่โดยปัดเศษขึ้นเสมอ เพื่อป้องกันราคาค้างที่ 1
            float nextPrice = _upgradeneed * 1.22f;

            // ถ้าผลลัพธ์ใหม่เท่ากับค่าเดิม (กรณีราคาต่ำมาก) ให้บวกเพิ่มเอง 1
            if (Mathf.CeilToInt(nextPrice) <= _upgradeneed)
            {
                _upgradeneed += 1;
            }
            else
            {
                _upgradeneed = Mathf.CeilToInt(nextPrice);
            }
            // ------------------

            UpdateUI();
        }
    }
    private void RefreshTarget()
    {
        enemyController = null;
        // หยุดยิงชั่วคราว 0.1 วินาที เพื่อให้ระบบ Spawner เซ็ตค่าบอสใหม่ให้เสร็จก่อน
        StopAllCoroutines();
        if (isSpawned) attackRoutine = StartCoroutine(AutoattackDelay());
    }
    IEnumerator AutoattackDelay()
    {
        yield return new WaitForSeconds(0.2f); // พักรบแป๊บนึงตอนสลับบอส
        yield return Autoattack();
    }
    private void OnEnable()
    {
        // เมื่อบอสใหม่เกิด หรือ มอนสเตอร์ตาย ให้ล้างค่าเป้าหมายเดิมทิ้งซะ
        GameEventBus.Subscribe(GameEventType.BossState, RefreshTarget);
        GameEventBus.Subscribe(GameEventType.Defeated, RefreshTarget);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.BossState, RefreshTarget);
        GameEventBus.Unsubscribe(GameEventType.Defeated, RefreshTarget);
    }
}
