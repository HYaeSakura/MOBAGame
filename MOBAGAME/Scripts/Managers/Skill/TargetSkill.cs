using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ѡ��Ŀ���͵ļ���
/// </summary>
public class TargetSkill : MonoBehaviour
{
    /// <summary>
    /// Ŀ��
    /// </summary>
    private Transform target;
    /// <summary>
    /// ����ID
    /// </summary>
    private int skillId;
    /// <summary>
    /// ������ID
    /// </summary>
    private int attackId;
    /// <summary>
    /// Ŀ��Id
    /// </summary>
    private int targetId;
    /// <summary>
    /// �Ƿ���Ҫ����
    /// </summary>
    private bool send;

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="target">Ŀ������</param>
    /// <param name="skillId">����Id</param>
    /// <param name="attackId">������Id</param>
    /// <param name="targetId">Ŀ��Id</param>
    /// <param name="send">�Ƿ���</param>
    public void Init(Transform target, int skillId, int attackId, int targetId, bool send)
    {
        this.target = target;
        this.skillId = skillId;
        this.attackId = attackId;
        this.targetId = targetId;
        this.send = send;
    }

    void Update()
    {
        //�����û��Ŀ��
        if (target == null)
            return;
        //��ֵ�ƶ���Ч��
        transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
        float d = Vector3.Distance(transform.position, target.position);
        if (d > 1f)
            return;
        if (send)
        {
            //��ֹ�ظ�����
            send = false;
            //�����˺�������
            PhotonManager.Instance.Request
                (OpCode.FightCode, OpFight.Damage, attackId, skillId, new int[] { targetId });
        }
        //�������� 
        PoolManager.Instance.HideObjet(gameObject);
    }
}
