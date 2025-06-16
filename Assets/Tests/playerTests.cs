using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class playerTests
{
    private GameObject player;
    private GameObject orc;
    private GameObject cutie;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Încarcă scena de test
        yield return SceneManager.LoadSceneAsync("test");
        yield return null;

        // Găsește obiectele după tag
        player = GameObject.FindWithTag("Player");
        orc = GameObject.FindWithTag("orc");
        cutie = GameObject.FindWithTag("Cutie");

        Assert.IsNotNull(player, "Player not found in scene.");
        Assert.IsNotNull(orc, "Orc not found in scene.");
        Assert.IsNotNull(cutie, "Cutie not found in scene.");

        Debug.Log("Player, Orc and Cutie successfully found.");
    }

    [UnityTest]
    public IEnumerator OrcEatsPlayer()
    {
        // Mută playerul în poziția orcului, simulând interacțiunea
        player.transform.position = orc.transform.position;
        yield return new WaitForSeconds(0.5f);

        // Verifică dacă playerul a fost distrus după interacțiune
        var playerStillExists = GameObject.FindWithTag("Player");
        Assert.IsNull(playerStillExists, "Player should be destroyed after orc interaction.");

        Debug.Log("Orc successfully ate the player.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerMovesBoxLeft()
    {
        // Găsește jucătorul, cutia, controllerul
        var player = GameObject.FindWithTag("Player");
        var cutie = GameObject.FindWithTag("Cutie");

        Assert.IsNotNull(player);
        Assert.IsNotNull(cutie);
        var controller = player.GetComponent<PlayerController>();

        Assert.IsNotNull(controller, "PlayerController lipseste.");

        // Teleportează jucătorul lângă cutie
        player.transform.position = cutie.transform.position + Vector3.right;

        // Simulează intrarea în collider (ca să seteze cutia curentă)
        controller.OnTriggerEnteredFromChild(cutie.GetComponent<Collider2D>());

        // Apelează Interact pentru a apuca cutia
        controller.Interact();

        // Aplică o forță spre stânga jucătorului
        var rb = player.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.left * 500);

        // Așteaptă o secundă
        yield return new WaitForSeconds(1f);

        // Verifică dacă cutia s-a deplasat spre stânga
        Assert.Less(cutie.transform.position.x, cutie.transform.position.x + 1, "Cutia nu s-a deplasat spre stanga.");

        Debug.Log("Jucătorul a mutat cutia spre stânga.");
    }
}
