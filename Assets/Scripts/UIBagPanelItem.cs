using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIBagPanelItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] Image bg;
    [SerializeField] Image iconImg;

    public ItemDefine iteamDefine;
    private bool isSelect = false;

    public bool IsSelect 
    { 
        get => isSelect;
        set 
        {
            isSelect = value;
            if (isSelect)
            {
                bg.color = Color.green;
            }
            else
            {
                bg.color = Color.white;
            }
        }
    }

    private void Update()
    {
        if(isSelect && iteamDefine != null && Input.GetMouseButtonDown(1))
        {
            if(Player_Controller.Instance.UseItem(iteamDefine.ItemType))
            {
                Init(null);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsSelect = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsSelect = false;
    }

    /// <summary>
    /// 初始化，如果传null过来，相当于是空格子
    /// </summary>
    public void Init(ItemDefine itemDefine = null)
    {
        this.iteamDefine = itemDefine;
        IsSelect = false;
        if(this.iteamDefine == null)
        {
            iconImg.gameObject.SetActive(false);
        }
        else
        {
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = iteamDefine.Icon;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (iteamDefine == null) return;
        Player_Controller.Instance.isDarging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (iteamDefine == null) return;
        iconImg.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (iteamDefine == null) return;
        Player_Controller.Instance.isDarging = false;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            string targetTag = hitInfo.collider.tag;
            iconImg.transform.localPosition = Vector3.zero;

            switch (iteamDefine.ItemType)
            {
                case ItemType.Meat:
                    if(targetTag == "Ground")
                    {
                        Instantiate(iteamDefine.Prefab, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }
                    else if(targetTag == "Campfire")
                    {
                        if(Campfire_Controller.Instance.CanBake())
                        {
                            Init(ItemManager.Instance.GetIteamDefine(ItemType.CookMeat));
                        }
                        else
                        {
                            iconImg.transform.localPosition = Vector3.zero;
                        }
                    }
                    break;
                case ItemType.CookMeat:
                    if (targetTag == "Ground")
                    {
                        Instantiate(iteamDefine.Prefab, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }
                    else if(targetTag == "Campfire")
                    {
                        hitInfo.collider.GetComponent<Campfire_Controller>().AddWood();
                        Init(null);
                    }
                    break;
                case ItemType.Wood:
                    if (targetTag == "Ground")
                    {
                        Instantiate(iteamDefine.Prefab, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }
                    else if(targetTag == "Campfire")
                    {
                        hitInfo.collider.GetComponent<Campfire_Controller>().AddWood();
                        Init(null);
                    }
                    break;
                case ItemType.Campfire:
                    if (targetTag == "Ground")
                    {
                        Instantiate(iteamDefine.Prefab, hitInfo.point, Quaternion.identity);
                        Init(null);
                    }
                    break;
                default: 
                    break;
            }
        }
    }
}
