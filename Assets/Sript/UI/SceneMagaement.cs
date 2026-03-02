using UnityEngine;

public class SceneMagaement : MonoBehaviour
{
    [SerializeField] private Texture[] _texture;
    private int sceneindex=0;

   private Renderer _material;


    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.BossDefeated, changeBackGround);
    }
    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.BossDefeated, changeBackGround);
    }
    private void changeBackGround()
    {
        if (_material != null)
        {
            _material.material.SetTexture("BackGround", _texture[sceneindex]);
        }
        else if (_material == null)
        {
            _material = GetComponentInChildren<Renderer>();
        }
        if (sceneindex != _texture.Length)
        {
            sceneindex++;
        }
        else
        {
            sceneindex = 0;
        }
       
      
    }
}
