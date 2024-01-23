using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float per; // 관통
    bool isRotate;   

    float move_speed;
    float move_x_rate;
    float move_y_rate;

    Vector3 dir;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, float per, Vector3 dir, int id)
    {
        this.damage = damage;
        this.per = per;
        this.dir = dir;

        switch(id)
        {
            case 1:
                rigid.velocity = dir * 5;
                break;
            case 2:
                rigid.velocity = dir * 15;
                break;
            case 3:
                transform.localPosition = Vector3.zero;
                isRotate = true;
                break;
        }
    }

    void Start()
    {
        if (!isRotate)
            return;

        Vector2 ran;
        do { 
            ran = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        } while (ran == Vector2.zero);
        ran = ran.normalized * 5f;        

        rigid.AddForce(ran, ForceMode2D.Impulse);        

        move_speed = 5f;
    }

    void Update()
    {
        if (isRotate)
        {            
            transform.Rotate(0f, 0f, 500f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if(isRotate)
        {
            transform.Translate(Vector3.right * Time.deltaTime * move_speed * move_x_rate, Space.World);
            transform.Translate(Vector3.up * Time.deltaTime * move_speed * move_y_rate, Space.World);

            // 카메라를 벗어나지 않도록 범위 제한
            Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
            if (position.x < 0f)
            {
                position.x = 0f;
                move_x_rate = 1;
                rigid.velocity = Vector2.zero;
            }
            if (position.y < 0f)
            {
                position.y = 0f;
                move_y_rate = 1;
                rigid.velocity = Vector2.zero;
            }
            if (position.x > 1f)
            {
                position.x = 1f;
                move_x_rate = -1;
                rigid.velocity = Vector2.zero;
            }
            if (position.y > 1f)
            {
                position.y = 1f;
                move_y_rate = -1;
                rigid.velocity = Vector2.zero;
            }
            transform.position = Camera.main.ViewportToWorldPoint(position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    { 
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
