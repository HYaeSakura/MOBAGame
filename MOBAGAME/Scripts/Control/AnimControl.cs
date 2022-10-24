using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画控制脚本
/// </summary>
public class AnimControl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// 播放闲置动画
    /// </summary>
    public void Free()
    {
        animator.SetBool("WALK", false);
    }

    public void Idle()
    {
        animator.SetBool("IDLE", true);
    }

    /// <summary>
    /// 播放攻击动画
    /// </summary>
    public void Attack()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("ATTACK");
    }

    /// <summary>
    /// 播放技能1动画
    /// </summary>
    public void Skill1()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("SKILL2");
    }

    public void Skill2()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("SKILL");
    }

    /// <summary>
    /// 播放行走动画
    /// </summary>
    public void Walk()
    {
        animator.SetBool("WALK", true);
    }

    /// <summary>
    /// 播放死亡动画
    /// </summary>
    public void Death()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("DEATH");
    }
}

/// <summary>
/// 动画状态
/// </summary>
public enum AnimState
{
    FREE,
    IDLE,
    WALK,
    ATTACK,
    SKILL2,
    SKILL,
    DEATH,
    CHEER
}
