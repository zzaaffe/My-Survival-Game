using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Controller : ObjectBase
{
    [SerializeField] Animator animator;

    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        animator.SetTrigger("Hurt");
        PlayAudio(0);
    }

    protected override void Dead()
    {
        base.Dead();
        Destroy(gameObject);
    }
}
