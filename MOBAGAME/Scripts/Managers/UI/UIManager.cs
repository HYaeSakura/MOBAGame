using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// UI������
/// </summary>
public class UIManager : Singleton<UIManager>, IResourceListener
{
    /// <summary>
    /// UI���ֺ�����ӳ���ϵ
    /// </summary>
    private Dictionary<string, UIBase> nameUIDict = new Dictionary<string, UIBase>();

    /// <summary>
    /// ���UI
    /// </summary>
    /// <param name="ui"></param>
    public void AddUI(UIBase ui)
    {
        if (ui == null)
            return;

        nameUIDict.Add(ui.UIName(), ui);
    }

    /// <summary>
    /// ɾ��UI
    /// </summary>
    /// <param name="ui"></param>
    public void RemoveUI(UIBase ui)
    {
        if (ui == null)
            return;
        if (!nameUIDict.ContainsValue(ui))
            return;

        nameUIDict.Remove(ui.UIName());
    }

    /// <summary>
    /// ��ʾUI û�оʹ���һ��
    /// </summary>
    public void ShowUIPanel(string uiName)
    {
        if (nameUIDict.ContainsKey(uiName))
        {
            UIBase ui = nameUIDict[uiName];
            ui.OnShow();
            return;
        }
        ResourcesManager.Instance.Load(uiName, typeof(GameObject), this);
    }

    public void OnLoaded(string assetName, object asset)
    {
        GameObject uiPrefab = Instantiate(asset as GameObject);
        UIBase ui = uiPrefab.GetComponent<UIBase>();
        ui.OnShow();
        AddUI(ui);
    }

    /// <summary>
    /// �ر�UI
    /// </summary>
    /// <param name="uiName"></param>
    public void HideUIPanel(string uiName)
    {
        if (!nameUIDict.ContainsKey(uiName))
            return;

        UIBase ui = nameUIDict[uiName];
        ui.OnHide();
    }

}

