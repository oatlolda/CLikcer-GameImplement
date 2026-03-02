using System.Collections;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;


public class SliderManager : Subject 
{
    private PlayerController _playercontroller;
    private EnemyController _enemyController;

    [SerializeField] private float slidevaluseneed = 0.7f;

    public Image Fill;
    public Slider slider;
    [SerializeField] private float speed;
    private bool _iscritical;
    private float _sliderValue;

    private Color color;
    private void Awake()
    {
        _playercontroller = FindAnyObjectByType<PlayerController>();
        _enemyController = FindAnyObjectByType<EnemyController>();
       
        color = Fill.color;
       
    }
  

    private void Update()
    {

        _sliderValue += speed * Time.deltaTime;
        if (slider.value >= 1f)
        {
            _sliderValue = 0f;
        }


        if (_sliderValue < slidevaluseneed)
        {
            //
            
            Fill.color = Color.Lerp(color, Color.red, _sliderValue);
        }
        else
        {
            Fill.color = Color.red;
        }
        if (_playercontroller.AttackPressed())
        {

            if (slider.value > slidevaluseneed && _playercontroller.AttackPressed())
            {
                DidCirtical();
                
            }
            Fill.color = color;
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
        _iscritical = true;
        NotifyObservers();
    }
   
}
