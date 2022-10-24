using MobaCommon.Code;
using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyControl : MonoBehaviour
{    
    [SerializeField]
    private KeyCode Skill_Q = KeyCode.Q;
    [SerializeField]
    private KeyCode Skill_W = KeyCode.W;
    [SerializeField]
    private KeyCode Skill_E = KeyCode.E;
    [SerializeField]
    private KeyCode Skill_R = KeyCode.R;

    [SerializeField]
    private UISkill uiSkill_Q;
    [SerializeField]
    private UISkill uiSkill_W;
    [SerializeField]
    private UISkill uiSkill_E;
    [SerializeField]
    private UISkill uiSkill_R;

    void Update()
    {
        #region ������Ҽ����

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mouse = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit[] his = Physics.RaycastAll(ray);
            for (int i = his.Length - 1; i >= 0; i--)
            {
                RaycastHit hit = his[i];
                //����㵽�˵з���λ �Ǿ͹���
                if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
                {
                    attack(hit.collider.gameObject);
                    //����������˵ط����Ͳ����¼����ж��� ��Ϊ����Ҫ������
                    break;
                }
                //����㵽�˵��� �Ǿ��ƶ�
                else if (hit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
                {
                    move(hit.point);
                }
            }
        }

        #endregion

        #region �ո�

        if (Input.GetKey(KeyCode.Space))
        {
            //���ൽ�Լ���Ӣ��
            Camera.main.GetComponent<CameraControl>().FocusOn();
        }

        #endregion

        #region �����ͷ�
        if (Input.GetKeyDown(Skill_Q) && uiSkill_Q.CanUse)
        {
            Vector2 mouse = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //�ͷż���
                skill(1, hit.point);
            }
        }

        if (Input.GetKeyDown(Skill_W) && uiSkill_W.CanUse)
        {
            Vector2 mouse = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //�ͷż���
                skill(2, hit.point);
            }
        }

        if (Input.GetKeyDown(Skill_E)&&uiSkill_E.CanUse)
        {
            Vector2 mouse = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //�ͷż���
                skill(3, hit.point);
            }
        }
        if (Input.GetKeyDown(Skill_R) && uiSkill_R.CanUse)
        {
            Vector2 mouse = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //�ͷż���
                skill(4, hit.point);
            }
        }

        #endregion
    }


    /// <summary>
    /// ����
    /// </summary>
    /// <param name="target">Ŀ��</param>
    private void attack(GameObject target)
    {
        //��ȡĿ���Id
        int targetId = target.GetComponent<BaseControl>().Model.Id;
        // int myId = GameData.MyControl.Model.Id;
        //��������������� ������1.���ܵ�ID��2��������id  3��Ŀ���ID
        int attackId = GameData.MyControl.Model.Id;
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Skill, 1, attackId, targetId, -1f, -1f, -1f);
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    /// <param name="point"></param>
    private void move(Vector3 point)
    {
        //��ʾһ����Ч
        GameObject go = PoolManager.Instance.GetObject("ClickMove");
        go.transform.position = point + Vector3.up;
        //����������һ������ �������������
        PhotonManager.Instance.Request
            (OpCode.FightCode, OpFight.Walk, point.x, point.y, point.z);
    }

    /// <summary>
    /// �ͷż���
    /// </summary>
    /// <param name="index"></param>
    /// <param name="targetPos"></param>
    private void skill(int index, Vector3 targetPos)
    {
        HeroModel myHero = (HeroModel)GameData.MyControl.Model;
        int skillId = myHero.Skills[index - 1].Id;
        int attackId = myHero.Id;
        //��������������� ������1.���ܵ�ID��2��������id  3��Ŀ���ID  4.Ŀ��������
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Skill,
            skillId,
            attackId,
            -1,
            targetPos.x, targetPos.y, targetPos.z);
    }
}
