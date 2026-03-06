using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private float _internalDamage;
    public TextMeshProUGUI PlayerDamageHUD;
    public TextMeshProUGUI CoinNeed;
    public TextMeshProUGUI ShowLEvel;
    private int upgradecount = 0;
    private float bonusUpgrade= 10f;
    public Button button;
    public Button buttonAllIn;
    private long _coinupgrade = 10;
    private int Level = 1;
    private long Checkcoin;
 
    private void Start()
    {
        _internalDamage = StatusManager.Instance.GetPlayerDamage();
        UpdateUI();
        UpdateCoin();
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.Defeated, UpdateCoin);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Defeated, UpdateCoin);
    }

   
    public void UpgradePlayerDamage()
    {


        long currentCoin = StatusManager.Instance.GetCoin();

        // ตรวจสอบอีกชั้นว่าเงินพอไหม (กันเหนียว)
        if (currentCoin < _coinupgrade) return;

        _internalDamage = StatusManager.Instance.GetPlayerDamage();
        Level++;
        upgradecount++;

        if (upgradecount == 10)
        {
            upgradecount = 0;
            bonusUpgrade += bonusUpgrade * 0.5f;
            _coinupgrade = (long)(_coinupgrade + (_coinupgrade * 0.2f));
        }

        _internalDamage += bonusUpgrade;
        long damageToSend = (long)System.Math.Round(_internalDamage);

        // หักเงินจากค่าล่าสุดที่ดึงมา
        StatusManager.Instance.SetCoin(currentCoin - _coinupgrade);

        _coinupgrade += 20;
        StatusManager.Instance.SetPlayerDamage(damageToSend);
        UpdateUI();
        
    }
    public void Allin()
    {

        while (StatusManager.Instance.GetCoin() >= _coinupgrade)
        {
            // 2. สั่งอัปเกรด (ในนี้มีการหักเงินจริงใน StatusManager แล้ว)
            UpgradePlayerDamage();

            // 3. ป้องกัน Infinite Loop กรณีค่าอัปเกรดเป็น 0 หรือติดลบ (ถ้ามี)
            if (_coinupgrade <= 0) break;
        }
        UpdateCoin();
    }

    private void UpdateCoin()
    {
        Checkcoin= StatusManager.Instance.GetCoin();
        if (Checkcoin >= _coinupgrade) 
        {
            buttonAllIn.interactable = true;
            button.interactable = true;
        }
        else
        {
            buttonAllIn.interactable = false;
            button.interactable = false;
        }
       
    }
    private void UpdateUI()
    {
        UpdateCoin();
        if (PlayerDamageHUD != null)
        {
            float playerdamage = StatusManager.Instance.GetPlayerDamage();
            UpdateText(playerdamage, PlayerDamageHUD, "Dps:\n ");


        }
        if (CoinNeed != null)
        {
            UpdateText(_coinupgrade, CoinNeed,"need: ");


        }
        if(ShowLEvel != null)
        {
            ShowLEvel.text = "Lvl: " + Level.ToString();
        }
        SaveData.Instance.SaveGame();
    }
    public void UpdateText(float amount,TextMeshProUGUI text,string chat) // เปลี่ยนรับค่าเป็น long
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" };
        int suffixIndex = 0;
        double display = amount;

        // วนลูปหารทีละ 1000 จนกว่าค่าจะน้อยกว่า 1000 หรือหมด Array
        while (display >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            display /= 1000f;
            suffixIndex++;
        }

        // ถ้าไม่มีหน่วย (หลักหน่วย-ร้อย) ไม่ต้องมีทศนิยม, ถ้ามีหน่วยให้มีทศนิยม 1 ตำแหน่ง
        string format = (suffixIndex == 0) ? "F0" : "F1";
        text.text = chat + display.ToString(format) + suffixes[suffixIndex];
    }
    public PlayerUpgradeData GetData()
    {
        return new PlayerUpgradeData
        {
            Upgradeneed = this._coinupgrade,
            Upgradecount = this.upgradecount,
            Bonus = this.bonusUpgrade,
            LevelPlayer = this.Level
        };

    }
    public void LoadData(PlayerUpgradeData data)
    {
        _coinupgrade = data.Upgradeneed;
        upgradecount = data.Upgradecount;
        bonusUpgrade = data.Bonus;
        Level = data.LevelPlayer;
    }
}
