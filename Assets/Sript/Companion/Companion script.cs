using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Companionscript : MonoBehaviour
{
    public int companionID;
    [SerializeField] private float DelayAttck;
    [SerializeField]private float _damage =2;
    public float Damage { get { return _damage; } set { _damage = value; } }
    private EnemyController enemyController;

    [SerializeField] private int Staterprice;
    public Button button;//companion upgrade
    public Button AllinBtn;
    public TextMeshProUGUI HUD;
    public TextMeshProUGUI CoinNeed;
    public TextMeshProUGUI ShowLevel;
    private bool isSpawned = false;
    private MeshRenderer mesh;
    private Coroutine attackRoutine;
    
    private long Checkcoin;
    private long _upgradeneed =10;
    private int Upgradecount = 0;

    private int Level = 1;
    float bonus = 10;

    private void Awake()
    {
        mesh = GetComponentInChildren<MeshRenderer>(true);
    }
    private void Start()
    {

        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>(true);

        // ตรวจสอบสถานะการเกิด: ถ้ายังไม่เกิด ให้ซ่อน Mesh ทันที
        if (!isSpawned)
        {
            _upgradeneed = Staterprice;
            mesh.gameObject.SetActive(false);
            if (HUD != null) HUD.text = "";
        }
        else
        {
            mesh.gameObject.SetActive(true);
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
            AllinBtn.interactable = true;
            button.interactable = true;
        }
        else
        {
            AllinBtn.interactable = false;
            button.interactable = false;
        }
    }
    private void UpdateUI()
    {
        UpdateCoin();

        if (HUD != null)
        {
            if (Damage >= 1000000000f)
            {
                // เช็คหลักล้านก่อน
                HUD.text = "Dps:\n" + (Damage / 1000000000f).ToString("F1") + "b";
            }
            else if (Damage >= 1000000f)
            {
                // เช็คหลักล้านก่อน
                HUD.text = "Dps:\n" + (Damage / 1000000f).ToString("F1") + "M";
            }
            else if (Damage >= 1000f)
            {
                // ถ้าไม่ถึงล้าน แต่ถึงพัน ให้ใช้ K
                HUD.text = "Dps:\n" + (Damage / 1000f).ToString("F1") + "K";
            }

            else
            {
                HUD.text = "Dps:\n" + Damage.ToString("F0");
            }
        }
        if (CoinNeed != null)
        {
            if (_upgradeneed >= 1000000000)
            {
                // เช็คหลักล้านก่อน
                CoinNeed.text = "need: " + (_upgradeneed / 1000000000f).ToString("F1") + "B";
            }
            else if (_upgradeneed >= 1000000)
            {
                // เช็คหลักล้านก่อน
                CoinNeed.text = "need: " + (_upgradeneed / 1000000f).ToString("F1") + "M";
            }
            else if (Damage >= 1000)
            {
                // ถ้าไม่ถึงล้าน แต่ถึงพัน ให้ใช้ K
                CoinNeed.text = "need: " + (_upgradeneed / 1000f).ToString("F1") + "K";
            }

            else
            {
                CoinNeed.text = "need: " + _upgradeneed.ToString("F0");
            }
            
        }
        if(ShowLevel != null)
        {
            ShowLevel.text = "Lvl: "+Level.ToString();
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
                mesh.gameObject.SetActive(true);
                if (attackRoutine != null) StopCoroutine(attackRoutine);
                attackRoutine = StartCoroutine(Autoattack());
                Debug.Log("Companion Purchased!");
            }
            else
            {
                if (Upgradecount  < 10)
                {
                    _upgradeneed += 10;
                    Damage += bonus;
                }
                else
                {
                    bonus += 20;
                    Damage += bonus;
                    float nextPrice = _upgradeneed * 1.2f;

                    // ถ้าผลลัพธ์ใหม่เท่ากับค่าเดิม (กรณีราคาต่ำมาก) ให้บวกเพิ่มเอง 1
                    if ((long)System.Math.Round(nextPrice) <= _upgradeneed)
                    {
                        _upgradeneed += 1;
                    }
                    else
                    {
                        _upgradeneed = (long)System.Math.Round(nextPrice);
                    }
                    Upgradecount = 0;
                   
                    Debug.Log(Upgradecount);
                }
              
            }
            Level++;
            // --- จุดที่แก้ไข ---
            // คำนวณราคาใหม่โดยปัดเศษขึ้นเสมอ เพื่อป้องกันราคาค้างที่ 1
           
            // ------------------

            UpdateUI();
        }
    }

    public void AllIN()
    {
        while (StatusManager.Instance.GetCoin() >= _upgradeneed)
        {
            // 2. สั่งอัปเกรด (ในนี้มีการหักเงินจริงใน StatusManager แล้ว)
            Upgrade();

            // 3. ป้องกัน Infinite Loop กรณีค่าอัปเกรดเป็น 0 หรือติดลบ (ถ้ามี)
            if (_upgradeneed <= 0) break;
        }
        UpdateUI();
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
    public CompanionSaveData GetData()
    {
        return new CompanionSaveData
        {
            ID = this.companionID,
            IsSpawn = this.isSpawned,
            companiondamgage = this.Damage,
            CompanionUpgradeneed = this._upgradeneed,
            upgradeCount = this.Upgradecount,
            bonus = this.bonus,
            LevelCom = this.Level
        };
    }
    public void LoadData(CompanionSaveData data)
    {
        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();
        isSpawned = data.IsSpawn;
        Damage = data.companiondamgage;
        _upgradeneed = data.CompanionUpgradeneed;
        Upgradecount = data.upgradeCount;
        bonus = data.bonus;
        Level = data.LevelCom;
        mesh.gameObject.SetActive(isSpawned);
    }
}
