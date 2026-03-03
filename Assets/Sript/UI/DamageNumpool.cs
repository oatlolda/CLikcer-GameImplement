using UnityEngine;
using UnityEngine.Pool;

public class DamageNumpool : Observer
{
    public int Maxpoolsize = 10;
    public int stackdefalut = 10;

    [SerializeField] private Damagenumber prefab;
    private IObjectPool<Damagenumber> _pool;
    public IObjectPool<Damagenumber> Pool
    {
        get
        {
            if (_pool == null)
            {
                // สร้าง Pool ใหม่ และบอกคำสั่งสอน 4 อย่าง (Create, Take, Return, Destroy)
                _pool = new ObjectPool<Damagenumber>(
                    CreateItem,       // 1. ถ้าของขาด ให้ทำยังไง
                    OnTakeFromPool,        // 2. ถ้าหยิบไปใช้ ให้ทำยังไง
                    OnReturnedToPool,      // 3. ถ้าเอามาคืน ให้ทำยังไง
                    OnDestroyPoolObject,   // 4. ถ้าของเกินจำกัดแล้วคืนมา ให้ทำยังไง
                    true,
                    stackdefalut,
                    Maxpoolsize);
            }
            return _pool;
        }
    }
    private Damagenumber CreateItem()
    {
       
        Damagenumber num = Instantiate(prefab);

        num.Pool = this.Pool;
        return num;
    }
    private void OnTakeFromPool(Damagenumber num)
    {
        num.gameObject.SetActive(true); // เปิดใช้งาน
    }
    private void OnReturnedToPool(Damagenumber num)
    {
        num.gameObject.SetActive(false); // ปิดการใช้งาน (แทนการ Destroy)
    }

    private void OnDestroyPoolObject(Damagenumber num)
    {
        if (num != null && num.gameObject != null)
        {
            Destroy(num.gameObject);
        }
    }

  
    public void SpawNumber()
    {
        var num = Pool.Get();
        Vector3 offset = Random.insideUnitSphere * 0.5f;
        num.transform.position = transform.position + offset;
    }
    private void SpawnCriticalChecked(PlayerAttackState sm)
    {
        var num = Pool.Get();
        Vector3 offset = Random.insideUnitSphere * 0.5f;

        num.transform.position = transform.position + offset;

        num.UpdateDamage(true);
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.Attacked, SpawNumber);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Attacked, SpawNumber);
    }
    private void Start()
    {
        PlayerAttackState sm = FindAnyObjectByType<PlayerAttackState>();
        if (sm != null) sm.Attach(this);
    }

    public override void Notify(Subject subject)
    {
        if (subject is PlayerAttackState sm)
        {
            // เมื่อ Slider บอกว่ามีการโจมตี ให้ Spawn เลขออกมา
            SpawnCriticalChecked(sm);
        }
    }
}
