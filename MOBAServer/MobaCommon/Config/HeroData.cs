using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{
    /// <summary>
    /// 英雄数据
    /// </summary>
    public class HeroData
    {
        static Dictionary<int, HeroDataModel> idModelDict = new Dictionary<int, HeroDataModel>();

        static HeroData()
        {
            //英雄的ID范围：0
            //技能的ID范围：1 1001 1002 1003 1004
            createHero(1, "狂战士", 65, 15, 500, 100, 8, 5, 60, 20, 0.5, 0.04, 4,
                new int[] { 1001, 1002, 1003, 1004 });
            createHero(2, "艾希", 55, 10, 400, 80, 15, 2, 30, 20, 0.7, 0.045, 10,
                new int[] { 2001, 2002, 2003, 2004 });
            createHero(3, "八重樱", 60, 12, 450, 0, 10, 3, 50, 0, 0.6, 0.045, 4,
                new int[] { 3001, 3002, 3003, 3004 });
            createHero(4, "姬子", 65, 15, 500, 0, 8, 5, 60, 0, 0.5, 0.04, 4,
                new int[] { 4001, 4002, 4003, 4004 });
        }

        public static HeroDataModel GetHeroData(int heroId)
        {
            HeroDataModel model = null;
            idModelDict.TryGetValue(heroId, out model);
            return model;
        }

        /// <summary>
        /// 创建英雄
        /// </summary>
        /// <returns></returns>
        private static void createHero(int id, string name, int baseAttack, int baseDefense, int hp, int mp, int growAttack, int growDefens, int growHp, int growMp, double sp, double growSp,double attackDistance, int[] skillIds)
        {
            HeroDataModel hero = new HeroDataModel(id, name, baseAttack, baseDefense, hp, mp, growAttack, growDefens, growHp, growMp, sp, growSp, attackDistance, skillIds);

            //保存英雄数据
            idModelDict.Add(hero.TypeId, hero);
        }
    }


    /// <summary>
    /// 英雄的数据模型
    /// </summary>
    public class HeroDataModel
    {
        /// <summary>
        /// 英雄编号
        /// </summary>
        public int TypeId;
        /// <summary>
        /// 英雄名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 基础攻击力
        /// </summary>
        public int BaseAttack;
        /// <summary>
        /// 基础防御力
        /// </summary>
        public int BaseDefense;
        /// <summary>
        /// 成长攻击力
        /// </summary>
        public int GrowAttack;
        /// <summary>
        /// 成长防御力
        /// </summary>
        public int GrowDefense;
        /// <summary>
        /// 生命值
        /// </summary>
        public int Hp;
        /// <summary>
        /// 成长生命
        /// </summary>
        public int GrowHp;
        /// <summary>
        /// 魔法值
        /// </summary>
        public int Mp;
        /// <summary>
        /// 成长魔法
        /// </summary>
        public int GrowMp;
        /// <summary>
        /// 攻击速度
        /// </summary>
        public double Sp;
        /// <summary>
        /// 成长攻速
        /// </summary>
        public double GrowSp;
        /// <summary>
        /// 攻击距离
        /// </summary>
        public double AttackDistance;
        /// <summary>
        /// 技能ID
        /// </summary>
        public int[] SkillIds;

        public HeroDataModel()
        {

        }

        public HeroDataModel(int id, string name, int baseAttack, int baseDefense, int hp, int mp, int growAttack, int growDefens, int growHp, int growMp, double sp, double growSp, double attackDistance, int[] skillIds)
        {
            this.TypeId = id;
            this.Name = name;
            this.BaseAttack = baseAttack;
            this.BaseDefense = baseDefense;
            this.Hp = hp;
            this.Mp = mp;
            this.GrowAttack = growAttack;
            this.GrowDefense = growDefens;
            this.GrowHp = growHp;
            this.GrowMp = growMp;
            this.Sp = sp;
            this.GrowSp = growSp;
            this.AttackDistance = attackDistance;
            this.SkillIds = skillIds;
        }
    }

}
