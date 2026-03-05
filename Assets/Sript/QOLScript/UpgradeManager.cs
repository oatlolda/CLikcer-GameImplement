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
            if (playerdamage >= 1000000000f)
            {
                // เช็คหลักล้านก่อน
                PlayerDamageHUD.text = "Dps:\n" + (playerdamage / 1000000000f).ToString("F1") + "B";
            }

            else if (playerdamage >= 1000000)
            {
                // เช็คหลักล้านก่อน
                PlayerDamageHUD.text = "Dps:\n" + (playerdamage / 1000000f).ToString("F1") + "M";
            }
            else if (playerdamage >= 1000)
            {
                // ถ้าไม่ถึงล้าน แต่ถึงพัน ให้ใช้ K
                PlayerDamageHUD.text = "Dps:\n" + (playerdamage / 1000f).ToString("F1") + "K";
            }

            else
            {
                PlayerDamageHUD.text = "Dps:\n" + playerdamage.ToString("F0");
            }

          
        }
        if (CoinNeed != null)
        {
            if (_coinupgrade >= 1000000000f)
            {
                // เช็คหลักล้านก่อน
                CoinNeed.text = "need: " + (_coinupgrade / 1000000000f).ToString("F1") + "B";
            }
            else if (_coinupgrade >= 1000000)
            {
                // เช็คหลักล้านก่อน
                CoinNeed.text = "need: " + (_coinupgrade / 1000000f).ToString("F1") + "M";
            }
            else if (_coinupgrade >= 1000)
            {
                // ถ้าไม่ถึงล้าน แต่ถึงพัน ให้ใช้ K
                CoinNeed.text = "need: " + (_coinupgrade / 1000f).ToString("F1") + "K";
            }

            else
            {
                CoinNeed.text = "need: " + _coinupgrade.ToString("F0");
            }
           
        }
        if(ShowLEvel != null)
        {
            ShowLEvel.text = "Lvl: " + Level.ToString();
        }
        SaveData.Instance.SaveGame();
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
