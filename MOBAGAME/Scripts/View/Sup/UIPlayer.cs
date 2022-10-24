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
        //�ж����ʱ�������
        if (!model.isEnter)
        {
            ResourcesManager.Instance.Load
                (Paths.RES_HEAD + "no-Connect", typeof(Sprite), this);
            return;
        }
        else //����֮��
        {
            ResourcesManager.Instance.Load
                (Paths.RES_HEAD + "no-Select", typeof(Sprite), this);
        }
        //����֮��
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
        //�ж��Ƿ�׼��
        if (model.isReady)
        {
            imgBg.color = Color.green;
            txtState.text = "��ѡ��";
        }
        else
        {
            imgBg.color = Color.white;
            txtState.text = "����ѡ��..";
        }
    }

    public void OnLoaded(string assetName, object asset)
    {
        Sprite s = asset as Sprite;
        imgHead.sprite = s;
    }
}
