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
        //_damageNum.text = StatusManager.Instance.GetPlayerDamage().ToString("N0");
       // float damage = StatusManager.Instance.GetPlayerDamage();
        UpdateText(StatusManager.Instance.GetPlayerDamage(), _damageNum, "");
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
    public void UpdateText(float amount, TextMeshPro text, string chat) // เปลี่ยนรับค่าเป็น long
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" };
        int suffixIndex = 0;
        double display = amount;

        // วนลูปหารทีละ 1000 จนกว่าค่าจะน้อยกว่า 1000 หรือหมด Array
        while (display >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            display /= 1000f;
            suffixIndex++;
        }

        // ถ้าไม่มีหน่วย (หลักหน่วย-ร้อย) ไม่ต้องมีทศนิยม, ถ้ามีหน่วยให้มีทศนิยม 1 ตำแหน่ง
        string format = (suffixIndex == 0) ? "F0" : "F1";
        text.text = chat + display.ToString(format) + suffixes[suffixIndex];
    }
    public void UpdateDamage(bool isCritical)
    {
        if (isCritical)
        {
            _damageNum.color = Color.white; 
            //_damageNum.text = StatusManager.Instance.GetCriticalDamage().ToString("N0");
            UpdateText(StatusManager.Instance.GetCriticalDamage(), _damageNum, "");
            _damageNum.fontSize = 6; 
        }
        else
        {
            _damageNum.color = Color.red;
            //_damageNum.text = StatusManager.Instance.GetPlayerDamage().ToString("N0");
            UpdateText(StatusManager.Instance.GetPlayerDamage(), _damageNum, "");
            _damageNum.fontSize = 4;
        }
    }
    

}
