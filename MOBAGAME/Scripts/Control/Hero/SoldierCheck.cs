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
    /// �Ƿ��Ǽ�����λ
    /// </summary>
    private bool isFriend;

    protected override void Start()
    {
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "E", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "Q", typeof(AudioClip), this);
        //��ֵ������Ϣ
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
    /// ����������� �����˺� 
    /// </summary>
    public override void RequestAttack()
    {
        if (state == AnimState.DEATH)
            return;
        //�������������Ĺ��� ��ôreturn
        if (this != GameData.MyControl)
            return;

        //��ȡ����ID
        int myId = GameData.MyControl.Model.Id;
        //��ȡĿ���ID
        int targetId = target.GetComponent<BaseControl>().Model.Id;
        //����һ�������˺������󣺲��� 1��ʹ����ID 2������ID��3��Ŀ��ID����
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Damage, myId, 1, new int[] { targetId });
    }

    
}
