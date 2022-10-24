using MobaCommon.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using MobaCommon.Code;
using MobaCommon.Dto;

public class UIHero : MonoBehaviour, IResourceListener
{
    [SerializeField]
    private Image imgHead;
    [SerializeField]
    private Button btnHead;

    private int heroId;
    public int Id { get { return heroId; } }
    private string heroName;

    private AudioClip ac;
    private AudioClip acSelect;

    /// <summary>
    /// ��ʼ����ͼ
    /// </summary>
    public void InitView(HeroDataModel hero)
    {
        //����ID
        this.heroId = hero.TypeId;
        this.heroName = hero.Name;
        //������Ч�ļ�
        ResourcesManager.Instance.Load(Paths.RES_SOUND_SELECT + hero.Name, typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_UI + "Select", typeof(AudioClip), this);
        //����ͼƬ
        string assetName = Paths.RES_HEAD + HeroData.GetHeroData(hero.TypeId).Name;
        ResourcesManager.Instance.Load
            (assetName, typeof(Sprite), this);
    }

    public void OnLoaded(string assetName, object asset)
    {
        if (assetName == Paths.RES_SOUND_SELECT + heroName)
            this.ac = asset as AudioClip;
        else if (Paths.RES_HEAD + heroName == assetName)
            this.imgHead.sprite = asset as Sprite;
        else if (assetName == Paths.RES_SOUND_UI + "Select")
            acSelect = asset as AudioClip;
    }

    /// <summary>
    /// Ӣ���Ƿ��ѡ��
    /// </summary>
    public bool Interactable
    {
        get { return btnHead.interactable; }
        set { btnHead.interactable = value; }
    }

    /// <summary>
    /// ѡ��Ӣ���¼�
    /// </summary>
    public void OnClick()
    {
        //��������
        //SoundManager.Instance.PlayEffectMusic(acSelect);
        SoundManager.Instance.PlayEffectMusic(ac);
        //����ѡ�˵�����
        PhotonManager.Instance.Request(OpCode.SelectCode, OpSelect.Select, heroId);
    }
}

