using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
public class OrcBehavior : MonoBehaviour
{
    public Animator animator;
    public float lookInterval = 5f;
    public float detectionDistance = 10f;
    public float runSpeed = 5f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private bool stun = false;
    private bool isLookingRight = true;
    private float timer;
    private bool isChasing = false;
    private Transform player;
    private Vector2 dirToPlayer;
    private Rigidbody2D rb;
    public CapsuleCollider2D collider1;
    public float bounceForce = 30f;
    private bool canJump = false;
    audioscipt audioManager;
    private float loseSightTimer = 1f;
    private const float loseSightDelay = 1f;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio")?.GetComponent<audioscipt>();
        if (audioManager == null) Debug.LogWarning("AudioManager not found!");

        rb = GetComponent<Rigidbody2D>();
        timer = lookInterval;
    }
    async void Awake()
    { await UnityServices.InitializeAsync();}
    private void FixedUpdate()
    {
        if (!stun)
        {
            if (!isChasing)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    FlipDirection();
                    timer = lookInterval;
                }
                LookForPlayer();
            }
            else Chase();
        }
    }
    public void enterStun()
    {
        stun = true;
    }

    public void resetStun()
    {
        stun = false;
    }

    private void ShrinkCollider()
    {
        collider1.offset = new Vector2(0.8f, -2.1f);
        collider1.size = new Vector2(4.4f, 2.65f);
        collider1.direction = CapsuleDirection2D.Horizontal;
        canJump = true;
    }

    private void ResetCollider()
    {
        collider1.offset = new Vector2(0.2f, -0.06f);
        collider1.size = new Vector2(3.5f, 7f);
        collider1.direction = CapsuleDirection2D.Vertical;
        canJump = false;
    }
    private void FlipDirection()
    {
        isLookingRight = !isLookingRight;
        transform.localScale = new Vector3(isLookingRight ? 0.9f : -0.9f, 0.8f, 1);
    }

    private void LookForPlayer()
    {
        Vector2 direction = isLookingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, playerLayer);
        if (hit.collider != null)
        {
            player = hit.collider.transform;
            dirToPlayer = (player.position - transform.position).normalized;
            Debug.Log("player gasit");
            isChasing = true;
            timer = lookInterval;
        }
    }

    private void Chase()
    {
        RaycastHit2D visionCheck = Physics2D.Raycast(transform.position, dirToPlayer, detectionDistance, playerLayer);

        if (visionCheck.collider != null && visionCheck.collider.CompareTag("Player"))
        {
            loseSightTimer = loseSightDelay;
        }
        else
        {
            loseSightTimer -= Time.deltaTime;
            if (loseSightTimer <= 0f)
            {
                isChasing = false;
                animator.SetFloat("chase", 0f);
                rb.linearVelocity = Vector2.zero;
                Debug.Log("Orcul a pierdut playerul din vedere");
                FlipDirection();
                return;
            }
        }
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, dirToPlayer, 2f, obstacleLayer);
        if (wallHit.collider != null)
        {
            animator.Play("orc_fall");
            isChasing = false;
            animator.SetFloat("chase", 0f);
            rb.linearVelocity = Vector2.zero;
            Debug.Log("Orcul s-a lovit de perete");

            Vector2 reverseDirection = isLookingRight ? Vector2.left : Vector2.right;
            rb.linearVelocity = new Vector2(reverseDirection.x * runSpeed / 3, rb.linearVelocity.y);

            timer = 5f;
            return;
        }
        rb.linearVelocity = new Vector2(dirToPlayer.x * runSpeed, rb.linearVelocity.y);
        animator.SetFloat("chase", 1f);
    }
    private void stepsSFX()
    {
        audioManager.PlaySFX(audioManager.steps);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.GetContact(0);

            if (stun && contact.normal.y < -0.1f && canJump == true)
            {
                Rigidbody2D rb = collision.collider.attachedRigidbody;
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                if (stun == false)
                {
                    Debug.Log("Orcul a facut contact cu player");
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    rb.linearVelocity = new Vector2(0f, 0f);
                    isChasing = false;
                    animator.SetFloat("chase", 0f);
                    isChasing = false;
                    analizari.moarte("Jucatorul a fost mancat de ciclop");
                    if (player != null)
                    {
                        Destroy(player);
                    }
                    audioManager.PlaySFX(audioManager.swallow);
                    animator.Play("orc_eat");
                    Invoke("dead", 2);
                }
            }
        }
    }
    private void dead()
    { SceneManager.LoadScene("main menu"); }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 direction = isLookingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * detectionDistance));
    }
}
