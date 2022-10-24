using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MobaCommon.Code;

/// <summary>
/// Photon����
/// </summary>
public class PhotonManager : Singleton<PhotonManager>, IPhotonPeerListener
{
    #region Receivers
    //�˺�
    private AccountReceiver account;
    public AccountReceiver Account
    {
        get
        {
            if (account == null)
                account = FindObjectOfType<AccountReceiver>();
            return account;
        }
    }

    //��ɫ
    private PlayerReceiver player;
    public PlayerReceiver Player
    {
        get
        {
            if (player == null)
                player = FindObjectOfType<PlayerReceiver>();
            return player;
        }
    }

    //ѡ��
    private SelectReceiver select;
    public SelectReceiver Select
    {
        get
        {
            if (select == null)
                select = FindObjectOfType<SelectReceiver>();
            return select;
        }
    }

    //ս��
    private FightReceiver fight;
    public FightReceiver Fight
    {
        get
        {
            if (fight == null)
                fight = FindObjectOfType<FightReceiver>();
            return fight;
        }
    }

    #endregion

    #region Photon�ӿ�

    /// <summary>
    /// ����ͻ���
    /// </summary>
    private PhotonPeer peer;
    /// <summary>
    /// IP��ַ
    /// </summary>
    private string serverAddress = "127.0.0.1:5055";
    /// <summary>
    /// ����
    /// </summary>
    private string applicationName = "MOBA";
    /// <summary>
    /// Э��
    /// </summary>
    private ConnectionProtocol protocol = ConnectionProtocol.Udp;
    /// <summary>
    /// ����Flag
    /// </summary>
    private bool isConnect = false;


    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnEvent(EventData eventData)
    {

    }

    /// <summary>
    /// ���ܷ���������Ӧ
    /// </summary>
    /// <param name="response"></param>
    public void OnOperationResponse(OperationResponse response)
    {
        Log.Debug(response.ToStringFull());
        //������
        byte opCode = response.OperationCode;
        //�Ӳ���
        byte subCode = (byte)response[80];
        //"ת��"
        switch (opCode)
        {
            case OpCode.AccountCode:
                Account.OnReceive(subCode, response);
                break;
            case OpCode.PlayerCode:
                Player.OnReceive(subCode, response);
                break;
            case OpCode.SelectCode:
                Select.OnReceive(subCode, response);
                break;
            case OpCode.FightCode:
                Fight.OnReceive(subCode, response);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ״̬�ı�
    /// </summary>
    /// <param name="statusCode"></param>
    public void OnStatusChanged(StatusCode statusCode)
    {
        Log.Debug(statusCode);
        switch (statusCode)
        {
            case StatusCode.Connect:
                isConnect = true;
                break;
            case StatusCode.Disconnect:
                isConnect = false;
                break;
            default:
                break;
        }
    }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        peer = new PhotonPeer(this, protocol);
        peer.Connect(serverAddress, applicationName);

        //�������������ǿ糡�����ڵ�
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!isConnect)
        {
            peer.Connect(serverAddress, applicationName);
        }
        peer.Service();
    }

    void OnApplicationQuit()
    {
        //�Ͽ�����
        peer.Disconnect();
    }


    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="code">������</param>
    /// <param name="SubCode">�Ӳ�����</param>
    /// <param name="parameters">����</param>
    public void Request(byte code, byte SubCode, params object[] parameters)
    {
        //���������ֵ�
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        //ָ���Ӳ�����
        dict[80] = SubCode;
        //��ֵ����
        for (int i = 0; i < parameters.Length; i++)
            dict[(byte)i] = parameters[i];
        //����
        peer.OpCustom(code, dict, true);
    }

}
