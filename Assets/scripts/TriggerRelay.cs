using UnityEngine;

public class TriggerRelay : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null)
        {
            player.OnTriggerEnteredFromChild(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player != null)
        {
            player.OnTriggerExitedFromChild(other);
        }
    }
}
