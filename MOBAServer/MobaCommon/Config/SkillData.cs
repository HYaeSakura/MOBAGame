using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{
    /// <summary>
    /// 技能表
    /// </summary>
    public class SkillData
    {
        static Dictionary<int, SkillDataModel> idSkillDict = new Dictionary<int, SkillDataModel>();

        static SkillData()
        {
            //战士技能表
            createSkill(1001, "致馋打击", "狂战士开始立定跳高，跳下时对敌人造成打击，让敌人发饿",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 7, 100, 20, 10),
                new SkillLevelDataModel(5, 6, 120, 30, 10),
                new SkillLevelDataModel(9, 5, 150, 40, 10),
                new SkillLevelDataModel(-1, 4, 200, 50, 10));

            createSkill(1002, "无情灌饭", "狂战士向前横扫，给前方所有敌方单位喂饭",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 9, 80, 10, 10),
                new SkillLevelDataModel(5, 7, 90, 15, 10),
                new SkillLevelDataModel(9, 5, 100, 20, 10),
                new SkillLevelDataModel(-1, 4, 90, 40, 10));

            createSkill(1003, "祖国人", "狂战士开始想念美国，美国恩赐给敌人回血",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 18, -120, 0, 10),
                new SkillLevelDataModel(5, 16, -150, 0, 10),
                new SkillLevelDataModel(9, 14, -200, 0, 10),
                new SkillLevelDataModel(-1, 4, -90, 0, 10));

            createSkill(1004, "故技重施", "模仿致馋打击，但是无消耗伤害更高，哈哈，无消耗！",
                new SkillLevelDataModel(6, 0, 0, 0, 0),
                new SkillLevelDataModel(9, 100, 150, 0, 10),
                new SkillLevelDataModel(11, 95, 180, 0, 10),
                new SkillLevelDataModel(16, 80, 220, 0, 10));

            //射手技能表
            createSkill(2001, "强力打击", "艾希为自己的剑加强伤害",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 7, 100, 20, 10),
                new SkillLevelDataModel(5, 6, 120, 30, 10),
                new SkillLevelDataModel(9, 5, 150, 40, 10),
                new SkillLevelDataModel(-1, 4, 200, 50, 10));

            createSkill(2002, "万箭齐发", "艾希一次射出扇形区域的箭",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 9, 80, 10, 10),
                new SkillLevelDataModel(5, 7, 90, 15, 10),
                new SkillLevelDataModel(9, 5, 100, 20, 10),
                new SkillLevelDataModel(-1, 4, 90, 40, 10));

            createSkill(2003, "开眼", "发射一只鸟",
                new SkillLevelDataModel(1, 0, 0, 0, 0),
                new SkillLevelDataModel(3, 0, 0, 0, 0),
                new SkillLevelDataModel(5, 0, 0, 0, 0),
                new SkillLevelDataModel(9, 0, 0, 0, 0),
                new SkillLevelDataModel(-1, 0, 0, 0, 0));

            createSkill(2004, "千钧一发", "发射一只箭冻结敌人",
                new SkillLevelDataModel(6, 0, 0, 0, 0),
                new SkillLevelDataModel(9, 100, 150, 0, 10),
                new SkillLevelDataModel(11, 95, 180, 0, 10),
                new SkillLevelDataModel(16, 80, 220, 0, 10));
        }

        private static void createSkill(int id, string name, string des, params SkillLevelDataModel[] lvModels)
        {
            //创建数据
            SkillDataModel data = new SkillDataModel(id, name, des, lvModels);
            //保存到字典
            idSkillDict.Add(id, data);
        }

        public static SkillDataModel GetSkillData(int id)
        {
            SkillDataModel data = null;
            idSkillDict.TryGetValue(id, out data);
            return data;
        }
    }

    /// <summary>
    /// 技能的数据模型
    /// </summary>
    public class SkillDataModel
    {
        /// <summary>
        /// 技能识别码
        /// </summary>
        public int Id;
        /// <summary>
        /// 技能的名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 技能的说明
        /// </summary>
        public string Description;
        /// <summary>
        /// 技能等级的信息
        /// </summary>
        public SkillLevelDataModel[] LvModels;

        public SkillDataModel() { }

        public SkillDataModel(int id, string name, string des, SkillLevelDataModel[] lvModels)
        {
            this.Id = id;
            this.Name = name;
            this.Description = des;
            this.LvModels = lvModels;
        }
    }

    /// <summary>
    /// 技能等级的数据模型
    /// </summary>
    public class SkillLevelDataModel
    {
        /// <summary>
        /// 学习技能的等级
        /// </summary>
        public int LearnLv;
        /// <summary>
        /// 技能冷却
        /// </summary>
        public int CoolDown;
        /// <summary>
        /// 技能伤害
        /// </summary>
        public int Damage;
        /// <summary>
        /// 蓝消耗
        /// </summary>
        public int Mp;
        /// <summary>
        /// 技能的距离
        /// </summary>
        public double Distance;

        public SkillLevelDataModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="learnLv">等级</param>
        /// <param name="cd">冷却</param>
        /// <param name="mp">耗蓝</param>
        /// <param name="distance">距离</param>
        public SkillLevelDataModel(int learnLv, int cd, int damage, int mp, double distance)
        {
            this.LearnLv = learnLv;
            this.CoolDown = cd;
            this.Damage = damage;
            this.Mp = mp;
            this.Distance = distance;
        }
    }
}