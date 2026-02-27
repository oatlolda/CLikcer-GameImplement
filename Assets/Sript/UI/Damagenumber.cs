using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Damagenumber : MonoBehaviour
{
    private TextMeshPro _damageNum;
    public IObjectPool<Damagenumber> Pool { get; set; }
    private DamageNumpool _damagepool;
    private Animator _animator;
    private static readonly int fadeStateHash = Animator.StringToHash("Fade");

    private void OnEnable()
    {
        if (_damageNum == null) _damageNum = GetComponentInChildren<TextMeshPro>();
        _damageNum.color = Color.red;
        _damageNum.text = StatusManager.Instance.GetPlayerDamage().ToString();
        _damageNum.fontSize = 5;
    }
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _damagepool= FindAnyObjectByType<DamageNumpool>();
        if (_damagepool != null) Debug.Log("Damagepool");

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
    public void UpdateDamage(bool isCritical)
    {
        if (isCritical)
        {
            _damageNum.color = Color.white; // คริติคอลใช้สีแดงจะเด่นกว่า
            _damageNum.text = StatusManager.Instance.GetCriticalDamage().ToString();
            _damageNum.fontSize = 8; // ตัวใหญ่ขึ้น
        }
        else
        {
            _damageNum.color = Color.red;
            _damageNum.text = StatusManager.Instance.GetPlayerDamage().ToString();
            _damageNum.fontSize = 5;
        }
    }

}
