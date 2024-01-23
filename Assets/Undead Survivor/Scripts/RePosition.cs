using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePosition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area")) // CompareTag가 tag보다 효율적임
        {
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 myPos = transform.position;

            switch(transform.tag)
            {
                case "Ground":
                    float diffX = playerPos.x - myPos.x;
                    float diffY = playerPos.y - myPos.y;
                    float dirX = diffX < 0 ? -1 : 1;
                    float dirY = diffY < 0 ? -1 : 1;
                    diffX = Mathf.Abs(diffX);
                    diffY = Mathf.Abs(diffY);

                    if (diffX > diffY) // x축이 y축보다 많이 이동했다면
                    {
                        transform.Translate(Vector3.right * dirX * 40); // 40*40이므로 방향에 따라 40만큼 더 이동시킴
                    }
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                    break;
                case "Enemy":
                    if(coll.enabled)
                    {
                        Vector3 dist = playerPos - myPos;
                        Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);

                        transform.Translate(dist * 2 + ran);
                    }
                    break;
            }
        }
    }
}
