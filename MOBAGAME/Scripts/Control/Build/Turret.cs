using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����Ŀ�����
/// </summary>
public class Turret : BaseControl
{
    [SerializeField]
    private TurretCheck check;

    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    private Transform atkPoint;

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

    public override void AttackResponse(params Transform[] target)
    {
        if (state == AnimState.DEATH)
            return;

        //��Ŀ�귢һ��������Ч ��������֮�� �ټ����˺�
        GameObject go = PoolManager.Instance.GetObject("atkTurrent");
        go.transform.position = atkPoint.position;
        int attackId = Model.Id;
        int targetId = target[0].GetComponent<BaseControl>().Model.Id;
        go.GetComponent<TargetSkill>().Init(target[0], 1, attackId, targetId, isFriend);
    }

    public override void DeathResponse()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �������
    /// </summary>
    private float intevalTime = 3f;
    private float timer = 3f;

    protected override void Update()
    {
        base.Update();

        if (!isFriend)
            return;

        //�ȼ����û��Ŀ��
        if (this.target == null)
        {
            if (check.conList.Count == 0)
                return;

            this.target = check.conList[0];
        }
        //���Ŀ����û������
        if (target.Model.CurrHp <= 0)
        {
            this.check.conList.Remove(target);
            this.target = null;
            return;
        }
        //�ж�һ�¹�������
        float d = Vector3.Distance(transform.position, target.transform.position);
        if (d >= Model.AttackDistance)
        {
            this.target = null;
            //���ù���ʱ��
            this.timer = intevalTime;
            return;
        }
        //��ʼ����
        if (timer < intevalTime)
        {
            timer += Time.deltaTime;
            return;
        }

        //����������𹥻�������
        int attackId = Model.Id;
        int targetId = target.Model.Id;
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Skill, 1, attackId, targetId, -1f, -1f, -1f);
        //���ü�ʱ��
        timer = 0f;
    }
}
