using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadAndSaveNextLevel();
        }
    }

    void LoadAndSaveNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        analizari.levelOver(currentIndex);
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextScenePath = SceneUtility.GetScenePathByBuildIndex(nextIndex);
            string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(nextScenePath);

            if (nextSceneName == "ending")
            {
                PlayerPrefs.DeleteKey("LastLevel");
                PlayerPrefs.Save();

                SceneManager.LoadScene("ending");
            }
            else
            {
                PlayerPrefs.SetString("LastLevel", nextSceneName);
                PlayerPrefs.Save();

                SceneManager.LoadScene(nextIndex);
            }
        }
        else
        {
            PlayerPrefs.DeleteKey("LastLevel");
            PlayerPrefs.Save();

            SceneManager.LoadScene("ending");
        }
    }
}
