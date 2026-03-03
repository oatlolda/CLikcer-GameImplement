using UnityEngine;

public class SoundManager : Singletron<SoundManager>
{
    private AudioSource _audiosource;
    [SerializeField] private AudioClip _audioclip;
    private void Start()
    {
        _audiosource = GetComponent<AudioSource>();
    }
   
    public void PlaySound()
    {
      
        if (!_audiosource.isPlaying)
        {
            _audiosource.Play();
        }
        else
        {

        }
    }
    public void StopSound()
    {
        _audiosource.Stop();
    }
    public void Playonetime()
    {
        _audiosource.PlayOneShot(_audioclip);
    }
}
