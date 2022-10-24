using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using MobaCommon.Code;
using MobaCommon.Dto;
using LitJson;

public class PlayerReceiver : MonoBehaviour, IReceiver
{
    [SerializeField]
    private MainView view;

    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch (subCode)
        {
            case OpPlayer.GetInfo:
                onGetInfo(response.ReturnCode);
                break;
            case OpPlayer.Create:
                onCreate();
                break;
            case OpPlayer.Online:
                PlayerDto dto = JsonMapper.ToObject<PlayerDto>(response[0].ToString());
                onOnline(dto);
                break;
            case OpPlayer.RequestAdd:
                onRequestAdd(response.ReturnCode, response.DebugMessage);
                break;
            case OpPlayer.ToClientAdd:
                onToClientAdd
                    (response.ReturnCode, JsonMapper.ToObject<PlayerDto>(response[0].ToString()));
                break;
            case OpPlayer.FriendOffline:
                onFriendOff((int)response[0]);
                break;
            case OpPlayer.FriendOnline:
                onFriendOn((int)response[0]);
                break;
            case OpPlayer.MatchComplete:
                onMatchComplete();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ƥ��ɹ�
    /// </summary>
    private void onMatchComplete()
    {
        view.OnStopMatch();
        //��������
        SoundManager.Instance.PlayEffectMusic(view.acCountDown);

        MessageTip.Instance.Show("ƥ��ɹ���10s�ڽ��뷿��", () =>
        {
            //�ȹر�������
            UIManager.Instance.HideUIPanel(UIDefinit.UIMain);
            //��ѡ�˽���
            UIManager.Instance.ShowUIPanel(UIDefinit.UISelect);
            //ֹͣ����
            SoundManager.Instance.StopEffectMusic();
        }, 10f);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="friendId"></param>
    private void onFriendOff(int friendId)
    {
        view.UpdateFriendView(friendId, false);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="friendId"></param>
    private void onFriendOn(int friendId)
    {
        view.UpdateFriendView(friendId, true);
    }

    /// <summary>
    /// �ͻ����յ��Ӻ�������
    /// </summary>
    /// <param name="dto"></param>
    private void onToClientAdd(int retCode, PlayerDto dto)
    {
        if (retCode == 0)
            //������������ͬ��ͬ��
            view.ShowToClientAddPanel(dto);
        else if (retCode == 1) //��ӳɹ� ˢ����ͼ
        {
            view.UpdateView(dto);
        }
    }

    /// <summary>
    /// ����Ӻ�������
    /// </summary>
    /// <param name="retCode"></param>
    private void onRequestAdd(int retCode, string mess)
    {
        MessageTip.Instance.Show(mess);
    }

    /// <summary>
    /// ����
    /// </summary>
    private void onOnline(PlayerDto dto)
    {
        //��������
        GameData.Player = dto;
        //ˢ����ͼ
        view.UpdateView(dto);
    }

    /// <summary>
    /// ������ɫ
    /// </summary>
    private void onCreate()
    {
        //���ش������
        view.CreatePanelActive = false;
        //�����ɹ��������ߵ�����
        playerOnline();
    }

    /// <summary>
    /// ��ȡ��Ϣ
    /// </summary>
    /// <param name="retCode"></param>
    private void onGetInfo(int retCode)
    {
        if (retCode == -1)
        {
            //�Ƿ���¼
        }
        else if (retCode == 0)
        {
            //�н�ɫ
            //��ɫ����
            playerOnline();
        }
        else if (retCode == -2)
        {
            //û�н�ɫ
            //��ʾ�������
            view.CreatePanelActive = true;
        }
    }

    /// <summary>
    /// ��ɫ����
    /// </summary>
    private void playerOnline()
    {
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.Online);
    }


}
