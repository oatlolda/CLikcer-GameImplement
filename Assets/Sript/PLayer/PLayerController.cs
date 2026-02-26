using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerController : Subject
{
    private IPlayerState _idlestate, _attackstate;
    private IPlayerState _currentState;
    private PLayerStateContext _playercontext;

    private SliderManager _sliderManager;
    [SerializeField] private int _damage = 5;

    public bool IsCritical;
    public int Damage
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
    private void Awake()
    {
        _sliderManager = FindAnyObjectByType<SliderManager>();
    }
    private void Start()
    {
        _playercontext = new PLayerStateContext(this);

        _idlestate = gameObject.AddComponent<PlayerIdleState>();
        _attackstate = gameObject.AddComponent<PlayerAttackState>();

        Player_Idle();
    }
    private void OnEnable()
    {
        if (_sliderManager)
        {
            Attach(_sliderManager);
        }
    }
    private void OnDisable()
    {
        if (_sliderManager)
        {
            Detach(_sliderManager);
        }
    }
    private void Update()
    {



        if (AttackPressed())
        {
            IsCritical = _sliderManager.IsCritical;
            if(IsCritical)
            {
                BlachFlash();
            }
            Player_Attack();
            GameEventBus.Publish(GameEventType.Attacked);
        }
        /*else
        {
            if (_currentState != _idlestate)
            {
                Player_Idle();
            }
        }*/
    }
    public bool AttackPressed()
    {
        // 1. เช็คก่อนว่าตอนที่กด มีการกดทับ UI (ปุ่ม) หรือเปล่า?
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            // ถ้ากดโดนปุ่ม ให้ return false ทันที (ไม่โจมตี)
            return false;
        }

        // สำหรับ Mobile (Touch) ต้องเช็คเจาะจงลงไปที่ fingerId ด้วย
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return false;
        }

        // 2. ถ้าไม่ได้กดโดน UI ค่อยมาเช็คการคลิกปกติ
        return Input.GetMouseButtonDown(0) ||
               (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    public void BlachFlash()
    {
        Debug.Log("BlackFlash");
        NotifyObservers();
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
