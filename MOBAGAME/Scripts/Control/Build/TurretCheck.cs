using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����ĵ��˼��
/// </summary>
public class TurretCheck : MonoBehaviour
{
    /// <summary>
    /// ��ʾ��ǰ�����Ķ���
    /// </summary>
    [SerializeField]
    private int team;

    public void SetTeam(int team)
    {
        this.team = team;
    }

    /// <summary>
    /// ��⵽�ĵ����б�
    /// </summary>
    public List<BaseControl> conList = new List<BaseControl>();

    void OnTriggerEnter(Collider other)
    {
        BaseControl con = other.GetComponent<BaseControl>();
        if (con != null && con.Model.Team != team)
        {
            conList.Add(con);
        }
    }

    void OnTriggerExit(Collider other)
    {
        BaseControl con = other.GetComponent<BaseControl>();
        if (con != null && conList.Contains(con))
        {
            conList.Remove(con);
        }
    }
}
