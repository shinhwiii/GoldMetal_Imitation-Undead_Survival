using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpMagnet : MonoBehaviour
{
    public Transform target;

    public float moveSpeed = 300.0f;

    void Update()
    {
        if (!GameManager.instance.isMag)
        {
            transform.localRotation = Quaternion.identity;
            return;
        }

        target = GameManager.instance.player.transform;

        Vector2 relativePos = target.transform.position - transform.position;
        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
    }
}
