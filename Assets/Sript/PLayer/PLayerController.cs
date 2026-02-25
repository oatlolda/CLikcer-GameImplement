using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerState _idlestate, _attackstate;
    private IPlayerState _currentState;
    private PLayerStateContext _playercontext;
   

    private float _damage = 5;
    public float Damage
    {
        get { return _damage; }
        set
        {
            if (value >= 5) // เช็คค่าใหม่ที่ส่งมา (value)
            {
                _damage = value;
            }
        }
    }

    private void Start()
    {
        _playercontext = new PLayerStateContext(this);

        _idlestate = gameObject.AddComponent<PlayerIdleState>();
        _attackstate = gameObject.AddComponent<PlayerAttackState>();
       
        _playercontext.Transition(_idlestate);
    }
    private void Update()
    {
        bool isInputting = (Input.touchCount > 0) || Input.GetMouseButton(0);

      
        if (isInputting)
        {

            if (_currentState != _attackstate)
            {
                Player_Attack();
                GameEventBus.Publish(GameEventType.Attacked);
            }
        }
        /*else
        {
            if (_currentState != _idlestate)
            {
                Player_Idle();
            }
        }*/
    }
   
    public void Player_Idle()
    {
        _currentState = _idlestate;
        _playercontext.Transition(_idlestate);
    }
    public void Player_Attack()
    {
        _currentState = _attackstate;
        _playercontext.Transition(_attackstate);
    }
}
