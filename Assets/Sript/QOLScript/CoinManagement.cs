
using System;
using TMPro;
using UnityEngine;

public class CoinManagement : MonoBehaviour
{
  
    private long _dropcoin=60;
   [SerializeField] private long _coin;
    public long Coin
    {
        get => _coin;
        set { _coin = Math.Max(0, value);
            UpdateCoinText();
        }
    }
    public TextMeshProUGUI CoinText;

    private void Start()
    {
        UpdateCoinText();
        
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.Defeated, increaseCoin);

       
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Defeated, increaseCoin);
       
    }
    private void dropcoinincrease()
    {
        float Enemyhealth = StatusManager.Instance.GetEnemyMaxHealth();
        _dropcoin = (int)(Enemyhealth * 0.8f);
        //if (StatusManager.Instance.Enemycount == 1)
       // {
        //    _dropcoin = 10;
        //}
        Debug.Log("Dropcoin " + _dropcoin);
    }
    private void increaseCoin()
    {
      
        Coin += _dropcoin;
        dropcoinincrease();

    }
   
    private void UpdateCoinText()
    {
        if (_coin >= 1000000000f)
        {
            // เช็คหลักล้านก่อน
            CoinText.text = (_coin / 1000000000f).ToString("F1") + "B";
        }
        else if (_coin >= 1000000)
            {
            // เช็คหลักล้านก่อน
            CoinText.text =(_coin / 1000000f).ToString("F1") + "M";
        }
        else if (_coin >= 1000)
        {
            // ถ้าไม่ถึงล้าน แต่ถึงพัน ให้ใช้ K
            CoinText.text =  (_coin / 1000f).ToString("F1") + "K";
        }

        else
        {
            CoinText.text =  _coin.ToString("F0");
        }
        
       
    }
    public CoinData GetData()
    {
        return new CoinData
        {
            coin = _coin,
            DropcoinData = _dropcoin
        };

    }
    public void LoadData(CoinData data)
    {
       _dropcoin = data.DropcoinData;
        _coin = data.coin;
    }
}
