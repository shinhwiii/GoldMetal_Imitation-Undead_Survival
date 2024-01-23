using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float enemyTimer;
    float boxTimer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / (spawnData.Length-1);
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        enemyTimer += Time.deltaTime;
        boxTimer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length-2);

        if(enemyTimer > spawnData[level].spawnTime)
        {
            enemyTimer = 0f;
            EnemySpawn();
        }

        if (boxTimer > spawnData[5].spawnTime)
        {
            boxTimer = 0f;
            BoxSpawn();
        }
    }

    void EnemySpawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0, false);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }

    void BoxSpawn()
    {
        GameObject box = GameManager.instance.pool.Get(5, true);
        box.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        box.GetComponent<Box>().Init(spawnData[5]);
    }
}

[System.Serializable] // 직렬화되어 인스펙터 창에 뜨게 됨
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
