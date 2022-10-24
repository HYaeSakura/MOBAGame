using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee : BaseControl
{
    bool a = true;
    /// <summary>
    /// 检测
    /// </summary>
    [SerializeField]
    private TurretCheck check;

    /// <summary>
    /// 目标点
    /// </summary>
    [SerializeField]
    private Transform point;

    /// <summary>
    /// 是否是己方单位
    /// </summary>
    private bool isFriend;

    protected override void Start()
    {
        base.Start();
        //赋值队伍信息
        check.SetTeam(Model.Team);

        isFriend = GameData.MyControl.Model.Team == Model.Team;
    }

    /// <summary>
    /// 计算伤害 
    /// </summary>
    public override void RequestAttack()
    {
        if (state == AnimState.DEATH)
            return;

        int attackId = Model.Id;
        int targetId = target.Model.Id;
        //发起一个计算伤害的请求：参数 1、使用者ID 2、技能ID，3、目标ID数组
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Skill, 1, attackId, targetId, -1f, -1f, -1f);

        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Damage, attackId, 1, new int[] { targetId });
    }

    /// <summary>
    /// 同步攻击动画
    /// </summary>
    /// <param name="target"></param>
    public override void AttackResponse(params Transform[] target)
    {
        if (state == AnimState.DEATH)
            return;

        //保存目标
        this.target = target[0].GetComponent<BaseControl>();
        //一定要面向目标
        transform.LookAt(target[0]);
        //播放动画
        animControl.Attack();
        //设置状态
        state = AnimState.ATTACK;
    }

    protected override void Update()
    {
        if (!isFriend)
            return;

        //先检测有没有目标
        if (this.target == null)
        {
            if (check.conList.Count == 0)
            {
                DogMove(point.transform.position, a);
                return;
            }
            this.target = check.conList[0];
        }

        //检测目标有没有死亡
        if (this.target.Model.CurrHp <= 0 && this.target != null)
        {
            this.check.conList.Remove(this.target);
            this.target = null;
            a = true;
            return;
        }

        //如果有目标  我们就攻击
        if (this.target != null)
        {
            //判断一下攻击距离
            float d = Vector3.Distance(transform.position, target.transform.position);
            if (d > Model.AttackDistance)
            {
                DogMove(target.transform.position, true);
            }
            else//在攻击范围内 直接攻击
            {
                //停止寻路
                agent.isStopped = true;
                //一定要面向目标
                transform.LookAt(target.transform);
                //播放动画
                animControl.Attack(); 
            }
        } 
    }

    /// <summary>
    /// 死亡动画
    /// </summary>
    public override void DeathResponse()
    {
        //停止寻路
        agent.isStopped = true;
        //播放动画
        animControl.Death();
        //改变状态
        state = AnimState.DEATH;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 小兵移动
    /// </summary>
    /// <param name="point"></param>
    private void DogMove(Vector3 point, bool send)
    {
        if (send)
        {
            a = false;
            //给服务器发一个请求 参数：点的坐标
            PhotonManager.Instance.Request(OpCode.FightCode, OpFight.DogWalk, Model.Id, point.x, point.y, point.z);
        }
    }
}