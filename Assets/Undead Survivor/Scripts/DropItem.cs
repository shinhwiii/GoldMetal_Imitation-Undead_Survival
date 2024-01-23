using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public int id;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        switch (id)
        {
            case 0:
                GameManager.instance.GetExp(1);
                break;
            case 1:
                GameManager.instance.GetExp(5);
                break;
            case 2:
                GameManager.instance.GetExp(15);
                break;
            case 3:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
            case 4:
                GameManager.instance.isMag = true;
                break;
        }

        gameObject.SetActive(false);
    }
}
