using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Դ�ص������ӿ�
/// </summary>
public interface IResourceListener
{
    /// <summary>
    /// �������ʱ��ĵ���
    /// </summary>
    /// <param name="asset">��Դ</param>
    void OnLoaded(string assetName, object asset);
}
