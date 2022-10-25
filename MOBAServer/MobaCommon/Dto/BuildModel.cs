using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    /// <summary>
    /// 建筑数据模型
    /// </summary>
    public class BuildModel : DogModel
    {
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

        public BuildModel()
        {

        }


        public BuildModel(int id, int typeId, int team, int maxHp, int attack, int defense, double attackDistance, string name, bool agressire, bool rebirth, int rebirthTime)
            : base(id, typeId, team, name, maxHp, attack, defense, attackDistance)
        {
            this.Agressire = agressire;
            this.Rebirth = rebirth;
            this.RebirthTime = rebirthTime;
        }
    }
}
