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

    [SerializeField] private int StarterPrice =20;
    public Button button;//companion upgrade
    public TextMeshProUGUI HUD;
    public TextMeshProUGUI CoinNeed;
    private bool isSpawned = false;
    private MeshRenderer mesh;

    private bool Canupgrade = false;
    private int Checkcoin;
    private int _upgradeneed=10;
    private void Start()
    {
       
        mesh = GetComponent<MeshRenderer>();
       
       
        UpdateUI();
        UpdateCoin();
        if (_upgradeneed == StarterPrice)
        {
            mesh.enabled = false;
            HUD.text = "";
        }
        else if (_upgradeneed != StarterPrice) mesh.enabled = true;
    }
    private void Update()
    {
        
        UpdateCoin();
    }

    protected virtual IEnumerator Autoattack()
    {
        while (true) // ใช้ Loop แทนการเรียกฟังก์ชันซ้อนฟังก์ชัน
        {
            if (enemyController == null)
            {
                enemyController = Object.FindAnyObjectByType<EnemyController>();
            }

            if (enemyController != null)
            {
                enemyController.TakeDamage(Damage);
            }

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
        // 1. ดึงเงินล่าสุดมาเช็คก่อนกด
        Checkcoin = StatusManager.Instance.GetCoin();

        if (Checkcoin >= _upgradeneed)
        {
            // 2. หักเงินทันที (หักจากราคาปัจจุบันก่อนเพิ่มราคา)
            StatusManager.Instance.SetCoin(Checkcoin - _upgradeneed);

            if (!isSpawned)
            {
                isSpawned = true;
                mesh.enabled = true;
                StartCoroutine(Autoattack());
            }
            else
            {
                // แก้ไข: ใช้ CeilToInt เพื่อให้ดาเมจเพิ่มขึ้นอย่างน้อย 1 เสมอ
                float bonus = Damage * 0.3f;
                Damage += bonus;
                Debug.Log("Upgrade Success! New Damage: " + Damage);
            }

            // 3. ค่อยเพิ่มราคาสำหรับครั้งถัดไป
            _upgradeneed = (int)(_upgradeneed + (_upgradeneed * 0.22f));

            // 4. อัปเดต UI ทันที
            UpdateUI();
        }
    }
}
