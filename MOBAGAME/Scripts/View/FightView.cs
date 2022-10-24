using MobaCommon.Code;
using MobaCommon.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightView : MonoBehaviour, IResourceListener
{
    #region �ֶ�

    /// <summary>
    /// ͷ��
    /// </summary>
    [SerializeField]
    private Image imgHead;
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    private Slider barExp;
    /// <summary>
    /// �ȼ�
    /// </summary>
    [SerializeField]
    private Text txtLv;
    /// <summary>
    /// Ѫ��
    /// </summary>
    [SerializeField]
    private Slider barHp;
    /// <summary>
    /// Ѫ��
    /// </summary>
    [SerializeField]
    private Text txtHp;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    private Slider barMp;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    private Text txtMp;
    /// <summary>
    /// ͳ�����
    /// </summary>
    [SerializeField]
    private Text txtKDA;
    /// <summary>
    /// Ǯ
    /// </summary>
    [SerializeField]
    private Text txtMoney;
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    private Text txtAttack;
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    private Text txtDefense;
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    private Text txtSp;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    private UISkill[] skills;

    #endregion

    void Start()
    {
        //�ͷŲ���Ҫ����Դ
        ResourcesManager.Instance.ReleaseAll();
        //����ս��������������
        ResourcesManager.Instance.Load(Paths.RES_SOUND_FIGHT + "FightBGM", typeof(AudioClip), this);
        //�������������������
        PhotonManager.Instance.Request(OpCode.FightCode, OpFight.Enter, GameData.Player.id);
    }

    #region ������ͼ

    /// <summary>
    /// ��ʼ����ͼ����ʾ
    /// </summary>
    public void InitView(HeroModel hero)
    {
        //ͷ��
        ResourcesManager.Instance.Load(Paths.RES_HEAD + hero.Name, typeof(Sprite), this);
        //Ѫ
        barHp.value = (float)hero.CurrHp / hero.MaxHp;
        txtHp.text = string.Format("{0} / {1}", hero.CurrHp, hero.MaxHp);
        //��
        barMp.value = (float)hero.CurrMp / hero.MaxMp;
        txtMp.text = string.Format("{0} / {1}", hero.CurrMp, hero.MaxMp);
        //����
        barExp.value = (float)hero.Exp / (hero.Level * 100);
        txtLv.text = "Lv." + hero.Level.ToString();
        //ͳ��
        txtKDA.text = string.Format("Kill��{0}      Dead��{1} ", hero.Kill, hero.Dead);
        //Ǯ
        txtMoney.text = hero.Money.ToString();
        //����
        txtAttack.text = hero.Attack.ToString();
        txtDefense.text = hero.Defense.ToString();
        txtSp.text=hero.Sp.ToString();
        //���¼����б�
        for (int i = 0; i < hero.Skills.Length; i++)
            skills[i].Init(hero.Skills[i]);
    }

    /// <summary>
    /// ������ʾ
    /// </summary>
    public void UpdateView(HeroModel hero)
    {
        //Ѫ
        barHp.value = (float)hero.CurrHp / hero.MaxHp;
        txtHp.text = string.Format("{0} / {1}", hero.CurrHp, hero.MaxHp);
        //��
        barMp.value = (float)hero.CurrMp / hero.MaxMp;
        txtMp.text = string.Format("{0} / {1}", hero.CurrMp, hero.MaxMp);
        //����
        barExp.value = (float)hero.Exp / (hero.Level * 100);
        txtLv.text = "Lv." + hero.Level.ToString();
        //ͳ��
        txtKDA.text = string.Format("Kill��{0}      Dead��{1} ", hero.Kill, hero.Dead);
        //Ǯ
        txtMoney.text = hero.Money.ToString();
        //����
        txtAttack.text = hero.Attack.ToString();
        txtDefense.text = hero.Defense.ToString();
        txtSp.text = hero.Sp.ToString();
    }

    /// <summary>
    /// ���¼���
    /// </summary>
    public void UpdateSkills(HeroModel hero)
    {
        for(int i = 0; i < hero.Skills.Length; i++)
        {
            SkillModel skill = hero.Skills[i];
            //�����heromodel�ﵽ��һ�����ܿ��Լӵ�ĵȼ� ���Ǿ������������ť��ʾ
            if (hero.Level > skill.LearnLevel)
            {
                //Ҫ��ʾ������ť
                if (hero.Points > 0)
                    skills[i].UpInteractable = true;
                else
                    skills[i].UpInteractable = false;
            }
            else
                skills[i].UpInteractable = false;
            //����������ܵ���Ϣ
            skills[i].Skill = skill;
            //�������� ���ü���
            skills[i].Reset();
        }
    }

    /// <summary>
    /// ������ȴ
    /// </summary>
    public void UpdateCoolDown(int skillId)
    {
        foreach(var item in skills)
        {
            if (item.Skill.Id == skillId)
            {
                item.Use(item.Skill.CoolDown);
                break;
            }
        }
    }

    #endregion

    public void OnLoaded(string assetName, object asset)
    {
        if (asset is AudioClip)
        {
            SoundManager.Instance.PlayBgMusic(asset as AudioClip);
            SoundManager.Instance.BGVolume = 1f;
        }
        else if (asset is Sprite)
        {
            imgHead.sprite = asset as Sprite;
        }
    }

    #region �̵�

    [SerializeField]
    private Image ItemPanel;

    public void OnItemPanelClick()
    {
        bool active = ItemPanel.gameObject.activeSelf;
        ItemPanel.gameObject.SetActive(!active);
    }

    #endregion

    #region ����

    [SerializeField]
    private Image WinPanel;
    [SerializeField]
    private Image LosePanel;

    /// <summary>
    /// ��Ϸ����
    /// </summary>
    /// <param name="isWin"></param>
    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            WinPanel.gameObject.SetActive(true);
        }
        else
        {
            LosePanel.gameObject.SetActive(true);
        }
    }

    public void onExitClick()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }
    #endregion
}
