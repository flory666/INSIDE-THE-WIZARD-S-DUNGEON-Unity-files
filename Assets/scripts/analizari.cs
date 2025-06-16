using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Threading.Tasks;

public class analizari : MonoBehaviour
{
    public static analizari Instance;

    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            await Unity.Services.Core.UnityServices.InitializeAsync();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void moarte(string motiv)
    {
        var ev = new CustomEvent("GameOver");
        ev["motiv"] = motiv;
        AnalyticsService.Instance.RecordEvent(ev);
        AnalyticsService.Instance.Flush();
        Debug.Log("Eveniment Analytics trimis.");
    }
    public static void levelOver(int nivel)
    {var ev = new CustomEvent("level");
        ev["nivel"] = nivel-2;
        AnalyticsService.Instance.RecordEvent(ev);
        Debug.Log("Eveniment Analytics trimis.");}
}
