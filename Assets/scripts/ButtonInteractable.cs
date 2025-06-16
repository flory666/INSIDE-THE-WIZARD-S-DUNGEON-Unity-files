using UnityEngine;
public class ButtonInteractable : MonoBehaviour
{
    public GameObject door;
    public string colorCode = "#5E5E5E";
    private bool flipped = false;
    audioscipt audioManager;
    

    public void Activate()
    {
        if (!flipped)
        {
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
            flipped = true;
            GetComponent<Collider2D>().enabled = false;
            audioManager = GameObject.FindGameObjectWithTag("audio")?.GetComponent<audioscipt>();
            audioManager.PlaySFX(audioManager.lever);
        }
        if (door.tag == "door")
        {
            if (door != null)
            {
                SpriteRenderer sr = door.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (ColorUtility.TryParseHtmlString(colorCode, out Color newColor))
                    {
                        sr.color = newColor;
                    }
                    else
                    {
                        Debug.LogWarning("Cod culoare invalid: " + colorCode);
                    }
                }
                Collider2D col = door.GetComponent<Collider2D>();
                if (col != null)
                {
                    col.enabled = false;
                }
            }
        }
        else door.SetActive(false);
    }
}
