using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using MobaCommon.Code;
using MobaCommon.Dto;
using LitJson;
using MobaCommon.Config;

public class FightReceiver : MonoBehaviour, IReceiver
{
    /// <summary>
    /// ���������Ӣ��
    /// </summary>
    HeroModel[] Heros;
    /// <summary>
    /// ��������н���
    /// </summary>
    BuildModel[] Builds;
    /// <summary>
    /// ��������С��
    /// </summary>
    DogModel[] Dogs;
    /// <summary>
    /// ��������Ұ��
    /// </summary>
    MonsterModel[] Monsters;

    [Header("����1")]
    [SerializeField]
    private Transform team1Parent;
    [SerializeField]
    private Transform[] team1HeroPoints;
    [SerializeField]
    private GameObject[] team1Builds;
    [SerializeField]
    private Transform[] team1DogPoints;

    [Header("����2")]
    [SerializeField]
    private Transform team2Parent;
    [SerializeField]
    private Transform[] team2HeroPoints;
    [SerializeField]
    private GameObject[] team2Builds;
    [SerializeField]
    private Transform[] team2DogPoints;

    [Header("Ұ��")]
    [SerializeField]
    private Transform[] Wolf;

    private Dictionary<int, BaseControl> idControlDict = new Dictionary<int, BaseControl>();

    [Header("��ͼ")]
    [SerializeField]
    private FightView view;

    [Header("��Ѫ����")]
    [SerializeField]
    private bl_HUDText HUDText;

    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch (subCode)
        {
            case OpFight.GetInfo:
                onGetInfo
                    (JsonMapper.ToObject<HeroModel[]>(response[0].ToString()),
                    JsonMapper.ToObject<BuildModel[]>(response[1].ToString()),
                    JsonMapper.ToObject<MonsterModel[]>(response[2].ToString()));
                break;
            case OpFight.Walk:
                onWalk((int)response[0],
                    (float)response[1],
                    (float)response[2],
                    (float)response[3]);
                break;
            case OpFight.Skill:
                if (response.ReturnCode == 0)
                    onAttack((int)response[0], (int)response[1]);
                else if (response.ReturnCode == 1)
                {
                    //�Ǽ���
                    onSkill((int)response[0], (int)response[1], (int)response[2], (float)response[3], (float)response[4], (float)response[5]);
                }
                break;
            case OpFight.Damage:
                onDamage(JsonMapper.ToObject<DamageModel[]>(response[0].ToString()));
                break;
            case OpFight.Buy:
                onBuy(JsonMapper.ToObject<HeroModel>(response[0].ToString()));
                break;
            case OpFight.SkillUp:
                onSkillUp((int)response[0], JsonMapper.ToObject<SkillModel>(response[1].ToString()));
                break;
            case OpFight.UpdataModel:
                onUpdataModel(JsonMapper.ToObject<HeroModel>(response[0].ToString()));
                break;
            case OpFight.Resurge:
                if (response.ReturnCode == 0)
                {
                    onResurge(JsonMapper.ToObject<HeroModel>(response[0].ToString()));
                }
                else if (response.ReturnCode == 1)
                    onResurge(JsonMapper.ToObject<BuildModel>(response[0].ToString()));
                else if (response.ReturnCode == 2)
                    onResurge(JsonMapper.ToObject<MonsterModel>(response[0].ToString()));
                break;
            case OpFight.Dog:
                onGetDog(JsonMapper.ToObject<HeroModel[]>(response[0].ToString()),
                    JsonMapper.ToObject<DogModel[]>(response[1].ToString()));
                break;
            case OpFight.GameOver:
                int winTeam = (int)response[0];
                view.GameOver(GameData.MyControl.Model.Team == winTeam);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �ٻ�С��
    /// </summary>
    private void onGetDog(HeroModel[] heros, DogModel[] dogs)
    {
        this.Dogs = dogs;

        int myTeam = this.GetTeam(heros, GameData.Player.id);

        GameObject go = null;

        foreach (DogModel item in dogs)
        {
            if (item.Team == 1)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_DOG + item.Name), team1DogPoints[0].position, Quaternion.identity);
                go.transform.SetParent(team1Parent);
            }
            else if (item.Team == 2)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_DOG + item.Name), team2DogPoints[0].position, Quaternion.identity);
                go.transform.SetParent(team2Parent);
            }
            //��ʼ��������
            BaseControl con = go.GetComponent<BaseControl>();
            con.Init(item, item.Team == myTeam);
            //��ӵ��ֵ���
            this.idControlDict.Add(item.Id, con);
        }
    }

    /// <summary>
    /// Ӣ�۸���
    /// </summary>
    /// <param name="hero"></param>
    private void onResurge(HeroModel hero)
    {
        BaseControl con = idControlDict[hero.Id];
        con.Model = hero;
        //������ ����״̬����
        con.ResurgeResponse();
        //����λ��
        if (hero.Team == 1)
        {
            con.transform.position = team1HeroPoints[0].transform.position;
        }
        else if (hero.Team == 2)
        {
            con.transform.position = team2HeroPoints[0].transform.position;
        }

        //������� ������ͼ
        if (GameData.MyControl == con)
        {
            view.UpdateView(hero);
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="build"></param>
    private void onResurge(BuildModel build)
    {
        BaseControl con = idControlDict[build.Id];
        con.Model = build;

        //����
        con.ResurgeResponse();
    }

    /// <summary>
    /// Ұ�ָ���
    /// </summary>
    /// <param name="build"></param>
    private void onResurge(MonsterModel monster)
    {
        BaseControl con = idControlDict[monster.Id];
        con.Model = monster;

        //����
        con.ResurgeResponse();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="heroModel"></param>
    private void onUpdataModel(HeroModel heroModel)
    {
        //��ȡ������
        BaseControl con = idControlDict[heroModel.Id];
        //����ģ��
        con.Model = heroModel;

        //������� ������ͼ
        if (GameData.MyControl == con)
        {
            view.UpdateView(heroModel);
        }
    }

    /// <summary>
    /// ʹ�ü���
    /// </summary>
    private void onSkill(int skillId, int attackId, int targetId, float x, float y, float z)
    {
        //��ȡ�����ߵĿ�����
        BaseControl con = idControlDict[attackId];
        //�жϼ�������
        if (targetId == -1)
        {
            con.SkillResponse(skillId, null, new Vector3(x, y, z));
        }
        else
        {
            //TODO
        }
        //����������ͷ� ��Ҫˢ����ȴ
        if (con == GameData.MyControl)
        {
            view.UpdateCoolDown(skillId);
        }
    }


    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="skillModel"></param>
    private void onSkillUp(int playerId, SkillModel skillModel)
    {
        //�Ȼ�ȡ������
        BaseControl con = idControlDict[playerId];

        //����Ӣ�۵ļ���
        for (int i = 0; i < ((HeroModel)con.Model).Skills.Length; i++)
        {
            SkillModel skill = ((HeroModel)con.Model).Skills[i];
            //�����������Ҫ��ļ��� ֱ����һ��ѭ��
            if (skill.Id != skillModel.Id)
                continue;

            //����� ֱ�Ӹ�ֵ����
            ((HeroModel)con.Model).Skills[i] = skillModel;
            //ˢ�����������ʾ
            if (con == GameData.MyControl)
                view.UpdateSkills((HeroModel)con.Model);
            break;
        }
    }

    /// <summary>
    /// ����װ��
    /// </summary>
    /// <param name="hero"></param>
    private void onBuy(HeroModel hero)
    {
        //��ȡID
        int id = hero.Id;
        //��������ģ��
        idControlDict[id].Model = hero;
        //�����������װ����
        if (GameData.MyControl.Model.Id == id)
            view.UpdateView(hero);
    }

    /// <summary>
    /// �ܵ��˺�
    /// </summary>
    /// <param name="damages"></param>
    private void onDamage(DamageModel[] damages)
    {
        foreach (var item in damages)
        {
            //Ŀ��ID
            int toId = item.toId;
            //��ȡ������
            BaseControl con = idControlDict[toId];
            //����
            con.Model.CurrHp -= item.damage;
            con.OnHpChange();
            //ʵ��������һ����Ѫ���� 
            HUDText.NewText("- " + item.damage, con.transform, Color.red, 16, 20f, -1f, 2.2f, bl_Guidance.Up);
            //������˵������� ��Ҫ����UI����ʾ
            if (con == GameData.MyControl)
            {
                view.UpdateView((HeroModel)con.Model);
                //��������� �ͻҰ���Ļ 
                if (item.isDead)
                    con.DeathResponse();
            }
            else //���˵�Ѫ
            {
                if (item.isDead)
                    con.DeathResponse();
            }
        }
    }

    /// <summary>
    /// ���յ��չ�����Ӧ
    /// </summary>
    /// <param name="fromId"></param>
    /// <param name="toId"></param>
    private void onAttack(int fromId, int toId)
    {
        //ʹ���߿�����
        BaseControl fromCon = idControlDict[fromId];
        //Ŀ�������
        BaseControl toCon = idControlDict[toId];
        //���ù�������
        fromCon.AttackResponse(toCon.transform);
    }

    /// <summary>
    /// ���յ��ƶ�����Ӧ
    /// </summary>
    private void onWalk(int id, float x, float y, float z)
    {
        BaseControl con = idControlDict[id];
        con.Move(new Vector3(x, y, z));
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="team1Hero">Ӣ��</param>
    /// <param name="team1Build">����</param>
    private void onGetInfo(HeroModel[] heros, BuildModel[] builds, MonsterModel[] monsters)
    {
        //Ҫ��ս�������ڵ�ģ�� �ȱ��浽����
        this.Heros = heros;
        this.Builds = builds;
        this.Monsters = monsters;

        int myTeam = this.GetTeam(heros, GameData.Player.id);

        //������Ϸ����
        GameObject go = null;

        #region Ӣ��
        foreach (HeroModel item in heros)
        {
            //����Ӣ�۵���Ϸ���� ����Ҫ����Ԥ��
            if (item.Team == 1)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_HERO + item.Name), team1HeroPoints[0].position, Quaternion.identity);
                go.transform.SetParent(team1Parent);
            }
            else if (item.Team == 2)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_HERO + item.Name), team2HeroPoints[0].position, Quaternion.identity);
                go.transform.SetParent(team2Parent);
            }
            //��ʼ��������
            BaseControl con = go.GetComponent<BaseControl>();
            con.Init(item, item.Team == myTeam);
            //�ж����Ӣ���ǲ����Լ���Ӣ��
            if (item.Id == GameData.Player.id)
            {
                // ���浱ǰ��Ӣ��
                GameData.MyControl = con;
                // ��ʼ��UIFight
                view.InitView(item);
                //���ൽ�Լ���Ӣ��
                Camera.main.GetComponent<CameraControl>().FocusOn();
            }
            //��ӵ��ֵ���
            this.idControlDict.Add(item.Id, con);
        }
        #endregion

        #region ����
        for (int i = 0; i < builds.Length; i++)
        {
            BuildModel build = builds[i];
            if (build.Team == 1)
            {
                // Main 1,Camp 2,Turret 3 
                go = team1Builds[(build.TypeId - 1)];
                go.SetActive(true);
            }
            else if (build.Team == 2)
            {
                go = team2Builds[(build.TypeId - 1)];
                go.SetActive(true);
            }
            //��ʼ��������
            BaseControl con = go.GetComponent<BaseControl>();
            con.Init(build, build.Team == myTeam);
            //��ӵ��ֵ���
            this.idControlDict.Add(build.Id, con);
        }
        #endregion

        #region Ұ��
        foreach(MonsterModel item in monsters)
        {
            if (item.Team == 1)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_MONSTER + item.Name), Wolf[1].position, Quaternion.identity);
                go.SetActive(true);
            }
            if (item.Team == 2)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_MONSTER + item.Name), Wolf[0].position, Quaternion.identity);
                go.SetActive(true);
            }
            //��ʼ��������
            BaseControl con = go.GetComponent<BaseControl>();
            con.Init(item, item.Team == myTeam);
            //��ӵ��ֵ���
            this.idControlDict.Add(item.Id, con);
        }
        #endregion
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="heros"></param>
    /// <param name="playerId"></param>
    /// <returns></returns>
    private int GetTeam(HeroModel[] heros, int playerId)
    {
        foreach (var item in heros)
        {
            if (item.Id == playerId)
                return item.Team;
        }
        return -1;
    }
}
