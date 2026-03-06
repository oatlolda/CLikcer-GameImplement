//using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;


public class EnemyController : Observer
{
   
    public GameObject[] enemyModel;
    [SerializeField] private float BossHp = 500;
    public float _maxhealth = 100;
    private float _enemyhealth;
    private int _currentIndexModel = 0;
    private Animator _animator;
    private static readonly int attackStateHash = Animator.StringToHash("Attacked");

    public float EnemyHealth { get { return _enemyhealth; } set { _enemyhealth = value; } }


    // public IObjectPool<EnemyController> Pool { get;set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
       
        ChangeMesh();


    }

    private void OnEnable()
    {

        _enemyhealth = _maxhealth;
        GameEventBus.Subscribe(GameEventType.BossState, BossState);
        GameEventBus.Subscribe(GameEventType.Attacked, GotDamage);
        GameEventBus.Subscribe(GameEventType.Defeated, Death);

    }
    private void OnDisable()
    {

        GameEventBus.Unsubscribe(GameEventType.Attacked, GotDamage);
        GameEventBus.Unsubscribe(GameEventType.BossState, BossState);
        GameEventBus.Unsubscribe(GameEventType.Defeated, Death);
    }


    public void ChangeMesh()
    {

        enemyModel[_currentIndexModel].SetActive(false);
        _currentIndexModel = (_currentIndexModel + 1) % enemyModel.Length;
        enemyModel[_currentIndexModel].SetActive(true);
    }


    private void GotDamage()
    {

        TakeDamage(StatusManager.Instance.GetPlayerDamage());
    }
    private void Death()
    {
        /* _currentIndexModel++;
         if (_currentIndexModel >= enemyModel.Length)
         {
             _currentIndexModel = 0;
         }
        */
        // àÍÒàÃ×èÍ§ÃÕà«çµàÅ×Í´ÍÍ¡¨Ò¡µÃ§¹ÕéãËéËÁ´! à¾ÃÒÐàÃÒÂéÒÂä»·Óã¹ TakeDamage áÅéÇ
        ChangeMesh();
        Debug.Log($"Monster Mesh Changed to: {_currentIndexModel}");
    }
    public void TakeDamage(float amount)
    {
        if (_enemyhealth <= 0) return;

        _enemyhealth = Mathf.Max(0, _enemyhealth - amount);
        GameEventBus.Publish(GameEventType.EnemyDamaged);
        _animator.Play(attackStateHash);
        if (_enemyhealth <= 0)
        {

            _enemyhealth = 0; // ÅçÍ¤àÅ×Í´äÇé·Õè 0 ·Ñ¹·Õ

            if (StatusManager.Instance.Enemycount == 0) // ºÍÊµÒÂ
            {
                BossHp += BossHp * 0.2f;
                GameEventBus.Publish(GameEventType.BossDefeated);
                SaveData.Instance.SaveGame();
                StatusManager.Instance.Enemycount = 1;

                _maxhealth = BossHp * 0.1f;          // ÃÕà«çµ¡ÅÑºä»ÁÍ¹»¡µÔ
                _enemyhealth = _maxhealth;

                GameEventBus.Publish(GameEventType.EnemyDamaged);
                GameEventBus.Publish(GameEventType.Defeated); // ãËé UI ÍÑ»à´µÃÍºãËÁè
            }
            else // ¡Ã³ÕÁÍ¹ÊàµÍÃì·ÑèÇä»µÒÂ
            {
                GameEventBus.Publish(GameEventType.Defeated);

                // 1. ÃÕà«çµàÅ×Í´ÁÍ¹ÊàµÍÃìµÑÇãËÁè
                _maxhealth += (int)(_maxhealth * 0.1f);
                _enemyhealth = _maxhealth;

             
                GameEventBus.Publish(GameEventType.EnemyDamaged);
            }

        }
    }

    public override void Notify(Subject subject)
    {
        PlayerAttackState sm = subject as PlayerAttackState;
        if (sm != null)
        {
            TakeDamage(StatusManager.Instance.GetCriticalDamage());
            GameEventBus.Publish(GameEventType.EnemyDamaged);


        }
    }

    private void BossState()
    {
        BossHp = Mathf.Max(BossHp, 500); // ¡Ñ¹ºÑ¤àÅ×Í´¹éÍÂ
        _maxhealth = BossHp;
        _enemyhealth = _maxhealth;

        Debug.Log($"Boss Reset: {_enemyhealth} / {_maxhealth}");

        // µéÍ§ÁÑè¹ã¨ÇèÒ Publish ËÅÑ§¨Ò¡à«çµ¤èÒ¢éÒ§º¹àÊÃç¨áÅéÇà·èÒ¹Ñé¹!
        GameEventBus.Publish(GameEventType.EnemyDamaged);
    }
    public EnemyData GetData()
    {
        return new EnemyData
        {
            BossHpData = BossHp,
            enemyMaxHealth = _maxhealth,
        };

    }
    public void LoadData(EnemyData data)
    {
        BossHp = data.BossHpData;
        _maxhealth = data.enemyMaxHealth;
    }

}
