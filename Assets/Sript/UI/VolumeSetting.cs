using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField]private AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SfxSLider;

    private void Start()
    {
        SetMusic();
        SetSfx();
    }
    public void SetMusic()
    {
        float volume =MusicSlider.value;
        if (volume <= 0.0001f)
        {
            audioMixer.SetFloat("Ost", -80f); // ลดจนเงียบสนิท
        }
        else
        {
            // สูตรแปลง Linear (0-1) เป็น Decibel (-80 ถึง 0)
            audioMixer.SetFloat("Ost", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat("OstVolume", volume);
    }
    public void SetSfx()
    {
        float volume = SfxSLider.value;
        if (volume <= 0.0001f)
        {
            audioMixer.SetFloat("Sfx", -80f);
        }
        else
        {
            audioMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }
    public VolumeData GetData()
    {
        return new VolumeData
        {
            SfxVolume = PlayerPrefs.GetFloat("SfxVolume"),
            OstVolume = PlayerPrefs.GetFloat("OstVolume")
        };
    }
    public void LoadData(VolumeData data)
    {
        MusicSlider.value = data.OstVolume;
        SfxSLider.value = data.SfxVolume;
    }
   
}
