using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Move,
    Pursue,
    Attack,
    Hurt,
    Die
}

public class Boar_Controller : ObjectBase
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] CheckCollider checkCollider;
    [SerializeField] float rotationSpeed = 4f;
    //行动范围
    public float maxX = 4.74f;
    public float minX = -5.62f;
    public float maxZ = 5.92f;
    public float minZ = -6.33f;

    private EnemyState enemyState;
    private Vector3 targerPos;

    public EnemyState EnemyState 
    { 
        get => enemyState; 
        set
        {
            enemyState = value;
            switch (enemyState)
            {
                case EnemyState.Idle:
                    // 播放动画
                    animator.CrossFadeInFixedTime("Idle",0.25f);
                    // 关闭导航
                    navMeshAgent.enabled = false;
                    // 一段时间自动巡逻
                    Invoke(nameof(GoMove), Random.Range(3f, 10f));
                    break;
                case EnemyState.Move:
                    // 播放动画
                    animator.CrossFadeInFixedTime("Move", 0.25f);
                    // 开启导航
                    navMeshAgent.enabled = true;
                    targerPos = GetTargetPos();
                    navMeshAgent.SetDestination(targerPos);
                    
                    break;
                case EnemyState.Pursue:
                    // 播放动画
                    animator.CrossFadeInFixedTime("Move", 0.25f);
                    // 开启导航
                    navMeshAgent.enabled = true;

                    break;
                case EnemyState.Attack:
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(Player_Controller.Instance.transform.position - transform.position),rotationSpeed);
                    // 播放动画
                    animator.CrossFadeInFixedTime("Attack", 0.25f);
                    // 关闭导航
                    navMeshAgent.enabled = false;
                    break;
                case EnemyState.Hurt:
                    // 播放动画
                    animator.CrossFadeInFixedTime("Damage", 0.25f);
                    PlayAudio(0);
                    // 关闭导航
                    navMeshAgent.enabled = false;
                    break;
                case EnemyState.Die:
                    PlayAudio(0);
                    // 关闭导航
                    navMeshAgent.enabled = false;
                    // 播放动画
                    animator.CrossFadeInFixedTime("Die", 0.25f);
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        checkCollider.Init(this, 10);
        EnemyState = EnemyState.Idle;
    }

    private void Update()
    {
        StateOnUpdate();
    }

    private void StateOnUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.Move:
                if (Vector3.Distance(transform.position,targerPos) < 1.5f)
                {
                    EnemyState = EnemyState.Idle;
                }
                break;
            case EnemyState.Pursue:
                if(Vector3.Distance(transform.position,Player_Controller.Instance.transform.position) < 1)
                {
                    EnemyState = EnemyState.Attack;
                }
                else
                {
                    navMeshAgent.SetDestination(Player_Controller.Instance.transform.position);
                }
                break;
            default:
                break;
        }
    }

    private void GoMove()
    {
        EnemyState = EnemyState.Move;
    }

    private Vector3 GetTargetPos()
    {
        return new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
    }

    public override void Hurt(int damage)
    {
        if (EnemyState == EnemyState.Die) return;
        CancelInvoke(nameof(GoMove));
        base.Hurt(damage);
        if(Hp > 0)
        {
            EnemyState = EnemyState.Hurt;
        }
    }

    protected override void Dead()
    {
        base.Dead();
        EnemyState = EnemyState.Die;

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
        checkCollider.StopHit();
    }

    public void StopAttack()
    {
        if (EnemyState != EnemyState.Die)
        {
            EnemyState = EnemyState.Pursue;
        }

    }

    public void HurtOver()
    {
        if(EnemyState != EnemyState.Die)
        {
            EnemyState = EnemyState.Pursue;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    #endregion
}
