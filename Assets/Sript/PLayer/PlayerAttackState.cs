using UnityEngine;

public class PlayerAttackState : MonoBehaviour,IPlayerState
{
    private PlayerController _playercontroller;
    private Animator _animator;


    private static readonly int attackStateHash = Animator.StringToHash("Attack");
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Handle(PlayerController controller)
    {
       if(!_playercontroller)
        {
            _playercontroller = controller; 
        }
        if (_animator == null)
        {
            _animator = controller.GetComponent<Animator>();
        }
        if (_animator != null)
        {
            _animator.Play(attackStateHash);
        }
       
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.Attacked, PlayAnim);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Attacked, PlayAnim);
    }
    private void Update()
    {
        // เช็คว่าถ้าเล่นจบแล้ว ให้สั่ง Controller เปลี่ยน State
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.shortNameHash == attackStateHash && stateInfo.normalizedTime >= 0.9f)
        {
            // อ้างอิงกลับไปที่ Controller เพื่อสั่งเปลี่ยนเป็น Idle
            GetComponent<PlayerController>().Player_Idle();
        }
    }
    private void PlayAnim()
    {
        Debug.Log("PLayanim work");
    }
    

}
