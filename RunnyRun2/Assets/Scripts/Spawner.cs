using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;

    public float obstacleSpawnTime = 3f;
    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;

    public float obstacleSpeed = 4f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f;

    private float _obstacleSpawnTime;
    private float InGameObstacleSpeed;

    private float timeAlive;
    private float timeUntilObstacleSpawn = 2f;

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearObstacles);
        GameManager.Instance.onPlay.AddListener(resetFactors);
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            timeAlive += Time.deltaTime;

            CalculateFactors();

            SpawnLoop();
        }
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if (timeUntilObstacleSpawn >= _obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ClearObstacles()
    {
        foreach(Transform child in obstacleParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        InGameObstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);

    }

    private void resetFactors()
    {
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        InGameObstacleSpeed = obstacleSpeed;

    }

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject obstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        obstacle.transform.parent = obstacleParent;

        Rigidbody2D obstacleRB = obstacle.GetComponent<Rigidbody2D>();

        obstacleRB.velocity = Vector2.left * InGameObstacleSpeed;
    }
}

