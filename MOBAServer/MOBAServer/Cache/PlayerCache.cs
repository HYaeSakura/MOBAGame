using MOBAServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Threading;
using System.Collections.Concurrent;

namespace MOBAServer.Cache
{
    public class PlayerCache
    {
        #region 数据

        /// <summary>
        /// 玩家ID对应的玩家数据
        /// </summary>
        ConcurrentDictionary<int, PlayerModel> idModelDict = new ConcurrentDictionary<int, PlayerModel>();

        /// <summary>
        /// 账号ID对应的玩家ID
        /// </summary>
        SynchronizedDictionary<int, int> accPlayerDict = new SynchronizedDictionary<int, int>();


        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="accId"></param>
        public void Create(string name, int accId)
        {
            PlayerModel model = new PlayerModel();
            model.Name = name;
            model.AccountId = accId;
            //保存到数据库
            model.Id = model.Add();//返回值是 自增的id
            //保存到内存
            accPlayerDict.TryAdd(accId, model.Id);
            idModelDict.TryAdd(model.Id, model);
        }

        /// <summary>
        /// 判断是否存在角色
        /// </summary>
        /// <param name="accoutId"></param>
        /// <returns></returns>
        public bool Has(int accoutId)
        {
            if (accPlayerDict.ContainsKey(accoutId))
                return true;

            PlayerModel model = new PlayerModel();
            //如果没有 那就真的没有
            if (!model.ExistsByAccId(accoutId))
                return false;
            //如果数据库里面存在
            model.GetModelByAccId(accoutId);
            //添加到内存里
            accPlayerDict.TryAdd(accoutId, model.Id);
            idModelDict.TryAdd(model.Id, model);
            //再进行判断
            return true;
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="client"></param>
        public void AddFrient(int playerId, int friendId)
        {
            PlayerModel p1 = idModelDict[playerId];
            if (p1.FriendIdList.Equals(""))
                p1.FriendIdList += friendId.ToString();
            else
                p1.FriendIdList += "," + friendId.ToString();

            p1.Update();

            PlayerModel p2 = idModelDict[friendId];
            if (p2.FriendIdList.Equals(""))
                p2.FriendIdList += playerId.ToString();
            else
                p2.FriendIdList += "," + playerId.ToString();

            p2.Update();

        }

        /// <summary>
        /// 更新数据模型
        /// </summary>
        /// <param name="model">玩家</param>
        /// <param name="result">0胜利 1失败 2逃跑</param>
        public void UpdateModel(PlayerModel model, int result)
        {
            switch (result)
            {
                case 0://胜利
                    model.WinCount++;
                    model.Exp += 100;
                    model.Power += 100;
                    break;
                case 1://失败
                    model.LoseCount++;
                    model.Exp += 20;
                    model.Power -= 100;
                    break;
                case 2:
                    model.RunCount++;
                    model.Power -= 200;
                    break;
                default:
                    break;
            }
            //升级
            if (model.Exp >= model.lv * 100)
            {
                model.lv++;
                model.Exp = 0;
            }
            //替换原来保存的model
            idModelDict[model.Id] = model;
            //保存到数据库里面
            model.Update();
        }

        #endregion

        #region 在线

        //双向映射
        private SynchronizedDictionary<MobaClient, int> clientIdDict = new SynchronizedDictionary<MobaClient, int>();
        private SynchronizedDictionary<int, MobaClient> idClientDict = new SynchronizedDictionary<int, MobaClient>();

        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="accId"></param>
        public void Online(MobaClient client, int playerId)
        {
            clientIdDict.TryAdd(client, playerId);
            idClientDict.TryAdd(playerId, client);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="accId"></param>
        public void Offline(MobaClient client)
        {
            if (!clientIdDict.ContainsKey(client))
                return;

            int id = clientIdDict[client];

            if (clientIdDict.ContainsKey(client))
                clientIdDict.Remove(client);

            if (idClientDict.ContainsKey(id))
                idClientDict.Remove(id);
        }

        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(MobaClient client)
        {
            return clientIdDict.ContainsKey(client);
        }
        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(int playerId)
        {
            return idClientDict.ContainsKey(playerId);
        }

        #endregion

        /// <summary>
        /// 获取玩家ID
        /// </summary>
        /// <param name="accId"></param>
        public int GetId(int accId)
        {
            if (accPlayerDict.ContainsKey(accId))
                return accPlayerDict[accId];

            PlayerModel model = new PlayerModel();
            //如果没有 那就真的没有
            if (!model.ExistsByAccId(accId))
                return -1;
            //如果数据库里面存在
            model.GetModelByAccId(accId);
            //添加到内存里
            accPlayerDict.TryAdd(model.AccountId, model.Id);
            idModelDict.TryAdd(model.Id, model);
            //再进行判断
            return model.AccountId;
        }

        /// <summary>
        /// 获取在线玩家ID
        /// </summary>
        /// <param name="accId"></param>
        public int GetId(MobaClient client)
        {
            int retId = -1;
            clientIdDict.TryGetValue(client, out retId);
            return retId;
        }

        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public PlayerModel GetModel(int playerId)
        {
            if (idModelDict.ContainsKey(playerId))
                return idModelDict[playerId];

            PlayerModel model = new PlayerModel();
            //如果没有 那就真的没有
            if (!model.Exists(playerId))
                return null;
            //如果数据库里面存在
            model.GetModel(playerId);
            //添加到内存里
            accPlayerDict.TryAdd(model.AccountId, model.Id);
            idModelDict.TryAdd(model.Id, model);
            //再进行判断
            return model;
        }

        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public PlayerModel GetModel(MobaClient client)
        {
            int playerId = -1;
            if (clientIdDict.TryGetValue(client, out playerId))
            {
                return idModelDict[playerId];
            }
            return null;
        }

        /// <summary>
        /// 获取玩家数据
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public PlayerModel GetModel(string name)
        {
            foreach (PlayerModel item in idModelDict.Values)
            {
                if (item.Name == name)
                    return item;
            }

            PlayerModel model = new PlayerModel();
            //如果没有 那就真的没有
            if (!model.ExistsByName(name))
                return null;
            //如果数据库里面存在
            model.GetModelByName(name);
            //添加到内存里
            accPlayerDict.TryAdd(model.AccountId, model.Id);
            idModelDict.TryAdd(model.Id, model);
            //再进行判断
            return model;
        }

        /// <summary>
        /// 获取对应玩家的连接对象
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public MobaClient GetClient(int playerId)
        {
            return idClientDict[playerId];
        }

    }
}