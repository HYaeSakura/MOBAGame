using MobaCommon.Code;
using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierCheck : BaseControl, IResourceListener
{
    [SerializeField]
    private TurretCheck check;

    /// <summary>
    /// 是否是己方单位
    /// </summary>
    private bool isFriend;

    protected override void Start()
    {
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "E", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "Q", typeof(AudioClip), this);
        //赋值队伍信息
        check.SetTeam(Model.Team);

        isFriend = GameData.MyControl.Model.Team == Model.Team;
    }

    public void OnLoaded(string assetName, object asset)
    {
        switch (assetName)
        {
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
    /// 动画播放完毕 计算伤害 
    /// </summary>
    public override void RequestAttack()
    {
        if (state == AnimState.DEATH)
            return;
        //如果不是自身发起的攻击 那么return
        if (this != GameData.MyControl)
            return;

        //获取自身ID
        int myId = GameData.MyControl.Model.Id;
        //获取目标的ID
        int targetId = target.GetComponent<BaseControl>().Model.Id;
        //发起一个计算伤害的请求：参数 1、使用者ID 2、技能ID，3、目标ID数组
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Damage, myId, 1, new int[] { targetId });
    }

    
}
