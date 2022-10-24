using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MobaCommon.Dto;

/// <summary>
/// սʿӢ�۵Ŀ��ƽű�
/// </summary>
public class Soldier : BaseControl, IResourceListener
{
    protected override void Start()
    {
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "SoldierAttack", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "SoldierDeath", typeof(AudioClip), this); 
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "E", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "Q", typeof(AudioClip), this);
    }

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
            case Paths.RES_SOUND_FIGHT + "SoldierAttack":
                nameClipDict.Add("Attack", asset as AudioClip);
                break;
            case Paths.RES_SOUND_FIGHT + "SoldierDeath":
                nameClipDict.Add("Death", asset as AudioClip);
                break;
            case Paths.RES_SOUND_FIGHT + "E":
                nameClipDict.Add("E", asset as AudioClip);
                break;
            case Paths.RES_SOUND_FIGHT + "Q":
                nameClipDict.Add("Q", asset as AudioClip);
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
        
        //����Ŀ��
        this.target = target[0].GetComponent<BaseControl>();
        //һ��Ҫ����Ŀ��
        transform.LookAt(target[0]);
        //���Ŷ���
        animControl.Attack();
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
                targetPos.y = transform.position.y;
                //�����
                transform.LookAt(targetPos);
                //���Ŷ���
                animControl.Skill1();
                this.PlayAudio("Q");
                //�ı�״̬
                state = AnimState.FREE;
                break;
            case 1002:
                targetPos.y = transform.position.y;
                //�����
                transform.LookAt(targetPos);
                //���Ŷ���
                animControl.Skill2();
                this.PlayAudio("Q");
                //�ı�״̬
                state = AnimState.FREE;
                break;
            case 1003:
                targetPos.y = transform.position.y;
                //�����
                transform.LookAt(targetPos);
                //���ؼ��ܵĹ�����Ч
                GameObject go3 = PoolManager.Instance.GetObject("E");
                this.PlayAudio("E");
                //��ʼ���ű�
                go3.GetComponent<LineSkill>().Init(
                    transform,
                    (float)((HeroModel)this.Model).Skills[2].Distance,
                    1.5f,
                    skillId,
                    this.Model.Id,
                    this == GameData.MyControl);
                break;
            case 1004:
                targetPos.y = transform.position.y;
                //�����
                transform.LookAt(targetPos);
                //���Ŷ���
                animControl.Skill1();
                this.PlayAudio("Q");
                //�ı�״̬
                state = AnimState.FREE;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
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

    public override void ResurgeResponse() 
    {
        gameObject.SetActive(true);
    }
}
