using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private float _internalDamage;
    public TextMeshProUGUI PlayerDamageHUD;
    public TextMeshProUGUI CoinNeed;
    private int upgradecount = 0;
    private float bonusUpgrade= 10f;
    public Button button;
    private int _coinupgrade= 10;

    private int Checkcoin;
    private bool Canupgrade;
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
        
        
        _internalDamage = StatusManager.Instance.GetPlayerDamage();
        
        upgradecount++;
        if (upgradecount == 10)
        {
            upgradecount = 0;
            bonusUpgrade += 10f;
        }
        _internalDamage += bonusUpgrade;
        int damageToSend = Mathf.RoundToInt(_internalDamage);

        StatusManager.Instance.SetCoin(Checkcoin - _coinupgrade);
        _coinupgrade = (int)(_coinupgrade + (_coinupgrade * 0.2f));
        StatusManager.Instance.SetPlayerDamage(damageToSend);
        UpdateUI();
        
    }

    private void UpdateCoin()
    {
        Checkcoin= StatusManager.Instance.GetCoin();
        if (Checkcoin >= _coinupgrade) 
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
        if (PlayerDamageHUD != null)
        {
            PlayerDamageHUD.text = StatusManager.Instance.GetPlayerDamage().ToString();
        }
        if (CoinNeed != null)
        {
            CoinNeed.text =_coinupgrade.ToString();
        }
    }
}
