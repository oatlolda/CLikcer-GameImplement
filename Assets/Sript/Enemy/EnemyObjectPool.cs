using UnityEngine;
using UnityEngine.Pool;

public class EnemyObjectPool : MonoBehaviour
{
    
    public int MaxPoolSize = 10;
    public int StackDefault = 10;

    private IObjectPool<EnemyController> _pool;
}
