using UnityEngine;

public class OstManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _ost;
   private AudioSource _audio;
    private int random;
    [SerializeField] private int selectsong=0;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        if (selectsong > _ost.Length)
        {
            random = Random.Range(1, _ost.Length);
            

            _audio.clip = _ost[random];
          
        }
        else
        {
            _audio.clip = _ost[selectsong];
            
        }
        _audio.Play();

    }

   
    void Update()
    {
        if (!_audio.isPlaying)
        {
            NextSong();
        }

    }
    private void PlayCurrent()
    {
        _audio.clip = _ost[random];
        _audio.Play();
    }
    private void NextSong()
    {
        random++;

        if (random >= _ost.Length)
            random = 0; // วนกลับเพลงแรก

        PlayCurrent();
    }
}
