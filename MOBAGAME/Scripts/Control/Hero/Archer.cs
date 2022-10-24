using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ӣ�ۿ��ƽű�
/// </summary>
public class Archer : BaseControl, IResourceListener
{
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    private Transform atkPoint;

    protected override void Start()
    {
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "ArcherAttack", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "ArcherDeath", typeof(AudioClip), this);
    }

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
            case Paths.RES_SOUND_FIGHT + "ArcherAttack":
                nameClipDict.Add("Attack", asset as AudioClip);
                break;
            case Paths.RES_SOUND_FIGHT + "ArcherDeath":
                nameClipDict.Add("Death", asset as AudioClip);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ����������� �����˺� 
    /// </summary>
    public override void RequestAttack()
    {
        if (state == AnimState.DEATH)
            return;
        //�������������Ĺ��� ��ôreturn
        if (this != GameData.MyControl)
            return;

        //��������
        this.PlayAudio("Attack");
        //��ȡ����ID
        int myId = GameData.MyControl.Model.Id;
        //��ȡĿ���ID
        int targetId = target.GetComponent<BaseControl>().Model.Id;
        //����һ�������˺������󣺲��� 1��ʹ����ID 2������ID��3��Ŀ��ID����
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Damage, myId, 1, new int[] { targetId });
    }

    /// <summary>
    /// ͬ���������� ���������˺���������ʾ�����ģ�
    /// </summary>
    /// <param name="target"></param>
    public override void AttackResponse(params Transform[] target)
    {
        if (state == AnimState.DEATH)
            return;

        //��Ŀ�귢һ��������Ч ��������֮�� �ټ����˺�
        GameObject go = PoolManager.Instance.GetObject("arrow");
        go.transform.position = atkPoint.position;
        //����Ŀ��
        this.target = target[0].GetComponent<BaseControl>();
        //һ��Ҫ����Ŀ��
        transform.LookAt(target[0]);
        //����״̬
        state = AnimState.ATTACK;
    }

    protected override void Update()
    {
        base.Update();

        //�����Ŀ�� �������ǹ���״̬ ���Ǿ͹���
        if (target != null && state == AnimState.ATTACK)
        {
            //�ȼ�⹥����Χ
            float distance = Vector3.Distance(transform.position, target.transform.position);
            //����������Χ �ߵ������һ���� Ȼ�󹥻�
            if (distance > Model.AttackDistance)
            {
                //��Ŀ���߶�
                agent.ResetPath();
                agent.SetDestination(target.transform.position);
                //���Ŷ���
                animControl.Walk();
            }
            else//�ڹ�����Χ�� ֱ�ӹ���
            {
                //ֹͣѰ·
                agent.isStopped = true;
                //һ��Ҫ����Ŀ��
                transform.LookAt(target.transform);
                //���Ŷ���
                animControl.Attack();
                //�ı�״̬
                state = AnimState.FREE;
            }
        }
    }

    public override void SkillResponse(int skillId, Transform target, Vector3 targetPos)
    {
        //�������������� ��ôreturn
        if (this != GameData.MyControl)
            return;

        switch (skillId)
        {
            case 1001:
                
                break;
            case 1002:
                
                break;
            case 1003:
                
                break;
            case 1004:

                break;
            default:
                break;
        }
    }

    public override void DeathResponse()
    {
        //ֹͣѰ·
        agent.isStopped = true;
        //���Ŷ���
        animControl.Death();
        //�ı�״̬
        state = AnimState.DEATH;
        //��������
        this.PlayAudio("Death");
    }
}
