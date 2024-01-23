using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.125f, 0);
    Vector3 rightPosReverse = new Vector3(-0.35f, -0.125f, 0);
    Vector3 leftPos = new Vector3(-0.15f, -0.35f, 0);
    Vector3 leftPosReverse = new Vector3(0.15f, -0.35f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if(isLeft) // 근접 무기
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            transform.localPosition = isReverse ? leftPosReverse : leftPos;
            spriter.flipY = isReverse;
        }
        else if (GameManager.instance.player.scanner.nearestTarget) // 적이 사정거리에 있을 때 원거리 무기가 적을 따라감
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;

            Vector3 targetPos = GameManager.instance.player.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            transform.localRotation = Quaternion.FromToRotation(Vector3.right, dir);

            bool isRotA = transform.localRotation.eulerAngles.z > 90 && transform.localRotation.eulerAngles.z < 270;
            bool isRotB = transform.localRotation.eulerAngles.z < -90 && transform.localRotation.eulerAngles.z > -270;
            spriter.flipX = false;
            spriter.flipY = isRotA || isRotB;
        }
        else // 적이 사정거리에 없을 때 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            transform.localRotation = Quaternion.identity;
            spriter.flipX = isReverse;
            spriter.flipY = false;
        }
    }
}
