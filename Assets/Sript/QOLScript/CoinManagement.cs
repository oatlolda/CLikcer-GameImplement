
using TMPro;
using UnityEngine;

public class CoinManagement : MonoBehaviour
{
    [SerializeField]private int _coin;
    private int _dropcoin = 10;

    public int Coin { get { return _coin; } set { _coin = value; UpdateCoinText(); } }
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
        int Enemyhealth = StatusManager.Instance.GetEnemyHealth();
        _dropcoin = (int)(Enemyhealth  * 0.12f);
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
        if (_coin >= 1000) 
        {
            float coinK = _coin / 1000f;
            CoinText.text = coinK.ToString("F1") +" K";
        }
        else if(_coin <0) _coin = 0;
        else
        {

            CoinText.text = _coin.ToString();
        }
       
    }
}
