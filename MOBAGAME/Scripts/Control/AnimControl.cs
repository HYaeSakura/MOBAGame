using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ƽű�
/// </summary>
public class AnimControl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// �������ö���
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
    /// ���Ź�������
    /// </summary>
    public void Attack()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("ATTACK");
    }

    /// <summary>
    /// ���ż���1����
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
    /// �������߶���
    /// </summary>
    public void Walk()
    {
        animator.SetBool("WALK", true);
    }

    /// <summary>
    /// ������������
    /// </summary>
    public void Death()
    {
        animator.SetBool("WALK", false);
        animator.SetTrigger("DEATH");
    }
}

/// <summary>
/// ����״̬
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
