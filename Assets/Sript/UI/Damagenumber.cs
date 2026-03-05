using TMPro;

using UnityEngine;
using UnityEngine.Pool;

public class Damagenumber : MonoBehaviour
{
    private TextMeshPro _damageNum;
    public IObjectPool<Damagenumber> Pool { get; set; }
   
    private Animator _animator;
    private static readonly int fadeStateHash = Animator.StringToHash("Fade");
    private bool _returned = false;
    private void OnEnable()
    {
        if (_damageNum == null) _damageNum = GetComponentInChildren<TextMeshPro>();
        _damageNum.color = Color.red;
        _damageNum.text = StatusManager.Instance.GetPlayerDamage().ToString("N0");
        _damageNum.fontSize = 5;
        _returned = false;
    }
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
       

    }
    private void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!_returned &&
            stateInfo.shortNameHash == fadeStateHash &&
            stateInfo.normalizedTime >= 0.9f)
        {
            _returned = true;
            ReturnPool();
        }
    }
    public void ReturnPool()
    {
        Pool.Release(this);
    }
    public void UpdateDamage(bool isCritical)
    {
        if (isCritical)
        {
            _damageNum.color = Color.white; 
            _damageNum.text = StatusManager.Instance.GetCriticalDamage().ToString("N0");
            _damageNum.fontSize = 6; 
        }
        else
        {
            _damageNum.color = Color.red;
            _damageNum.text = StatusManager.Instance.GetPlayerDamage().ToString("N0");
            _damageNum.fontSize = 4;
        }
    }
    

}
