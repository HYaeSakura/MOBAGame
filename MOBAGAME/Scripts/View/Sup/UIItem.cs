using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField]
    private int Id;

    public void OnClick()
    {
        //������������������
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Buy, this.Id);
    }
}
