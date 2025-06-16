using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class end_scene : MonoBehaviour
{
    public GameObject wizard;
    public GameObject princess;
    private Animator princessAnimator;
    public GameObject cavaler;
    public GameObject bubble1;
    public GameObject bubble2;
    public GameObject bubble3;
    public GameObject bubble4;
    public GameObject bubble5;
    public GameObject end;

    // Start is called before the first frame update
    void Start()
    {
        princessAnimator = princess.GetComponent<Animator>();
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        yield return StartCoroutine(WaitForInput());
        bubble1.SetActive(true);
        yield return StartCoroutine(WaitForInput());

        bubble1.SetActive(false);
        princessAnimator.SetBool("next", true);
        bubble2.SetActive(true);
        yield return StartCoroutine(WaitForInput());

        bubble2.SetActive(false);
        bubble3.SetActive(true);
        yield return StartCoroutine(WaitForInput());

        bubble3.SetActive(false);
        bubble4.SetActive(true);
        yield return StartCoroutine(WaitForInput());

        cavaler.transform.localScale = new Vector3(-0.4032145f, 0.4018961f, 1f);
        bubble4.SetActive(false);
        bubble5.SetActive(true);
        wizard.SetActive(true);
        yield return StartCoroutine(WaitForInput());

        bubble5.SetActive(false);
        princess.SetActive(false);
        cavaler.SetActive(false);
        end.SetActive(true);
        yield return StartCoroutine(WaitForInput());
        SceneManager.LoadScene("intro");
    }

    IEnumerator WaitForInput()
    {
        // First wait until the key is released (if it's already being held)
        yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space));

        // Then wait until the key is pressed again
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
    }
}
