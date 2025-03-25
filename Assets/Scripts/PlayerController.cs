using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed = 10.0f;
    private float xRange = 8;
    private Rigidbody playerRb;
    public float jumpForce;
    public float gravityModifier;
    public bool isGrounded = true;
    public bool isJumping = false;

    public bool hasPowerup = false;
    public float capsuleDuration = 5;

    private GameManager gameManager;
    private int pointValue = 1;

    private float scaleMod = .35f;
    private float powerUpScaleMod = 1.5f;

    private Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    { 
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //player movement and keep player inside boundaries

        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);

        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        //player jump

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && isJumping)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }

        else if (collision.gameObject.CompareTag("Ball"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            gameManager.UpdateP1Score(pointValue);
        }
        else if (collision.gameObject.CompareTag("Capsule"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            hasPowerup = true;
            transform.localScale = new Vector3 (scaleMod, scaleMod, scaleMod) * powerUpScaleMod;
            StartCoroutine(CapsuleCooldown());
        }
    }

    IEnumerator CapsuleCooldown()
    {
        yield return new WaitForSeconds(capsuleDuration);
        hasPowerup = false;
        transform.localScale = new Vector3 (scaleMod, scaleMod, scaleMod);
    }
}
