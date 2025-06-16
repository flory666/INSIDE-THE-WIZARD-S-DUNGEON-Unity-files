using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public Controls controls;
    public Animator animator;
    private Vector2 moveInput;
    public float speed = 7f;
    public float jumpForce = 17f;
    public Rigidbody2D rb;
    private bool isInteracting = false;
    private PushableBox currentBox;
    public Transform holdPoint;
    private FixedJoint2D boxJoint;
    public ClimbableChain currentChain;
    public bool isClimbing = false;
    public float climbSpeed = 5f;
    public GameObject hand;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    public audioscipt audioManager;
    private ButtonInteractable currentButton;

    private void Awake()
    {
        hand.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("audio")?.GetComponent<audioscipt>();
        if (audioManager == null) Debug.LogWarning("AudioManager not found!");

        controls = new Controls();

        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        if (groundCheck == null) Debug.LogWarning("groundCheck not set!");
        if (animator == null) Debug.LogWarning("Animator not set!");

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.exit.performed += ctx => death();
        controls.Player.Interact.performed += ctx => Interact();
    }


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        if (!isGrounded && isInteracting)
        {
            if (boxJoint != null)
            {
                Destroy(boxJoint);
                boxJoint = null;
            }

            speed = 10f;
            isInteracting = false;
            currentBox = null;
            animator.SetBool("on_box", false);
            Debug.Log("Lăsat cutia");
        }

        CheckGrounded();
        Move();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("jump", !isGrounded);
    }

    private void Move()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(0, moveInput.y * climbSpeed);
            animator.SetFloat("chain_speed", Mathf.Abs(moveInput.y));
            return;
        }

        if (!isInteracting)
        {
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
            animator.SetFloat("speed", Mathf.Abs(moveInput.x));

            if (moveInput.x > 0)
                transform.localScale = new Vector3(0.5f, 0.5f, 0);
            else if (moveInput.x < 0)
                transform.localScale = new Vector3(-0.5f, 0.5f, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput.x * speed, 0);
            animator.SetFloat("speed", moveInput.x);
        }
    }

    private void Jump()
    {
        if (isClimbing)
        {
            isClimbing = false;
            currentChain = null;
            rb.gravityScale = 3;
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            animator.SetBool("on_chain", false);
            return;
        }

        if (isInteracting || !isGrounded) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Interact()
    {
        Debug.Log("Interact button pressed");

        if (currentButton != null)
        {
            currentButton.Activate();
            currentButton = null;
            hand.SetActive(false);
        }

        if (currentChain != null && !isClimbing)
        {
            hand.SetActive(false);
            isClimbing = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.transform.position = new Vector3(currentChain.transform.position.x, transform.position.y, transform.position.z);
            animator.SetBool("on_chain", true);
            Debug.Log("Player started climbing");
            return;
        }

        if (isClimbing)
        {
            isClimbing = false;
            currentChain = null;
            rb.gravityScale = 3;
            animator.SetBool("on_chain", false);
            Debug.Log("Player stopped climbing");
            return;
        }

        if (currentBox == null) return;

        if (!isInteracting)
        {
            animator.SetBool("on_box", true);
            hand.SetActive(false);
            speed = 40f;
            boxJoint = currentBox.gameObject.AddComponent<FixedJoint2D>();
            boxJoint.connectedBody = rb;

            Vector2 directionToBox = (currentBox.transform.position - transform.position).normalized;
            transform.position = currentBox.transform.position - (Vector3)directionToBox * 1.0f;

            boxJoint.autoConfigureConnectedAnchor = false;
            boxJoint.anchor = Vector2.zero;
            boxJoint.connectedAnchor = transform.InverseTransformPoint(holdPoint.position);

            isInteracting = true;
            Debug.Log("Apucat cutia");

        }
        else
        {
            if (boxJoint != null)
            {
                Destroy(boxJoint);
                boxJoint = null;
            }
            if (transform.localScale.x > 0)
                transform.position += new Vector3(-0.1f, 0, 0);
            else transform.position += new Vector3(0.1f, 0, 0);
            speed = 10f;
            isInteracting = false;
            currentBox = null;
            animator.SetBool("on_box", false);
            Debug.Log("Lăsat cutia");
            rb.linearVelocity = new Vector2(0f, 0f);
            animator.Play("idle");
        }
    }
    private void death()
    { SceneManager.LoadScene("main menu"); }
    private void stepsSFX()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.steps);
    }
    private void chainSFX()

    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.chain);
    }

    public void OnTriggerEnteredFromChild(Collider2D other)
    {
        PushableBox box = other.GetComponent<PushableBox>();
        if (box != null)
        {
            currentBox = box;
            Debug.Log("Entered box zone.");
            if (!isInteracting)
                hand.SetActive(true);
            return;
        }

        ClimbableChain chain = other.GetComponent<ClimbableChain>();
        if (chain != null)
        {
            if (!isClimbing)
                hand.SetActive(true);
            currentChain = chain;
            Debug.Log("Entered climbable chain zone.");
        }

        death moarte = other.GetComponent<death>();
        if (moarte != null)
        {
            Invoke("death", 2);
        }
        ButtonInteractable button = other.GetComponent<ButtonInteractable>();
        if (button != null)
        {
            currentButton = button; // definește `currentButton` în clasă
            if (!isInteracting)
                hand.SetActive(true);
            Debug.Log("Entered button zone.");
            return;
        }
    }

    public void OnTriggerExitedFromChild(Collider2D other)
    {
        if (other.TryGetComponent(out PushableBox box))
        {
            if (!isInteracting && box == currentBox)
            {
                hand.SetActive(false);
                currentBox = null;
                Debug.Log("Ieșit din zona cutiei.");
            }
        }

        if (other.TryGetComponent(out ClimbableChain chain))
        {
            if (chain == currentChain)
            {
                hand.SetActive(false);
                currentChain = null;
            }
        }
    }
    
}
