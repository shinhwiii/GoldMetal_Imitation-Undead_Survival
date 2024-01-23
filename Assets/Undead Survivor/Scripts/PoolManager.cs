using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩들을 보관할 변수
    public GameObject[] prefabs;
    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index, bool isBox)
    {
        GameObject select = null;

        // 선택한 풀에서 비활성화된 오브젝트 접근
        if (!isBox)
        {
            foreach (GameObject item in pools[index])
            {
                if (!item.activeSelf) // 발견하면 select에 할당
                {
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }
        }

        // 못 찾았다면 
        if(!select)
        {   // 새롭게 생성해서 select에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
