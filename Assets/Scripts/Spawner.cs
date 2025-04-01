using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnPrefabs;
    public float speed = 5;
    private Vector3 offset = new Vector3(0, 1.5f, 0);

    private float xRange = 8;
    public bool isMovingRight;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SpawnRandom();
        //StartCoroutine(SpawnCountdownCoroutine());
    }

    IEnumerator SpawnCountdownCoroutine()
    {
        yield return new WaitForSeconds(Random.Range (2, 5));
        SpawnRandom();
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > xRange)
        {
            isMovingRight = false;
        }
        else if (transform.position.x < -xRange)
        {
            isMovingRight = true;
        }

        if (isMovingRight)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        
    }

    public void SpawnRandom()
    {
        if (gameManager.isGameActive)
        {
            int spawnNumber = Random.Range(1, 4);
            for (int i = 0; i < spawnNumber; i++)
            {
                int spawnIndex = Random.Range(0, spawnPrefabs.Length);
                Instantiate(spawnPrefabs[spawnIndex], transform.position + offset, transform.rotation);

            }
            StartCoroutine(SpawnCountdownCoroutine());

        }
            
        
    }



}
