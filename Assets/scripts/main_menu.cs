using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class main_menu : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void startGame()
    {
        PlayerPrefs.SetString("LastLevel", "level1");
        PlayerPrefs.Save();
        SceneManager.LoadScene("cutscene"); 

    }
    public void continua()
    {
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            string sceneName = PlayerPrefs.GetString("LastLevel");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene("level1");
        }
    }
    public void quit()
    { Application.Quit(); }


    public void volumeMixer()
    {
        float volume = sliderMusic.value;
        mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        volume = sliderSFX.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
