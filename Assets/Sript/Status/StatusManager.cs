using UnityEngine;

public class StatusManager : Singletron<StatusManager>
{
    private PlayerController _playerController;
    private EnemyController _enemyController;
    private CoinManagement _coinManagement;
    public int Enemycount = 1;
    private void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
    }

   
    public  void SetPlayerDamage(int damage)
    {
        if (_playerController == null)
        {
            _playerController = FindAnyObjectByType<PlayerController>();
        }
        if (_playerController != null)
        {
            _playerController.Damage = damage;
            
        }
    }
    public int GetPlayerDamage()
    {
        if (_playerController == null)
        {
            _playerController = FindAnyObjectByType<PlayerController>();
        }

        // เช็คเผื่อกรณีหาไม่เจอจริงๆ ในฉาก
        if (_playerController != null)
        {
            return _playerController.Damage;
        }
        Debug.LogError("StatusManager: ไม่พบ PlayerController ในฉาก!");
        return 0;
    }
    public  void SetEnemyHealyh(int Health)
    {
        if (_enemyController == null)
        {
            _enemyController = FindAnyObjectByType<EnemyController>();
        }
        if (_enemyController != null)
        {
            _enemyController.EnemyHealth = Health;

        }
    }
    public int GetEnemyHealth()
    {
        if (_enemyController == null)
        {
            _enemyController = Object.FindAnyObjectByType<EnemyController>();
        }

        // เช็คเผื่อกรณีหาไม่เจอจริงๆ ในฉาก
        if (_enemyController != null)
        {
            return _enemyController.EnemyHealth;
        }
        Debug.LogError("StatusManager: ไม่พบ EnemyController ในฉาก!");
        return 0;
    }
    public int GetCriticalDamage()
    {
       
        
        int Critical = GetPlayerDamage();
        Critical += (int)(Critical*1.5f);
        Debug.Log("Critical "+Critical);
        return Critical;
    }

    public void Updatecount()
    {
        Enemycount++;
        if (Enemycount > 8)
        {
        Enemycount = 1;            
        }
       ;
    }
    public int GetCoin()
    {

        if (_coinManagement == null)
        {
            _coinManagement = Object.FindAnyObjectByType<CoinManagement>();
        }
        if (_coinManagement != null)
        {
            // หมายเหตุ: คุณต้องไปเพิ่ม public int Coin { get; private set; } 
            // ไว้ในคลาส CoinManagement ด้วย ถึงจะเรียกใช้แบบนี้ได้
            return _coinManagement.Coin;
        }
        return 0;
        
    }
    public void SetCoin(int amount)
    {
        if (_coinManagement == null)
        {
            _coinManagement = FindAnyObjectByType<CoinManagement>();
        }
        if (_coinManagement != null)
        {
            _coinManagement.Coin = amount;

        }
    }
}
