using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToClientAddView : MonoBehaviour
{
    [SerializeField]
    private Text txtInfo;

    public int Id;

    public void UpdateView(PlayerDto player)
    {
        this.Id = player.id;
        txtInfo.text =
            string.Format("������{0}\n�ȼ���{1}\n���Ѹ�����{2}\n���ܳ��Σ�{3}", player.name, player.lv, player.friends.Length, player.runCount);
    }
}
