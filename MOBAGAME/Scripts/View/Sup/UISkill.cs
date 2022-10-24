using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using MobaCommon.Code;

public class UISkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IResourceListener
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public SkillModel Skill;

    /// <summary>
    /// ��ʼ������
    /// </summary>
    /// <param name="skill"></param>
    public void Init(SkillModel skill)
    {
        //��������
        this.Skill = skill;
        //����ͼƬ
        ResourcesManager.Instance.Load(Paths.RES_SKILL + skill.Id, typeof(Sprite), this);
        //��ʾ���ֲ�
        imgMask.gameObject.SetActive(true);
    }

    public void OnLoaded(string assetName, object asset)
    {
        imgMask.sprite = asset as Sprite;
        imgSkill.sprite = asset as Sprite;
    }

    #region �ֶ�

    /// <summary>
    /// ����ͼ��
    /// </summary>
    [SerializeField]
    private Image imgSkill;
    /// <summary>
    /// ������ʾ
    /// </summary>
    [SerializeField]
    private Image imgMask;
    /// <summary>
    /// ������ť
    /// </summary>
    [SerializeField]
    private Button btnUp;
    /// <summary>
    /// ����������ť�Ƿ�ɵ�
    /// </summary>
    public bool UpInteractable { set { btnUp.interactable = value; } }
    /// <summary>
    /// �����Ƿ����
    /// </summary>
    private bool canUse;
    /// <summary>
    /// �����Ƿ����
    /// </summary>
    public bool CanUse { get { return canUse; } }
    /// <summary>
    /// ��ȴʱ��
    /// </summary>
    private float cdTime;
    /// <summary>
    /// ��ǰʱ��
    /// </summary>
    private float curTime;

    #endregion

    void Update()
    {
        //�����ܲ����õ�ʱ�� ����������ȴ
        if (!canUse)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                //�ָ�����
                canUse = true;
                cdTime = 0f;
                curTime = 0f;
                imgMask.gameObject.SetActive(false);
            }
            //��fillAmount��ֵ ���� �Ƕ� ��0-1��
            imgMask.fillAmount = curTime / cdTime;
        }
    }

    /// <summary>
    /// ʹ�ü���
    /// </summary>
    /// <param name="cd"></param>
    public void Use(int cd)
    {
        if (!canUse)
            return;

        cdTime = cd;
        curTime = cd;
        //��ʾ���ֲ�
        imgMask.gameObject.SetActive(true);
        //���ò�����
        canUse = false;
    }

    /// <summary>
    /// �������ֲ�
    /// </summary>
    public void Reset()
    {
        if (!canUse)
            return;

        if (Skill.Level > 0)
            imgMask.gameObject.SetActive(false);
    }

    /// <summary>
    /// ������뿪��ʱ�򴥷�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //�رռ��ܵ���ʾ��Ϣ
        //Skill.Description
    }


    /// <summary>
    /// ���������ʱ�򴥷�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //��ʾ���ܵ���ʾ��Ϣ
        //Skill.Description
    }

    /// <summary>
    /// ��������ť���ʱ����
    /// </summary>
    public void OnUpClick()
    {
        //ֱ�������������һ������������ ����:���ܵ�ID
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.SkillUp, this.Skill.Id);
    }
}