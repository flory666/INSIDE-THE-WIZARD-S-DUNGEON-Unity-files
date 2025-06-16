using UnityEngine;

public class presure_plate : MonoBehaviour
{
    public GameObject door;
    public string activeColorCode = "#5E5E5E";
    public string defaultColorCode = "#FFFFFF"; // culoarea inițială
    private int objectsOnPlate = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cutie") || collision.CompareTag("orc"))
        {
            objectsOnPlate++;
            if (objectsOnPlate == 1) // doar prima intrare declanșează
            {
                ActivatePlate();
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cutie") || collision.CompareTag("orc"))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                DeactivatePlate();
                objectsOnPlate = 0;
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private void ActivatePlate()
    {
        if (door != null)
        {
            SpriteRenderer sr = door.GetComponent<SpriteRenderer>();
            if (sr != null && ColorUtility.TryParseHtmlString(activeColorCode, out Color newColor))
            {
                sr.color = newColor;
            }
            Collider2D col = door.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }

    private void DeactivatePlate()
    {
        if (door != null)
        {
            SpriteRenderer sr = door.GetComponent<SpriteRenderer>();
            if (sr != null && ColorUtility.TryParseHtmlString(defaultColorCode, out Color baseColor))
            {
                sr.color = baseColor;
            }
            Collider2D col = door.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
            }
        }
    }
}
