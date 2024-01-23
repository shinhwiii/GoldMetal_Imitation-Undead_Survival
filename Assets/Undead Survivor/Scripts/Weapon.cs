using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public float count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        switch (id)
        {
            case 0:            
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 1:
            case 2:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, float count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0 || id == 3)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch(id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            case 1:
                speed = 3f * Character.WeaponRate;
                break;
            case 2:
                speed = 0.5f * Character.WeaponRate;
                break;
            case 3:
                speed = 10f * Character.WeaponRate;
                Batch();
                break;
        }

        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // BroadcastMessage : 특정 함수 호출을 모든 자식에게 방송하는 함수
    }

    void Batch()
    {
        if(id == 0)
        {
            for (int index = 0; index < count; index++)
            {
                Transform bullet;

                if (index < transform.childCount)
                {
                    bullet = transform.GetChild(index);
                }
                else
                {
                    bullet = GameManager.instance.pool.Get(prefabId, false).transform;
                    bullet.parent = transform;
                }

                bullet.localPosition = Vector3.zero;
                bullet.localRotation = Quaternion.identity;

                Vector3 rotVec = Vector3.forward * 360 * index / count;
                bullet.Rotate(rotVec);
                bullet.Translate(bullet.up * 1.5f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector2.zero, 0); // -100은 무한으로 관통하는 근접공격
            }
        }

        else if(id == 3)
        {
            for (int index = 0; index < count; index++)
            {
                Transform bullet;

                if (index < transform.childCount)
                {
                    bullet = transform.GetChild(index);
                }
                else
                {
                    bullet = GameManager.instance.pool.Get(prefabId, false).transform;
                    bullet.parent = transform;
                }

                Vector3 rotVec = Vector3.forward * 360 * index / count;
                bullet.Rotate(rotVec);
                bullet.Translate(bullet.up * 1.5f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized, id);
            }
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId, false).transform;
        bullet.parent = transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        if (id == 1)
        {
            bullet.GetComponent<Bullet>().Init(damage, -100, dir, id);
            bullet.localScale = Vector3.one * count;
        }
        else if (id == 2)
        {
            bullet.GetComponent<Bullet>().Init(damage, count, dir, id);
        }

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
