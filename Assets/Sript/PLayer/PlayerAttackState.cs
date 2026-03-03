using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerAttackState : Subject,IPlayerState
{
    private PlayerController _playercontroller;
    private Animator _animator;
    [SerializeField] private float step = 0.1f;
    private EnemyController _enemyController;
    
    [SerializeField] private float attackHangTime = 0.5f; // ระยะเวลาที่จะค้างอยู่ในท่าสู้ (ปรับได้ตามความชอบ)
    private float _lastAttackTime;

    private static readonly int attackStateHash = Animator.StringToHash("Attack");
    void Start()
    {
      
        _animator = GetComponentInChildren<Animator>();
       
    }
    public void Handle(PlayerController controller)
    {
        int random = Random.Range(1, 5);
        Debug.Log("random = "+random);
        SoundManager.Instance.PlaySound();
        if (random == 1)
        {
            DidCirtical();
        }
        if (!_playercontroller)
        {
            _playercontroller = controller; 
        }
        if (_animator == null)
        {
            _animator = controller.GetComponentInChildren<Animator>();
        }
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
