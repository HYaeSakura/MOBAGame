using MobaCommon.Code;
using MobaCommon.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : UIBase, IResourceListener
{
    [HideInInspector]
    public AudioClip acCountDown;
    private AudioClip acClick;

    #region UIBASE

    public override void Init()
    {
        acClick = ResourcesManager.Instance.GetAsset(Paths.GetSoundFullName("Click")) as AudioClip;
        ResourcesManager.Instance.Load(Paths.RES_SOUND_UI + "CountDown", typeof(AudioClip), this);

        //��Ӽ���
        btnCreate.onClick.AddListener(OnCreateClick);

        //���������ȡ��ɫ��Ϣ
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.GetInfo);
    }

    public void OnLoaded(string assetName, object asset)
    {
        if (assetName == Paths.RES_SOUND_UI + "CountDown")
            acCountDown = asset as AudioClip;
    }

    public override void OnDestroy()
    {

    }

    public override string UIName()
    {
        return UIDefinit.UIMain;
    }

    #endregion

    #region ����ģ��
    [Header("����ģ��")]
    [SerializeField]
    private InputField inName;
    [SerializeField]
    private Button btnCreate;

    /// <summary>
    /// ������ť�Ŀɵ�״̬
    /// </summary>
    public bool CreateInteractable
    {
        set { btnCreate.interactable = value; }
    }

    [SerializeField]
    private GameObject createPanel;

    /// <summary>
    /// �������ɼ�
    /// </summary>
    public bool CreatePanelActive
    {
        set { createPanel.SetActive(value); }
    }

    public void OnCreateClick()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(acClick);

        //������
        if (string.IsNullOrEmpty(inName.text))
            return;

        //���𴴽�����
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.Create, inName.text);
        //���ð�ť
        CreateInteractable = false;
    }

    #endregion


    [Header("��ɫ��Ϣ")]
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Slider barExp;

    /// <summary>
    /// ������ʾ
    /// </summary>
    public void UpdateView(PlayerDto player)
    {
        txtName.text = player.name;
        barExp.value = (float)player.exp / (player.lv * 100);
        //���غ����б�
        Frient[] friends = player.friends;
        friendList.Clear();
        GameObject go = null;
        foreach (Frient item in friends)
        {
            go = Instantiate(UIFriend);
            go.transform.SetParent(friendTran);
            FriendView fv = go.GetComponent<FriendView>();
            fv.InitView(item.Id, item.Name, item.IsOnline);
            friendList.Add(fv);
        }
    }

    #region ����ģ��

    [SerializeField]
    private Transform friendTran;

    private List<FriendView> friendList = new List<FriendView>();

    [Header("������Ϣ��Ԥ��")]
    [SerializeField]
    private GameObject UIFriend;

    [SerializeField]
    private InputField inAddName;

    /// <summary>
    /// �����Ӻ��Ѱ�ť
    /// </summary>
    public void OnAddClick()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(acClick);
        //������
        if (string.IsNullOrEmpty(inAddName.text))
            return;
        //������Ӻ�������
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.RequestAdd, inAddName.text);
    }

    [SerializeField]
    private ToClientAddView toClientAddPanel;

    /// <summary>
    /// ��ʾ�Ӻ������
    /// </summary>
    public void ShowToClientAddPanel(PlayerDto dto)
    {
        toClientAddPanel.gameObject.SetActive(true);
        toClientAddPanel.UpdateView(dto);
    }

    /// <summary>
    /// ��ӽ������¼�
    /// </summary>
    public void OnResClick(bool result)
    {
        int id = toClientAddPanel.Id;
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.ToClientAdd, result, id);
        toClientAddPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���º��ѽ���
    /// </summary>
    /// <param name="friendId"></param>
    /// <param name="isOnline"></param>
    public void UpdateFriendView(int friendId, bool isOnline)
    {
        foreach (FriendView item in friendList)
        {
            if (item.Id == friendId)
                item.UpdateView(isOnline);
        }
    }

    #endregion


    #region ƥ��ģ��

    [Header("ƥ��ģ��")]
    [SerializeField]
    private Button btnOne;

    [SerializeField]
    private Button btnTwo;

    [SerializeField]
    private MatchView matchView;

    /// <summary>
    /// ����ƥ��ƥ��
    /// </summary>
    public void OnOneMatch()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(acClick);
        //��������
        int id = GameData.Player.id;
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.StartMatch, id);
        //��ʾƥ�����
        matchView.StartMatch();
        //���ð�ť
        btnOne.interactable = false;
        btnTwo.interactable = false;
    }

    /// <summary>
    /// ֹͣƥ��
    /// </summary>
    public void OnStopMatch()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(acClick);
        //��������
        int id = GameData.Player.id;
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.StopMatch, id);
        matchView.StopMatch();
        btnOne.interactable = true;
        btnTwo.interactable = true;
    }

    /// <summary>
    /// ����ƥ��
    /// </summary>
    /// <param name="ids"></param>
    public void OnTwoMatch(int[] ids)
    {
        PhotonManager.Instance.Request(OpCode.PlayerCode, OpPlayer.StartMatch, ids);
        //TODO
    }

    #endregion

}
