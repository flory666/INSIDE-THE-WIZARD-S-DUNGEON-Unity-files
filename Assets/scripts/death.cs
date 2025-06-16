using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;
public class death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            {
                analizari.moarte("Jucatorul a cazut in lava sau o groapa adanca");
            }
            Invoke(nameof(Distruge), 0.3f);
            Invoke(nameof(Death), 2f);
        }
    }

    private void Distruge()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
            Debug.Log("Player distrus");
        }
    }

    private void Death()
    {
        Debug.Log("Scena schimbata spre main menu");
        SceneManager.LoadScene("main menu");
    }
}
