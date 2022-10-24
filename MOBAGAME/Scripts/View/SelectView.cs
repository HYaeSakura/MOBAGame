using MobaCommon.Code;
using MobaCommon.Config;
using MobaCommon.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : UIBase, IResourceListener
{
    [SerializeField]
    private UIPlayer[] team1;
    [SerializeField]
    private UIPlayer[] team2;

    private AudioClip acClick;
    private AudioClip acReady;

    #region UIBase

    public override void Init()
    {
        acClick = ResourcesManager.Instance.GetAsset(Paths.RES_SOUND_UI + "Click") as AudioClip;

        ResourcesManager.Instance.Load(Paths.RES_SOUND_UI + "Ready", typeof(AudioClip), this);
    }

    public void OnLoaded(string assetName, object asset)
    {
        if (assetName == Paths.RES_SOUND_UI + "Ready")
            acReady = asset as AudioClip;
    }

    public override void OnShow()
    {
        //��Ҫ����base����ķ���
        base.OnShow();

        //������������
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Enter);
        //��ʼ�����ӵ��Ӣ�۵��б�
        this.InitSelectHeroPanel(GameData.Player.heroIds);
        //��������
        txtContent.text = string.Empty;
    }

    public override void OnDestroy()
    {

    }

    public override string UIName()
    {
        return UIDefinit.UISelect;
    }

    #endregion

    /// <summary>
    /// ������ͼ��ʾ
    /// </summary>
    /// <param name="myTeam">�������</param>
    /// <param name="team1"></param>
    /// <param name="team2"></param>
    public void UpdateView(int myTeam, SelectModel[] team1, SelectModel[] team2)
    {
        List<int> selectedHero = new List<int>();
        if (myTeam == 1)
        {
            for (int i = 0; i < team1.Length; i++)
                this.team1[i].UpdateView(team1[i]);
            for (int i = 0; i < team2.Length; i++)
                this.team2[i].UpdateView(team2[i]);
            //���� һ�������Ѿ�ѡ���Ӣ��ͷ��ĵ������
            foreach (SelectModel item in team1)
            {
                if (item.heroId != -1)
                    selectedHero.Add(item.heroId);
            }
        }
        else if (myTeam == 2)
        {
            for (int i = 0; i < team2.Length; i++)
                this.team1[i].UpdateView(team2[i]);
            for (int i = 0; i < team1.Length; i++)
                this.team2[i].UpdateView(team1[i]);
            //���� һ�������Ѿ�ѡ���Ӣ��ͷ��ĵ������
            foreach (SelectModel item in team2)
            {
                if (item.heroId != -1)
                    selectedHero.Add(item.heroId);
            }
        }
        //����Ӣ��
        foreach (UIHero item in idHeroDict.Values)
        {
            //�����ǰ���Ӣ���Ѿ���ѡ���� ���� �����׼��
            if (selectedHero.Contains(item.Id) || btnReady.interactable == false)
                item.Interactable = false;
            else
                item.Interactable = true;
        }
    }

    [Header("Ӣ��ѡ��Ԥ��")]
    [SerializeField]
    private GameObject UIHero;
    [SerializeField]
    private Transform heroParent;

    /// <summary>
    /// �Ѿ����ص�Ӣ��ѡ��ͷ��
    /// </summary>
    private Dictionary<int, UIHero> idHeroDict = new Dictionary<int, UIHero>();

    /// <summary>
    /// ��ʼ��ѡ��Ӣ�����
    /// </summary>
    public void InitSelectHeroPanel(int[] heroIds)
    {
        GameObject go;
        foreach (int id in heroIds)
        {
            if (idHeroDict.ContainsKey(id))
                continue;

            go = Instantiate(UIHero);
            UIHero hero = go.GetComponent<UIHero>();
            hero.InitView(HeroData.GetHeroData(id));
            go.transform.SetParent(heroParent);
            go.transform.localScale = Vector3.one;
            idHeroDict.Add(id, hero);
        }
    }


    [SerializeField]
    private Button btnReady;

    /// <summary>
    /// ȷ��ѡ��
    /// </summary>
    public void ReadySelect()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(acReady);
        //���ð�ť
        btnReady.interactable = false;
        //����������������
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Ready);
    }

    [Header("����ģ��")]
    [SerializeField]
    private Text txtContent;
    [SerializeField]
    private InputField inTalk;
    [SerializeField]
    private Scrollbar bar;

    /// <summary>
    /// ���Ͱ�ť����¼�
    /// </summary>
    public void OnSendClick()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(acClick);
        string text = inTalk.text;
        if (string.IsNullOrEmpty(text))
            return;
        //������������ ��������
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Talk, text);
        //����ϴ�����
        inTalk.text = string.Empty;
    }

    /// <summary>
    /// ׷�������¼
    /// </summary>
    public void TalkAppend(string text)
    {
        //���һ�м�¼
        txtContent.text += "\n" + text;
        //ÿ�����춼��ʾ���һ��
        bar.value = 0;
    }
}
