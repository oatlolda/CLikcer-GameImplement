using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Damagenumber : MonoBehaviour
{
    private TextMeshPro _damageNum;
    public IObjectPool<Damagenumber> Pool {  get; set; }
   
    private Animator _animator;
    private static readonly int fadeStateHash = Animator.StringToHash("Fade");

    private void OnEnable()
    {
        if (_damageNum == null) _damageNum = GetComponentInChildren<TextMeshPro>();
        UpdateDamage();
    }
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
       
    }
    private void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.shortNameHash == fadeStateHash && stateInfo.normalizedTime >= 0.9f)
        {
            ReturnPool();
        }
    }
    private void ReturnPool()
    {
       
        Pool.Release(this);
    }
    private void UpdateDamage()
    {
        _damageNum.text = StatusManager.Instance.GetPlayerDamage().ToString();
    }
   
   
}
