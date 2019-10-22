using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 4f;
    private float movement = 0f;
    public Rigidbody2D rigidBody2D;
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    public Vector3 RespawnPoint;
    public LevelManager levelManager;
    private Animator playerAnimator;
    public bool inputDisable;
    public bool PlayerDied;
    public PauseScript pause;
    public CircleCollider2D circleCollider;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        RespawnPoint = transform.position;
        //pause = FindObjectOfType<PauseScript>();
        levelManager = FindObjectOfType<LevelManager>();
        playerAnimator =  GetComponent<Animator>();
        inputDisable = false;
        PlayerDied = false;
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
            isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
            movement = Input.GetAxis("Horizontal");

            if (!inputDisable)
            {
                if (movement != 0f)
                {
                    rigidBody2D.velocity = new Vector2(movement * speed, rigidBody2D.velocity.y);
                    playerAnimator.SetInteger("isMoving", 1);
                }
                else
                {
                    playerAnimator.SetInteger("isMoving", 0);
                }
                if (Input.GetButtonDown("Jump") && (isTouchingGround))
                {
                    rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpSpeed);
                }
            }
            if (PlayerDied == true)
            {
                audioSource.Pause();
                rigidBody2D.velocity = new Vector3(0, 3f);
            }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Checkpoint")
        {
            RespawnPoint = other.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "FallDetector")
        {
            inputDisable = true;
            PlayerDied = true;
            Debug.Log("FallDetected!");
            circleCollider.enabled = false;
            StartCoroutine("WaitforAnimation");
        }
        if(other.gameObject.tag == "Enemy")
        {
            inputDisable = true;
            PlayerDied = true;
            circleCollider.enabled = false;
            StartCoroutine("WaitforAnimation");
        }
    }

    private IEnumerator WaitforAnimation()
    {
        playerAnimator.Play("PlayerDeath");
        rigidBody2D.gravityScale = 0;
        rigidBody2D.mass = 0;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitForSeconds(2.5f);
        levelManager.Respawn();
    }
}
