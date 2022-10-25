using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Config
{
    /// <summary>
    /// 装备的数据
    /// </summary>
    public class ItemData
    {
        /// <summary>
        /// ID和装备的映射关系
        /// </summary>
        static Dictionary<int, ItemModel> itemDict = new Dictionary<int, ItemModel>();

        static ItemData()
        {
            itemDict.Add(1, new ItemModel("磁暴·斩", 1, 20, 10, 100, 500));
        }

        /// <summary>
        /// 根据id来获取数据模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ItemModel GetItem(int id)
        {
            ItemModel item = null;
            itemDict.TryGetValue(id, out item);
            return item;
        }

    }

    /// <summary>
    /// 装备的数据模型
    /// </summary>
    public class ItemModel
    {
        public int Id;
        public int Attack;
        public int Defense;
        public int Hp;
        public string Name;
        public int Price;

        public ItemModel() { }

        public ItemModel(string name, int id, int attack, int defense, int hp, int price)
        {
            this.Attack = attack;
            this.Defense = defense;
            this.Hp = hp;
            this.Price = price;
            this.Name = name;
            this.Id = id;
        }
    }
}
