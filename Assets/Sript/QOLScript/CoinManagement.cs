
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
        _dropcoin = (int)(Enemyhealth * 0.5f);
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

        UpdateText(_coin, CoinText, "");
    }
    public void UpdateText(float amount, TextMeshProUGUI text, string chat) // เปลี่ยนรับค่าเป็น long
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
