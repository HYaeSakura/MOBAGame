using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{
    /// <summary>
    /// 野怪
    /// </summary>
    public class MonsterData
    {
        static Dictionary<int, MonsterDataModel> idMonsterDict = new Dictionary<int, MonsterDataModel>();

        static MonsterData()
        {
            createMonster(1, "Siege_Creep_Blue", 700, 30, 10, 8, true, true, 5);
            createMonster(2, "Siege_Creep_Red", 700, 30, 10, 8, true, true, 5);
        }

        public static MonsterDataModel GetMonsterData(int typeId)
        {
            MonsterDataModel model = null;
            idMonsterDict.TryGetValue(typeId, out model);
            return model;
        }

        /// <summary>
        /// 创建野怪
        /// </summary>
        /// <returns></returns>
        private static void createMonster(int typeId, string name, int hp, int attack, int defense, double attackDistance, bool agressire, bool rebirth, int rebirthTime)
        {
            MonsterDataModel monster = new MonsterDataModel(typeId, name, hp, attack, defense, attackDistance, agressire, rebirth, rebirthTime);

            //保存野怪数据
            idMonsterDict.Add(monster.TypeId, monster);
        }
    }

    /// <summary>
    /// 野怪的数据模型
    /// </summary>
    public class MonsterDataModel
    {
        /// <summary>
        /// 野怪编号
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 野怪名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 野怪队伍
        /// </summary>
        public int Team { get; set; }
        /// <summary>
        /// 基础攻击力
        /// </summary>
        public int Attack { get; set; }
        /// <summary>
        /// 基础防御力
        /// </summary>
        public int Defense { get; set; }
        /// <summary>
        /// 生命值
        /// </summary>
        public int Hp { get; set; }
        /// <summary>
        /// 攻击距离
        /// </summary>
        public double AttackDistance { get; set; }
        /// <summary>
        /// 是否攻击
        /// </summary>
        public bool Agressire { get; set; }
        /// <summary>
        /// 是否重生
        /// </summary>
        public bool Rebirth { get; set; }
        /// <summary>
        /// 重生时间
        /// </summary>
        public int RebirthTime { get; set; }

        public MonsterDataModel()
        {

        }

        public MonsterDataModel(int typeId, string name, int hp, int attack, int defense, double attackDistance, bool agressire, bool rebirth, int rebirthTime)
        {
            this.TypeId = typeId;
            this.Name = name;
            this.Hp = hp;
            this.Attack = attack;
            this.Defense = defense;
            this.AttackDistance = attackDistance;
            this.Agressire = agressire;
            this.Rebirth = rebirth;
            this.RebirthTime = rebirthTime;
        }
    }
}
