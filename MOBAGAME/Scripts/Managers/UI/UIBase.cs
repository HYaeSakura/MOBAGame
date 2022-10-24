using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI�Ļ���
/// </summary>
public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    public abstract string UIName();

    protected CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        Init();
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// ��ʾ
    /// </summary>
    public virtual void OnShow()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void OnHide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ����
    /// </summary>
    public abstract void OnDestroy();
}