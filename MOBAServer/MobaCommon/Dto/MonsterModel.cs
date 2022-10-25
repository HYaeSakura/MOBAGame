using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    /// <summary>
    /// 野怪模型
    /// </summary>
    public class MonsterModel : DogModel
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

        public MonsterModel()
        {

        }

        public MonsterModel(int id, int typeId, int team, string name, int maxHp, int attack, int defense, double attackDistance, bool agressire, bool rebirth, int rebirthTime)
            : base(id, typeId, team, name, maxHp, attack, defense, attackDistance)
        {
            this.Agressire = agressire;
            this.Rebirth = rebirth;
            this.RebirthTime = rebirthTime;
        }
    }
}
