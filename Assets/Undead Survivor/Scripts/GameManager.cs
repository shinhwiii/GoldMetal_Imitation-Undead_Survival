using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // static은 인스펙터 창에 안나옴

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime;
    public bool isInfinite;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    public bool isMag;
    public float magTime;
    public float magCoolTime;

    [Header("# Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiGameResult;
    public Transform uiJoy;
    public GameObject uiHealth;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void Setting(int difficulty)
    {
        switch(difficulty)
        {
            case 0:
                maxHealth = 200;
                player.speed = 3.5f;
                maxGameTime = 3 * 60f;
                isInfinite = false;
                break;
            case 1:
                maxHealth = 100;
                player.speed = 3f;
                maxGameTime = 4 * 60f;
                isInfinite = false;
                break;
            case 2:
                maxHealth = 80;
                player.speed = 2.8f;
                maxGameTime = 5 * 60f;
                isInfinite = false;
                break;
            case 3:
                maxHealth = 100;
                player.speed = 3f;
                isInfinite = true;
                break;   
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);

        switch(playerId)
        {
            case 0:
                uiLevelUp.Select(3);
                break;
            case 1:
                uiLevelUp.Select(2);
                break;
            case 2:
                uiLevelUp.Select(0);
                break;
            case 3:
                uiLevelUp.Select(1);
                break;
        }

        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        isMag = false;

        yield return new WaitForSeconds(1f);

        uiHealth.gameObject.SetActive(false);
        uiGameResult.gameObject.SetActive(true);
        uiGameResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        enemyCleaner.SetActive(false);

        uiGameResult.gameObject.SetActive(true);
        uiGameResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }


    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime && !isInfinite)
        {
            gameTime = maxGameTime;
            GameVictory();
        }


        if (!isMag)
            return;
            
        magTime += Time.deltaTime;

        if (magTime > magCoolTime)
        {
            magTime = 0f;
            isMag = false;
        }
    }

    public void GetExp(int amount)
    {
        if (!isLive)
            return;

        exp += amount;

        if(exp >= nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            exp -= nextExp[Mathf.Min(level, nextExp.Length - 1)];
            level++;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }

    public void SelectButtonSound()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }
}
