using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.EventSystems;


public class Player_Controller : ObjectBase
{
    public static Player_Controller Instance;
    [SerializeField] Animator animator;
    [SerializeField] CharacterController characterController;
    [SerializeField] CheckCollider checkCollider;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float hungrySpeed = 3f;


    private float hungryValue = 100f;
    private bool isAttacking = false; // 是否正在攻击
    private Quaternion targetDirQuaternion;
    private bool isHurting = false; //是否正在受伤
    public bool isDarging = false; // 是否正在拖拽物品s

    public float HungryValue 
    {   get => hungryValue;
        set 
        {
            hungryValue = value;
            if (hungryValue <= 0)
            {
                hungryValue = 0;
                Hp -= Time.deltaTime / 2;
            }
            UIManager.Instance.UpdateHungry();
        }
    }

    enum States
    {
        Idle,
        Walk,
        Attack
    }

    private void Awake()
    {
        Instance = this;
        checkCollider.Init(this, 30);
    }

    private void Update()
    {
        UpdateHungry();
        if (!isAttacking && !isHurting)
        {
            Move();
            Attack();
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * rotationSpeed);
        }
    }

    private void Attack()
    {
        if(Input.GetMouseButton(0))
        {   // 当前在拖拽物品 || 鼠标正在交互UI
            if(isDarging || EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100, LayerMask.GetMask("Ground")))
            {
                animator.SetTrigger("Attack");
                // 进入攻击状态
                isAttacking = true;
                // 转向攻击点
                targetDirQuaternion = Quaternion.LookRotation(hitInfo.point - transform.position);
            }
        }
    }

    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        animator.SetTrigger("Hurt");
        PlayAudio(2);
        isHurting = true;

    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs(horizontal)  >= 0.1f || Mathf.Abs(vertical) >= 0.1f)
        {
            animator.SetBool("Walk", true);

            targetDirQuaternion = Quaternion.LookRotation(new Vector3(horizontal, 0, vertical));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * rotationSpeed);

            characterController.SimpleMove(new Vector3(horizontal, 0, vertical).normalized * moveSpeed);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    private void UpdateHungry()
    {
        HungryValue -= Time.deltaTime * hungrySpeed;
    }

    protected override void OnHpUpdate()
    {
        UIManager.Instance.UpdateHp();
    }

    public override bool AddItem(ItemType itemType)
    {
        // 检测背包能否放下
        return UIBagPanel.Instance.AddItem(itemType);
    }

    public bool UseItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Meat:
                Hp += 10;
                HungryValue += 30;
                return true;
                break;
            case ItemType.CookMeat:
                Hp += 20;
                HungryValue += 50;
                return true;
                break;
        }
        return false;
    }

    #region 动画事件
    public void StartHit()
    {
        // 播放音效
        PlayAudio(0);
        // 攻击检测
        checkCollider.StartHit();
    }

    public void StopHit()
    {
        // 停止攻击检测
        isAttacking = false;
        checkCollider.StopHit();
    }

    private void HurtOver()
    {
        isHurting = false;
    }
    #endregion
}
