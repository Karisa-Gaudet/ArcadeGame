using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player2Controller : MonoBehaviour
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
    public bool hasBanana = false;
    public bool hasPear = false;
    public float waterDuration = 5;
    public float bananaDuration = 3;
    public float pearDuration = 4;

    private GameManager gameManager;
    private GameObject player1;
    private int pointValue = 1;

    public AudioClip jumpSound;
    public AudioClip itemSound;
    public AudioClip bananaSound;
    public AudioClip punchSound;
    private AudioSource playerAudio;

    public KeyCode jumpKey;

    private float scaleMod = .35f;
    private float powerUpScaleMod = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player1 = GameObject.Find("Player 1");
        Physics.gravity = new Vector3(0, gravityModifier, 0);
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //player movement and keep player inside boundaries

        horizontalInput = Input.GetAxis("Horizontal2");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);

        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        //to prevent glitching and falling through the ground
        if (transform.position.y < -1)
        {
            transform.position = new Vector3(transform.position.x, 4, transform.position.z);
        }

        //player jump

        if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && isGrounded && !isJumping)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = true;
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && !isGrounded && isJumping)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = false;
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if ((Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) && Mathf.Abs(player1.transform.position.x - transform.position.x) < 2)
        {
            Rigidbody p2Rigidbody = player1.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (player1.transform.position - transform.position);
            p2Rigidbody.AddForce(awayFromPlayer * 10, ForceMode.Impulse);
            playerAudio.PlayOneShot(punchSound, 1.0f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }

        else if (collision.gameObject.CompareTag("Player2"))
        {
            isGrounded = false;
            isJumping = true;
        }

        if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            gameManager.UpdateP2Score(pointValue);
            playerAudio.PlayOneShot(itemSound, 1.0f);
        }
        else if (collision.gameObject.CompareTag("Peach"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            gameManager.UpdateP2Score(pointValue +1);
            playerAudio.PlayOneShot(itemSound, 1.0f);
        }
        else if (collision.gameObject.CompareTag("Watermelon"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            hasPowerup = true;
            transform.localScale = new Vector3(scaleMod, scaleMod, scaleMod) * powerUpScaleMod;
            playerAudio.PlayOneShot(itemSound, 1.0f);
            StartCoroutine(WatermelonCooldown());
        }
        else if (collision.gameObject.CompareTag("Banana"))
        {
            playerAudio.PlayOneShot(bananaSound, 1.0f);
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            hasBanana = true;
            transform.localScale = new Vector3(-scaleMod, -scaleMod, -scaleMod);
            speed = 0;
            isJumping = false;
            StartCoroutine(BananaCooldown());
        }
        else if (collision.gameObject.CompareTag("Pear"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            hasPear = true;
            playerAudio.PlayOneShot(itemSound, 1.0f);
            speed = 15;
            StartCoroutine(PearCooldown());
        }

    }
    IEnumerator WatermelonCooldown()
    {
        yield return new WaitForSeconds(waterDuration);
        hasPowerup = false;
        transform.localScale = new Vector3(scaleMod, scaleMod, scaleMod);
    }
    IEnumerator BananaCooldown()
    {
        yield return new WaitForSeconds(bananaDuration);
        hasBanana = false;
        transform.localScale = new Vector3(scaleMod, scaleMod, scaleMod);
        speed = 10;
    }

    IEnumerator PearCooldown()
    {
        yield return new WaitForSeconds(pearDuration);
        hasPear = false;
        speed = 10;
    }
}


