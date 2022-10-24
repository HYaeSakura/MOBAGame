using MobaCommon.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

/// <summary>
/// ����ս��ģ�͵Ŀ���������
/// </summary>
public class BaseControl : MonoBehaviour
{
    /// <summary>
    /// ����������ģ��
    /// </summary>
    public DogModel Model { get; set; }

    /// <summary>
    /// Ŀ��
    /// </summary>
    [SerializeField]
    protected BaseControl target = null;

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="model">����</param>
    /// <param name="friend">�Ƿ��ѷ���λ</param>
    public void Init(DogModel model, bool friend)
    {
        //��������
        this.Model = model;
        //����Ѫ����ɫ
        hpControl.SetColor(friend);
        //���� �������������� ��㼶
        string layer = friend ? "Friend" : "Enemy";
        gameObject.layer = LayerMask.NameToLayer(layer);
    }

    #region ����

    [SerializeField]
    protected AnimControl animControl;

    /// <summary>
    /// ��ǰ�Ķ���״̬
    /// </summary>
    protected AnimState state = AnimState.FREE;

    #endregion

    #region Ѫ��

    [SerializeField]
    protected HpControl hpControl;

    /// <summary>
    /// Ѫ���ı�
    /// </summary>
    public void OnHpChange()
    {
        hpControl.SetHp((float)Model.CurrHp / Model.MaxHp);
    }

    #endregion

    #region �ƶ�����

    [SerializeField]
    protected NavMeshAgent agent;

    /// <summary>
    /// �Ƿ������ƶ�
    /// </summary>
    protected bool IsMoving
    {
        get { return agent.pathPending || agent.remainingDistance > agent.stoppingDistance || agent.velocity != Vector3.zero || agent.pathStatus != NavMeshPathStatus.PathComplete; }
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    /// <param name="point">Ŀ���</param>
    public void Move(Vector3 point)
    {
        if (state == AnimState.DEATH)
            return;

        point.y = transform.position.y;
        //Ѱ·
        agent.ResetPath();
        agent.SetDestination(point);
        //���Ŷ���
        animControl.Walk();
        state = AnimState.WALK;
    }

    protected virtual void Update()
    {
        //���Ѱ·�Ƿ���ֹ
        if (state == AnimState.WALK && !IsMoving)
        {
            animControl.Free();
            state = AnimState.FREE;
        }
    }

    #endregion

    #region ��������

    //������ ѡ������ ֱ�ӹ��� �����˺�
    //���Σ� ѡ������ ������������һ��Ҫ������ID����ͬ������������
    //              �������ŵ��ؼ�֡ʱ���ڷ����������˺���Ȼ��ͬ���˺���ÿһ���ͻ���

    /// <summary>
    /// ���󹥻�
    /// </summary>
    public virtual void RequestAttack() { }

    /// <summary>
    /// ������Ӧ
    /// </summary>
    public virtual void AttackResponse(params Transform[] target) { }

    /// <summary>
    /// ������Ӧ
    /// </summary>
    public virtual void SkillResponse(int skillId, Transform target, Vector3 targetPos) { }

    #endregion

    #region ��Ч

    [SerializeField]
    protected AudioSource audioSource;

    /// <summary>
    /// ��Ч���ƺ���Ч�ļ���ӳ���ϵ
    /// </summary>
    protected Dictionary<string, AudioClip> nameClipDict = new Dictionary<string, AudioClip>();

    protected virtual void Start()
    {
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    /// <summary>
    /// ������Ч ����״̬
    /// </summary>
    protected void PlayAudio(string name)
    {
        audioSource.clip = nameClipDict[name];
        audioSource.Play();
    }

    #endregion

    /// <summary>
    /// ����
    /// </summary>
    public virtual void DeathResponse() { }

    /// <summary>
    /// ����
    /// </summary>
    public virtual void ResurgeResponse() { }
}
