using MobaCommon.Config;
using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPlayer : MonoBehaviour, IResourceListener
{
    private Text txtName;
    private Image imgBg;
    private Text txtState;
    private Image imgHead;

    void Start()
    {
        txtName = transform.Find("txtName").GetComponent<Text>();
        imgBg = GetComponent<Image>();
        txtState = transform.Find("Text").GetComponent<Text>();
        imgHead = transform.Find("imgHead").GetComponent<Image>();
    }


    public void UpdateView(SelectModel model)
    {
        txtName.text = model.playerName;
        imgBg.color = Color.white;
        //判断玩家时候进入了
        if (!model.isEnter)
        {
            ResourcesManager.Instance.Load
                (Paths.RES_HEAD + "no-Connect", typeof(Sprite), this);
            return;
        }
        else //进入之后
        {
            ResourcesManager.Instance.Load
                (Paths.RES_HEAD + "no-Select", typeof(Sprite), this);
        }
        //进入之后
        if (model.heroId != -1)
        {
            string assetName = Paths.RES_HEAD + HeroData.GetHeroData(model.heroId).Name;
            ResourcesManager.Instance.Load
                (assetName, typeof(Sprite), this);
        }
        else
        {
            ResourcesManager.Instance.Load
                (Paths.RES_HEAD + "no-Select", typeof(Sprite), this);
        }
        //判断是否准备
        if (model.isReady)
        {
            imgBg.color = Color.green;
            txtState.text = "已选择";
        }
        else
        {
            imgBg.color = Color.white;
            txtState.text = "正在选择..";
        }
    }

    public void OnLoaded(string assetName, object asset)
    {
        Sprite s = asset as Sprite;
        imgHead.sprite = s;
    }
}
