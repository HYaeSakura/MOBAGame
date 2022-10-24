using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Դ������
/// </summary>
public class ResourcesManager : Singleton<ResourcesManager>
{
    /// <summary>
    /// �Ѿ����ص���Դ�ֵ�
    /// </summary>
    private Dictionary<string, object> nameAssetDict = new Dictionary<string, object>();

    /// <summary>
    /// ���ڼ��ص��б�
    /// </summary>
    private List<LoadAsset> loadingList = new List<LoadAsset>();

    /// <summary>
    /// �ȴ����ص��б�
    /// </summary>
    private Queue<LoadAsset> waitingQue = new Queue<LoadAsset>();

    void Update()
    {
        if (loadingList.Count > 0)
        {
            for (int i = 0; i < loadingList.Count; i++)
            {
                if (loadingList[i].IsDone)
                {
                    LoadAsset asset = loadingList[i];
                    for (int j = 0; j < asset.Listeners.Count; j++)
                    {
                        asset.Listeners[j].OnLoaded(asset.AssetName, asset.GetAsset);
                    }
                    nameAssetDict.Add(asset.AssetName, asset.GetAsset);
                    loadingList.RemoveAt(i);
                }
            }
        }

        while (waitingQue.Count > 0 && loadingList.Count < 5)
        {
            LoadAsset asset = waitingQue.Dequeue();
            loadingList.Add(asset);
            asset.LoadAsync();
        }
    }

    /// <summary>
    /// ������Դ
    /// </summary>
    /// <param name="assetName">��Դ��</param>
    /// <param name="assetType">��Դ����</param>
    /// <param name="listener">�ص�</param>
    public void Load(string assetName, Type assetType, IResourceListener listener)
    {
        //����Ѿ����� ��ֱ�ӷ���
        if (nameAssetDict.ContainsKey(assetName))
        {
            listener.OnLoaded(assetName, nameAssetDict[assetName]);
            return;
        }
        else //û�� ��ʼ�첽����
        {
            LoadAsync(assetName, assetType, listener);
        }
    }

    /// <summary>
    /// �첽����
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="assetType"></param>
    /// <param name="listener"></param>
    private void LoadAsync(string assetName, Type assetType, IResourceListener listener)
    {
        //���ڱ����� ��û�������
        foreach (LoadAsset item in loadingList)
        {
            if (item.AssetName == assetName)
            {
                item.AddListener(listener);
                return;
            }
        }
        //�ȴ��Ķ���������
        foreach (LoadAsset item in waitingQue)
        {
            if (item.AssetName == assetName)
            {
                item.AddListener(listener);
                return;
            }
        }
        //��û�� �ȴ���
        LoadAsset asset = new LoadAsset();
        asset.AssetName = assetName;
        asset.AssetType = assetType;
        asset.AddListener(listener);

        //��ӵ��ȴ�����
        waitingQue.Enqueue(asset);
    }

    /// <summary>
    /// ��ȡ��Դ
    /// </summary>
    /// <param name="assetName"></param>
    public object GetAsset(string assetName)
    {
        object asset = null;
        nameAssetDict.TryGetValue(assetName, out asset);
        return asset;
    }

    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    /// <param name="assetName"></param>
    public void ReleaseAsset(string assetName)
    {
        if (nameAssetDict.ContainsKey(assetName))
        {
            nameAssetDict[assetName] = null;
            nameAssetDict.Remove(assetName);
        }
    }

    /// <summary>
    /// ǿ���ͷ�
    /// </summary>
    public void ReleaseAll()
    {
        this.nameAssetDict.Clear();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

}
