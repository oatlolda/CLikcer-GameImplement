using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class PlayerController : Observer
{
    private IPlayerState _idlestate, _attackstate;
    private IPlayerState _currentState;
    private PLayerStateContext _playercontext;

    [SerializeField] private Switchcam _cameraSwitcher;

    [SerializeField] private float _damage = 5;

    public bool IsCritical;
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

        Player_Idle();
    }
   
    private void Update()
    {
        if (AttackPressed())
        {
            // สั่งเปลี่ยนเป็น Attack State
            Player_Attack();

            // ส่ง Event ดาเมจ
            GameEventBus.Publish(GameEventType.Attacked);
        }
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

   
    public void Player_Idle()
    {
        _currentState = _idlestate;
        _playercontext.Transition(_idlestate);
        _cameraSwitcher.SwitchToNormal();
    }
    public void Player_Attack()
    {
        _currentState = _attackstate;
        _playercontext.Transition(_attackstate);
        _cameraSwitcher.SwitchToCombat();
    }

    public override void Notify(Subject subject)
    {
        PlayerAttackState sm = subject as PlayerAttackState;
        if (sm != null)
        {
            Debug.Log("Notify player");
        }
           
    }
    public PlayerSaveData GetData()
    {
        return new PlayerSaveData
        {
            playerDamage = _damage
        };

    }
    public void LoadData(PlayerSaveData data)
    {
        _damage = data.playerDamage;
    }
}
