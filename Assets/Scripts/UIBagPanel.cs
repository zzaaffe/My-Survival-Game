using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBagPanel : MonoBehaviour
{
    public static UIBagPanel Instance;

    private UIBagPanelItem[] items;
    [SerializeField] GameObject itemPrefab;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        items = new UIBagPanelItem[5];
        // 生成篝火
        UIBagPanelItem item = Instantiate(itemPrefab, transform).GetComponent<UIBagPanelItem>();
        item.Init(ItemManager.Instance.GetIteamDefine(ItemType.Campfire));
        items[0] = item;
        for (int i = 1; i < 5; i++)
        {
            item = Instantiate(itemPrefab, transform).GetComponent<UIBagPanelItem>();
            item.Init();
            items[i] = item;
        }
    }
    /// <summary>
    /// 添加物品
    /// </summary>
    public bool AddItem(ItemType itemType)
    {
        for(int i = 0;i < items.Length;i++)
        {
            // 有空格子
            if (items[i].iteamDefine == null)
            {
                ItemDefine iteamDefine = ItemManager.Instance.GetIteamDefine(itemType);
                items[i].Init(iteamDefine);
                return true;
            }
        }
        return false;
    }
}
