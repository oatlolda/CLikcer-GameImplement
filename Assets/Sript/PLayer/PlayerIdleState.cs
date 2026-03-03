using TMPro;
using UnityEngine;

public class PlayerIdleState : MonoBehaviour,IPlayerState
{
    
    private PlayerController _playercontroller;
    private Animator _animator;
    
    
    private static readonly int idleStateHash = Animator.StringToHash("Idle");

    private void Start()
    {
       
    }
    public void Handle(PlayerController controller)
    {
        SoundManager.Instance.StopSound();
        if (!_playercontroller)
        {
            _playercontroller = controller;
        }
       
        if (_animator == null)
        {
            _animator = controller.GetComponentInChildren<Animator>();
        }

        // เมื่อมั่นใจว่ามี _animator แน่นอนแล้วค่อยสั่งเล่น
        if (_animator != null)
        {
            _animator.speed = 1;
            _animator.Play(idleStateHash);
        }
    }
   
}
