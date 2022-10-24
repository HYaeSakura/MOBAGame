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
            case Paths.RES_SOUND_UI + "孤勇者":
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
    /// 播放点击音效
    /// </summary>
    public void PlayClickAudio()
    {
        SoundManager.Instance.PlayEffectMusic(clickClip);
    }

    #region 注册模块
    [Header("注册模块")]
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
    /// 注册的点击事件
    /// </summary>
    public void OnRegisterClick()
    {
        //播放声音
        SoundManager.Instance.PlayEffectMusic(clickClip);

        //判断账号和密码是不是空
        if (string.IsNullOrEmpty(inAcc4Register.text) || string.IsNullOrEmpty(inPwd4Register.text)
            || !inPwd4Register.text.Equals(inPwd4Repeat.text))
        {
            //告诉客户 他非法输入了
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
    #region 登录模块
    [Header("登录模块")]
    [SerializeField]
    private InputField inAcc4Login;
    [SerializeField]
    private InputField inPwd4Login;
    [SerializeField]
    private Button btnEnter;

    /// <summary>
    /// 进入按钮是否可以点击
    /// </summary>
    public bool EnterInteractable
    {
        set { btnEnter.interactable = value; }
    }

    /// <summary>
    /// 进入游戏点击事件
    /// </summary>
    public void OnEnterClick()
    {
        //播放声音
        SoundManager.Instance.PlayEffectMusic(enterClip);

        if (string.IsNullOrEmpty(inAcc4Login.text) || string.IsNullOrEmpty(inPwd4Login.text))
        {
            // 提示
            return;
        }

        //创建传输模型
        AccountDto dto = new AccountDto()
        {
            Account = inAcc4Login.text,
            Password = inPwd4Login.text
        };
        //发送
        PhotonManager.Instance.Request(OpCode.AccountCode, OpAccount.Login, JsonMapper.ToJson(dto));

        //设置不可点击状态
        EnterInteractable = false;
    }

    /// <summary>
    /// 重置登录面板的输入
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
            (Paths.RES_SOUND_UI + "孤勇者", typeof(AudioClip), this);
        ResourcesManager.Instance.Load
            (Paths.RES_SOUND_UI + "Click", typeof(AudioClip), this);
        ResourcesManager.Instance.Load
            (Paths.RES_SOUND_UI + "EnterGame", typeof(AudioClip), this);
    }

    public override void OnShow()
    {
        //角色已经登录 就不用再次登录
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
