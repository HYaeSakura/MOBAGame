using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Code
{
    public class OpSelect
    {
        /// <summary>
        /// 进入
        /// </summary>
        public const byte Enter = 0;

        /// <summary>
        /// 摧毁
        /// </summary>
        public const byte Destroy = 3;

        /// <summary>
        /// 获取信息
        /// </summary>
        public const byte GetInfo = 4;

        /// <summary>
        /// 聊天
        /// </summary>
        public const byte Talk = 5;

        /// <summary>
        /// 选人
        /// </summary>
        public const byte Select = 1;

        /// <summary>
        /// 准备
        /// </summary>
        public const byte Ready = 2;

        /// <summary>
        /// 开始战斗
        /// </summary>
        public const byte StartFight = 6;
    }
}
