using MobaCommon.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSkill : MonoBehaviour
{
    /// <summary>
    /// ���ܵ�ʹ����
    /// </summary>
    private Transform user;
    /// <summary>
    /// ����
    /// </summary>
    private float distance;
    /// <summary>
    /// �Ѿ��ƶ��ľ���
    /// </summary>
    private float currDistance;
    /// <summary>
    /// �ٶ�
    /// </summary>
    private float speed;
    /// <summary>
    /// ����ID
    /// </summary>
    private int skillId;
    /// <summary>
    /// ������ID
    /// </summary>
    private int attackId;
    /// <summary>
    /// �Ƿ���Ҫ����
    /// </summary>
    private bool send;

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="user"></param>
    /// <param name="distance"></param>
    /// <param name="speed"></param>
    /// <param name="skillId"></param>
    /// <param name="attackId"></param>
    /// <param name="send"></param>
    public void Init(Transform user, float distance, float speed, int skillId, int attackId, bool send)
    {
        this.user = user;
        this.transform.position = user.position;
        this.transform.rotation = user.rotation;
        this.currDistance = 0;
        this.distance = distance;
        this.speed = speed;
        this.skillId = skillId;
        this.attackId = attackId;
        this.send = send;
    }

    private List<int> idList = new List<int>();
    void OnTriggerEnter(Collider other)
    {
        //�����˺���һ����
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            idList.Add(other.GetComponent<BaseControl>().Model.Id);
        }     
    }

    void Update()
    {
        if (user == null)
            return;

        Vector3 translation = Vector3.forward * speed * Time.deltaTime;
        transform.Translate(translation);
        currDistance += translation.z;
        //�ﵽ���� �����ص�
        if (currDistance >= distance)
        {
            PoolManager.Instance.HideObjet(gameObject);
            if (send)
            {
                send = false;
                //�����˺�
                PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Damage, attackId, skillId, idList.ToArray());
            }
        }
    }
}
