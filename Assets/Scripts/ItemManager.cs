using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Meat,
    CookMeat,
    Wood,
    Campfire
}

public class ItemDefine
{
    public ItemType ItemType;
    public Sprite Icon;
    public GameObject Prefab;

    public ItemDefine(ItemType itemType, Sprite icon, GameObject prefab)
    {
        ItemType = itemType;
        Icon = icon;
        Prefab = prefab;
    }
}


public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    [SerializeField] Sprite[] icons;
    [SerializeField ] GameObject[] itemPrefabs;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 获取物品的定义
    /// </summary>
    public ItemDefine GetIteamDefine(ItemType iteamType)
    {
        return new ItemDefine(iteamType, icons[(int)iteamType - 1], itemPrefabs[(int)iteamType - 1]);
    }

}
