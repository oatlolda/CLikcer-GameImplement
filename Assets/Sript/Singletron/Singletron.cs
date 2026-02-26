using UnityEngine;

public class Singletron<T> : MonoBehaviour
    
    where T : Component
{
   private static T _instance;

    public static T Instance
    {
        get
        {
            if( _instance == null)
            {
                _instance = Object.FindAnyObjectByType<T>();

                if ( _instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        if (_instance == null)
        {
            // ถ้ายังไม่มี Instance ให้ตัวเราเป็น Instance
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ถ้ามี Instance อยู่แล้ว (ตัวซ้ำ) ให้ทำลายตัวที่มาทีหลังทิ้ง
            Destroy(gameObject);
        }
    }
}
