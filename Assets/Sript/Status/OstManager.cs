using UnityEngine;

public class OstManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _ost;
   private AudioSource _audio;
    private int random;
    void Start()
    {
        random = Random.Range(1, _ost.Length);
        _audio = GetComponent<AudioSource>();

        _audio.clip = _ost[random];
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
