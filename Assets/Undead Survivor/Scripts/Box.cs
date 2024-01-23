using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float health;
    public float maxHealth;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriteRenderer;
    Animator anim;

    WaitForSeconds wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForSeconds(0.1f);
    }

    void OnEnable()
    {
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 4;
        anim.SetBool("Dead", false);
        StopCoroutine(GetItem());
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0) // 피격 받았을 때
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else // 죽었을 때
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            anim.SetBool("Dead", true);

            StartCoroutine(GetItem());
            Invoke("Dead", 2f);
        }
    }

    IEnumerator GetItem()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Open);
        GameObject[] item = new GameObject[8];

        for (int i = 0; i < 8; i++)
        {
            yield return wait;

            int ran = Random.Range(1, 101);

            if (ran < 71) // 70% 확률로 1 exp 획득
            {
                item[i] = GameManager.instance.pool.Get(6, false);
            }
            else if (ran < 86) // 15% 확률로 5 exp 획득
            {
                item[i] = GameManager.instance.pool.Get(7, false);
            }
            else if (ran < 91) // 5% 확률로 15 exp 획득
            {
                item[i] = GameManager.instance.pool.Get(8, false);
            }
            else if (ran < 96) // 5% 확률로 자석 획득
            {
                item[i] = GameManager.instance.pool.Get(10, false);
            }
            else // 5% 확률로 회복
            {
                item[i] = GameManager.instance.pool.Get(9, false);
            }
            item[i].transform.localPosition = transform.position;

            switch(i)
            {
                case 0:
                    item[i].transform.Translate(new Vector3(-1f, -1f, 0f));
                    break;
                case 1:
                    item[i].transform.Translate(new Vector3(0f, -1f, 0f));
                    break;
                case 2:
                    item[i].transform.Translate(new Vector3(1f, -1f, 0f));
                    break;
                case 3:
                    item[i].transform.Translate(new Vector3(1f, 0f, 0f));
                    break;
                case 4:
                    item[i].transform.Translate(new Vector3(1f, 1f, 0f));
                    break;
                case 5:
                    item[i].transform.Translate(new Vector3(0f, 1f, 0f));
                    break;
                case 6:
                    item[i].transform.Translate(new Vector3(-1f, 1f, 0f));
                    break;
                case 7:
                    item[i].transform.Translate(new Vector3(-1f, 0f, 0f));
                    break;
            }
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
