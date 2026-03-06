using System;
using TMPro;
using UnityEngine;

public class ChagneScene : MonoBehaviour
{
    public GameObject[] Map;
    [SerializeField]private int _currentIndexModel;
    public int _currentscene = 1;
    public TextMeshProUGUI SceneText;
    private AudioSource _audiosource;
    public Material[] newSkybox;
    private void Start()
    {
        SceneText.text = _currentscene.ToString();
        _audiosource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.BossDefeated, ChangeMesh);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.BossDefeated, ChangeMesh);
    }

    public void ChangeMesh()
    {
        _currentscene++;
        SceneText.text = _currentscene.ToString();
        if (_currentscene % 5 == 0)
        {
            _audiosource.Play();
            Map[_currentIndexModel].SetActive(false);
           
            _currentIndexModel = (_currentIndexModel + 1) % Map.Length;
            
            Map[_currentIndexModel].SetActive(true);
            RenderSettings.skybox = newSkybox[_currentIndexModel];
            DynamicGI.UpdateEnvironment();
        }
    }
    public SceneData GetData()
    {
        return new SceneData
        {
            CurrentScene = _currentscene,
            CurrenIndexModel = _currentIndexModel,
        };

    }
    public void LoadData(SceneData data)
    {
        _currentscene= data.CurrentScene;
        _currentIndexModel= data.CurrenIndexModel;

        SceneText.text = _currentscene.ToString();

        for (int i = 0; i < Map.Length; i++)
        {
            Map[i].SetActive(i == _currentIndexModel);
        }
    }
}
