using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private float _internalDamage;
    public TextMeshProUGUI PlayerDamageHUD;
    private int upgradecount = 0;
    private float bonusUpgrade= 10f;

    private void Start()
    {
        _internalDamage = StatusManager.Instance.GetPlayerDamage();
        UpdateUI();
    }
    public void UpgradePlayerDamage()
    {
         _internalDamage = StatusManager.Instance.GetPlayerDamage();
        upgradecount++;
           if (upgradecount  == 10)
        {
            upgradecount = 0;
            bonusUpgrade += 10f;
        }
        _internalDamage += bonusUpgrade;
        int damageToSend = Mathf.RoundToInt(_internalDamage);


        StatusManager.Instance.SetPlayerDamage(damageToSend);
        UpdateUI();
    }
    private void UpdateUI()
    {
       if (PlayerDamageHUD != null)
        {
            PlayerDamageHUD.text = StatusManager.Instance.GetPlayerDamage().ToString();
        }
    }
}
