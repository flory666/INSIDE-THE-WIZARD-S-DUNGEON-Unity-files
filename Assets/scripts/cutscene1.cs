using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class cutscene1 : MonoBehaviour
{
     public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }
    void Update()
{
    if (Input.anyKeyDown)
    {
        SceneManager.LoadScene("level1");
    }
}

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("level1"); // înlocuiește cu numele scenei tale de meniu
    }
}
