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
    public float waterDuration = 5;
    public float bananaDuration = 3;

    private GameManager gameManager;
    private GameObject player1;
    private int pointValue = 1;

    public KeyCode jumpKey;

    private float scaleMod = .35f;
    private float powerUpScaleMod = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player1 = GameObject.Find("Player 1");
        Physics.gravity *= gravityModifier;
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

        //player jump

        if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && isGrounded && !isJumping)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = true;
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && !isGrounded && isJumping)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = false;
        }

        if ((Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) && Mathf.Abs(player1.transform.position.x - transform.position.x) < 2)
        {
            Rigidbody p2Rigidbody = player1.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (player1.transform.position - transform.position);
            p2Rigidbody.AddForce(awayFromPlayer * 10, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            gameManager.UpdateP2Score(pointValue);
        }
        else if (collision.gameObject.CompareTag("Peach"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            gameManager.UpdateP2Score(pointValue +1);
        }
        else if (collision.gameObject.CompareTag("Watermelon"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            hasPowerup = true;
            transform.localScale = new Vector3(scaleMod, scaleMod, scaleMod) * powerUpScaleMod;
            StartCoroutine(WatermelonCooldown());
        }
        else if (collision.gameObject.CompareTag("Banana"))
        {
            Destroy(collision.gameObject);
            playerRb.velocity = Vector3.zero;
            hasBanana = true;
            transform.localScale = new Vector3(-scaleMod, -scaleMod, -scaleMod);
            speed = 0;
            StartCoroutine(BananaCooldown());
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
}


