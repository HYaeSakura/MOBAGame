using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTip : Singleton<MessageTip>
{
    /// <summary>
    /// ��ʾ����
    /// </summary>
    [SerializeField]
    private Text txtContent;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    private GameObject tip;

    /// <summary>
    /// ���֮��ĵ���
    /// </summary>
    private Action onCompleted;

    /// <summary>
    /// ��ʾ����
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text, Action action = null, float showTime = -1)
    {
        tip.SetActive(true);
        txtContent.text = text;
        onCompleted = action;
        if (showTime != -1)
        {
            Invoke("Hide", showTime);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Hide()
    {
        tip.SetActive(false);
    }

    /// <summary>
    /// ���ȷ����ť
    /// </summary>
    public void OnClick()
    {
        tip.SetActive(false);

        if (onCompleted != null)
        {
            onCompleted();
            onCompleted = null;
        }
    }
}