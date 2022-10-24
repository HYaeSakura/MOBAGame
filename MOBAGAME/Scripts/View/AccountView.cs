using MobaCommon.Code;
using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;

public class AccountView : UIBase, IResourceListener
{
    AudioClip bgClip;
    AudioClip enterClip;
    AudioClip clickClip;

    public void OnLoaded(string assetName, object asset)
    {
        AudioClip clip = asset as AudioClip;
        switch (assetName)
        {
            case Paths.RES_SOUND_UI + "������":
                bgClip = clip;
                SoundManager.Instance.PlayBgMusic(bgClip);
                break;
            case Paths.RES_SOUND_UI + "EnterGame":
                enterClip = clip;
                break;
            case Paths.RES_SOUND_UI + "Click":
                clickClip = clip;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���ŵ����Ч
    /// </summary>
    public void PlayClickAudio()
    {
        SoundManager.Instance.PlayEffectMusic(clickClip);
    }

    #region ע��ģ��
    [Header("ע��ģ��")]
    [SerializeField]
    private InputField inAcc4Register;
    [SerializeField]
    private InputField inPwd4Register;
    [SerializeField]
    private InputField inPwd4Repeat;

    public void OnResetPanel()
    {
        inAcc4Register.text = string.Empty;
        inPwd4Register.text = string.Empty;
        inPwd4Repeat.text = string.Empty;
    }

    /// <summary>
    /// ע��ĵ���¼�
    /// </summary>
    public void OnRegisterClick()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(clickClip);

        //�ж��˺ź������ǲ��ǿ�
        if (string.IsNullOrEmpty(inAcc4Register.text) || string.IsNullOrEmpty(inPwd4Register.text)
            || !inPwd4Register.text.Equals(inPwd4Repeat.text))
        {
            //���߿ͻ� ���Ƿ�������
            return;
        }

        string account = inAcc4Register.text;
        string password = inPwd4Register.text;
        //1 account 2 password
        PhotonManager.Instance.Request
            (OpCode.AccountCode, OpAccount.Register, account, password);
    }

    #endregion

    //ctrl + k + s
    #region ��¼ģ��
    [Header("��¼ģ��")]
    [SerializeField]
    private InputField inAcc4Login;
    [SerializeField]
    private InputField inPwd4Login;
    [SerializeField]
    private Button btnEnter;

    /// <summary>
    /// ���밴ť�Ƿ���Ե��
    /// </summary>
    public bool EnterInteractable
    {
        set { btnEnter.interactable = value; }
    }

    /// <summary>
    /// ������Ϸ����¼�
    /// </summary>
    public void OnEnterClick()
    {
        //��������
        SoundManager.Instance.PlayEffectMusic(enterClip);

        if (string.IsNullOrEmpty(inAcc4Login.text) || string.IsNullOrEmpty(inPwd4Login.text))
        {
            // ��ʾ
            return;
        }

        //��������ģ��
        AccountDto dto = new AccountDto()
        {
            Account = inAcc4Login.text,
            Password = inPwd4Login.text
        };
        //����
        PhotonManager.Instance.Request(OpCode.AccountCode, OpAccount.Login, JsonMapper.ToJson(dto));

        //���ò��ɵ��״̬
        EnterInteractable = false;
    }

    /// <summary>
    /// ���õ�¼��������
    /// </summary>
    public void ResetLoginInput()
    {
        inAcc4Login.text = string.Empty;
        inPwd4Login.text = string.Empty;
    }


    #endregion


    #region UIBase

    public override string UIName()
    {
        return UIDefinit.UIAccount;
    }

    public override void Init()
    {
        ResourcesManager.Instance.Load
            (Paths.RES_SOUND_UI + "������", typeof(AudioClip), this);
        ResourcesManager.Instance.Load
            (Paths.RES_SOUND_UI + "Click", typeof(AudioClip), this);
        ResourcesManager.Instance.Load
            (Paths.RES_SOUND_UI + "EnterGame", typeof(AudioClip), this);
    }

    public override void OnShow()
    {
        //��ɫ�Ѿ���¼ �Ͳ����ٴε�¼
        if (GameData.Player != null)
        {
            UIManager.Instance.HideUIPanel(UIDefinit.UIAccount);
            UIManager.Instance.ShowUIPanel(UIDefinit.UIMain);
        }
    }

    public override void OnDestroy()
    {
        bgClip = null;
        enterClip = null;
        clickClip = null;
    }

    #endregion

}
