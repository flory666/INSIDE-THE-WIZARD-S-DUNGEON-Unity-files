using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;
public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        videoPlayer.loopPointReached += OnVideoEnd;
    }
    void Update()
{
    if (Input.anyKeyDown)
    {
        SceneManager.LoadScene("main menu");
    }
}

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("main menu"); // înlocuiește cu numele scenei tale de meniu
    }
}
