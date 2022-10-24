using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee : BaseControl
{
    bool a = true;
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    private TurretCheck check;

    /// <summary>
    /// Ŀ���
    /// </summary>
    [SerializeField]
    private Transform point;

    /// <summary>
    /// �Ƿ��Ǽ�����λ
    /// </summary>
    private bool isFriend;

    protected override void Start()
    {
        base.Start();
        //��ֵ������Ϣ
        check.SetTeam(Model.Team);

        isFriend = GameData.MyControl.Model.Team == Model.Team;
    }

    /// <summary>
    /// �����˺� 
    /// </summary>
    public override void RequestAttack()
    {
        if (state == AnimState.DEATH)
            return;

        int attackId = Model.Id;
        int targetId = target.Model.Id;
        //����һ�������˺������󣺲��� 1��ʹ����ID 2������ID��3��Ŀ��ID����
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Skill, 1, attackId, targetId, -1f, -1f, -1f);

        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Damage, attackId, 1, new int[] { targetId });
    }

    /// <summary>
    /// ͬ����������
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
        if (!isFriend)
            return;

        //�ȼ����û��Ŀ��
        if (this.target == null)
        {
            if (check.conList.Count == 0)
            {
                DogMove(point.transform.position, a);
                return;
            }
            this.target = check.conList[0];
        }

        //���Ŀ����û������
        if (this.target.Model.CurrHp <= 0 && this.target != null)
        {
            this.check.conList.Remove(this.target);
            this.target = null;
            a = true;
            return;
        }

        //�����Ŀ��  ���Ǿ͹���
        if (this.target != null)
        {
            //�ж�һ�¹�������
            float d = Vector3.Distance(transform.position, target.transform.position);
            if (d > Model.AttackDistance)
            {
                DogMove(target.transform.position, true);
            }
            else//�ڹ�����Χ�� ֱ�ӹ���
            {
                //ֹͣѰ·
                agent.isStopped = true;
                //һ��Ҫ����Ŀ��
                transform.LookAt(target.transform);
                //���Ŷ���
                animControl.Attack(); 
            }
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
        gameObject.SetActive(false);
    }

    /// <summary>
    /// С���ƶ�
    /// </summary>
    /// <param name="point"></param>
    private void DogMove(Vector3 point, bool send)
    {
        if (send)
        {
            a = false;
            //����������һ������ �������������
            PhotonManager.Instance.Request(OpCode.FightCode, OpFight.DogWalk, Model.Id, point.x, point.y, point.z);
        }
    }
}