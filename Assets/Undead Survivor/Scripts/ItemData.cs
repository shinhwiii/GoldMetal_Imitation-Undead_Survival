using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")] // CreateAssetMenu : 커스텀 메뉴를 생성하는 속성 (Create에서 다음과 같은 속성의 메뉴를 생성할 수 있게 됨)
public class ItemData : ScriptableObject // ScriptableObject : 다양한 데이터를 저장하는 애셋
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public float[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}
