using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SliderManager : Subject 
{
    private PlayerController _playercontroller;
    private EnemyController _enemyController;

    public Image Fill;
    public Slider slider;
    [SerializeField] private float speed;
 
    private float _sliderValue;


    private void Awake()
    {
        _playercontroller = FindAnyObjectByType<PlayerController>();
        _enemyController = FindAnyObjectByType<EnemyController>();
       
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
                DidCirtical();
                
            }
            _sliderValue = 0f;
        }
        slider.value = _sliderValue;
    }
    private void OnEnable()
    {
        if (_playercontroller)
        {
            Attach(_playercontroller);
        }
        if (_enemyController)
        {
            Attach(_enemyController);
        }
       
    }
    private void OnDisable()
    {
        if (_playercontroller)
        {
            Detach(_playercontroller);
        }
        if (_enemyController)
        {
            Detach(_enemyController);
        }
       
    }
    private void DidCirtical()
    {
       
        NotifyObservers();
    }
   
}
