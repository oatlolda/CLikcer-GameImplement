using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Companionscript : MonoBehaviour
{
    [SerializeField] private float DelayAttck;
    [SerializeField]private int _damage =2;
    public int Damage { get { return _damage; } set { _damage = value; } }
    private EnemyController enemyController;

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
        if (_upgradeneed == 10) mesh.enabled = false;
        else if (_upgradeneed != 10 ) mesh.enabled = true;
       
        UpdateUI();
        UpdateCoin();
    }
    private void Update()
    {
        
        UpdateCoin();
    }

    private IEnumerator Autoattack()
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
            HUD.text = "DMG "+Damage.ToString();
        }
        if (CoinNeed != null)
        {
            CoinNeed.text = _upgradeneed.ToString();
        }
    }
    public void Upgrade()
    {
        UpdateCoin();
        if (Checkcoin >= _upgradeneed) // ป้องกันการกดซ้ำถ้าเงินไม่พอจริงๆ
        {
            StatusManager.Instance.SetCoin(Checkcoin - _upgradeneed);
            if (!isSpawned)
            {
                isSpawned = true;
                mesh.enabled = true;
                StartCoroutine(Autoattack());
            }
            else
            {
                Damage = (int)(Damage + (Damage * 0.2f));
            }
          
            
            _upgradeneed = (int)(_upgradeneed + (_upgradeneed * 0.3f));
            
            UpdateUI();
        }
            
    }
}
