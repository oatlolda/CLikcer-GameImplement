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
        Destroy(num.gameObject); // ลบทิ้งจริงๆ ถ้า Pool เต็มแล้ว
    }

  
    public void SpawNumber()
    {
        var num = Pool.Get();
        num.transform.position = Random.insideUnitSphere * 2.5f;
    }
    private void SpawnCriticalChecked(SliderManager sm)
    {
        var num = Pool.Get();
        num.transform.position = Random.insideUnitSphere * 2.5f;

        // เช็คจาก Slider จังหวะนั้นเลยว่า > 0.7 หรือไม่
        bool isCrit = sm.slider.value > 0.7f;
        num.UpdateDamage(isCrit);
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
        SliderManager sm = FindAnyObjectByType<SliderManager>();
        if (sm != null) sm.Attach(this);
    }

    public override void Notify(Subject subject)
    {
        if (subject is SliderManager sm)
        {
            // เมื่อ Slider บอกว่ามีการโจมตี ให้ Spawn เลขออกมา
            SpawnCriticalChecked(sm);
        }
    }
}
