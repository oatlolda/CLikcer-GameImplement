using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerAttackState : Subject,IPlayerState
{
    private PlayerController _playercontroller;
    private Animator _animator;

    private EnemyController _enemyController;
    
    [SerializeField] private float attackHangTime = 0.5f; // ระยะเวลาที่จะค้างอยู่ในท่าสู้ (ปรับได้ตามความชอบ)
    private float _lastAttackTime;
    public bool IsCritical { get; private set; }
    private static readonly int attackStateHash = Animator.StringToHash("Attack");
    void Start()
    {
      
        _animator = GetComponentInChildren<Animator>();
       
    }
    private bool _isAttached = false;
    public void Handle(PlayerController controller)
    {
        if (!_playercontroller)
        {
            _playercontroller = controller;
        }

        if (_animator == null)
        {
            _animator = controller.GetComponentInChildren<Animator>();
        }

        if (!_isAttached)
        {
            DamageNumpool pool = FindAnyObjectByType<DamageNumpool>();
            if (pool != null)
            {
                Attach(pool);
                _isAttached = true;   // 🔥 กัน Attach ซ้ำ
            }
        }

        IsCritical = Random.value < 0.25f;

        if (IsCritical)
            SoundManager.Instance.Playonetime();
        else
            SoundManager.Instance.PlaySound();

        NotifyObservers();

        _lastAttackTime = Time.time;
        _animator.Play(attackStateHash);
    }

    private void OnEnable()
    {
        if (_playercontroller)
        {
            Attach(_playercontroller);
        }
        if (_enemyController)
        {
            Attach(_enemyController);
        }


    }
    private void OnDisable()
    {
        if (_playercontroller)
        {
            Detach(_playercontroller);
        }
        if (_enemyController)
        {
            Detach(_enemyController);
        }

    }
    private void DidCirtical()
    {
        SoundManager.Instance.Playonetime();
        NotifyObservers();
    }

    private void Update()
     {
        if (_playercontroller == null || _animator == null) return;

        // ถ้าเวลาปัจจุบัน ห่างจากเวลาที่คลิกครั้งสุดท้าย เกินค่าที่กำหนด (เช่น 0.5 วินาที)
        if (Time.time - _lastAttackTime > attackHangTime)
        {
            // เช็คเพิ่มอีกนิดว่าแอนิเมชั่นฟันจบหรือยัง (เพื่อความเนียน)
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.1f)
            {
                _playercontroller.Player_Idle();
            }
        }

    }
  

}
