using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MOBAServer.Cache
{
    public class RoomCacheBase<TRoom>
        where TRoom : Room.RoomBase<MobaClient>
    {
        /// <summary>
        /// 房间ID对应的房间数据
        /// </summary>
        protected ConcurrentDictionary<int, TRoom> idRoomDict = new ConcurrentDictionary<int, TRoom>();

        /// <summary>
        /// 玩家ID对应的房间Id
        /// </summary>
        protected ConcurrentDictionary<int, int> playerRoomDict = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// 重用的队列
        /// </summary>
        protected ConcurrentQueue<TRoom> roomQue = new ConcurrentQueue<TRoom>();

        /// <summary>
        /// 主键ID
        /// </summary>
        protected int index = 0;


    }
}
