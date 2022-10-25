using MobaCommon.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{
    public class DogData
    {
        static Dictionary<int, DogDataModel> idDogDict = new Dictionary<int, DogDataModel>();

        static DogData()
        {
            createDog(1, "Creep_Melee_Red", 15, 2, 500, 4);
            createDog(2, "Creep_Melee_Blue", 15, 2, 500, 4);
        }

        public static DogDataModel GetDogData(int typeId)
        {
            DogDataModel model = null;
            idDogDict.TryGetValue(typeId, out model);
            return model;
        }

        /// <summary>
        /// 创建小兵
        /// </summary>
        /// <returns></returns>
        private static void createDog(int typeId, string name, int Attack, int Defense, int hp, double attackDistance)
        {
            DogDataModel dog = new DogDataModel(typeId, name, Attack, Defense, hp, attackDistance);

            //保存小兵数据
            idDogDict.Add(dog.TypeId, dog);
        }
    }

    /// <summary>
    /// 小兵的数据模型
    /// </summary>
    public class DogDataModel
    {
        /// <summary>
        /// 小兵编号
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 小兵名字
        /// </summary>
        public string Name { get; set; }
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

        public DogDataModel()
        {

        }

        public DogDataModel(int typeId, string name, int attack, int defense, int hp, double attackDistance)
        {
            this.TypeId = typeId;
            this.Name = name;
            this.Attack = attack;
            this.Defense = defense;
            this.Hp = hp;
            this.AttackDistance = attackDistance;
        }
    }
}
