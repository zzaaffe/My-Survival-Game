using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private ObjectBase owner;
    private int damage;
    private bool canAttack = false;
    private List<GameObject> attackedObjectList = new List<GameObject>();
    [SerializeField] List<string> enemyTages = new List<string>(); 
    [SerializeField] List<string> itemTages = new List<string>();
    public void Init(ObjectBase owner, int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }

    public void StartHit()
    {
        canAttack = true;
    }

    public void StopHit()
    {
        canAttack = false;
        attackedObjectList.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if(canAttack)
        {
            if(!attackedObjectList.Contains(other.gameObject) && enemyTages.Contains(other.tag))
            {
                attackedObjectList.Add(other.gameObject);
                other.GetComponent<ObjectBase>().Hurt(damage);
            }
            return;
        }

        if(itemTages.Contains(other.tag))
        {
            // 把捡到的物品变成枚举
            ItemType itemType = System.Enum.Parse<ItemType>(other.tag);
            if(itemType == ItemType.Campfire)
            {
                if(Input.GetKeyDown(KeyCode.F) && owner.AddItem(itemType))
                {
                    owner.PlayAudio(1);
                    Destroy(other.gameObject);
                }
            }
            else if(owner.AddItem(itemType))
            {
                owner.PlayAudio(1);
                Destroy(other.gameObject);
            }
        }
    }
}
