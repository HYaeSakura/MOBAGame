using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ϸ��ʼ���ű�
/// </summary>
public class GameInit : MonoBehaviour
{
    void Start()
    {
        //���ص�¼UI
        UIManager.Instance.ShowUIPanel(UIDefinit.UIAccount);

    }
}
