using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SliderManager : Observer 
{
    private PlayerController _playercontroller;
    private EnemyController _enemyController;

    public Slider slider;
    [SerializeField] private float speed;
    public bool IsCritical= false;
    private float _sliderValue;


    private void Start()
    {
        _playercontroller = FindAnyObjectByType<PlayerController>();
        _enemyController = FindAnyObjectByType<EnemyController>();
    }
    public override void Notify(Subject subject)
    {
        if (!_playercontroller)
        {
            _playercontroller = subject as PlayerController;
        }
       
        if (IsCritical)
        {
            int KeepDamage = StatusManager.Instance.GetPlayerDamage();

            IsCritical = false;
        }
    }

    private void Update()
    {
        _sliderValue += 0.05f*Time.deltaTime*speed;
        if (slider.value == 1f)
        {
            _sliderValue = 0f;
        }
        if (_playercontroller.AttackPressed())
        {

            if (slider.value > 0.5f && _playercontroller.AttackPressed())
            {
                IsCritical = true;
            }
            _sliderValue = 0f;
        }
        slider.value = _sliderValue;
    }
   
   
}
