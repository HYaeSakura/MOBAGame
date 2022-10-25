using System;
using System.Collections.Generic;
using Photon.SocketServer;
using MobaCommon.Dto;
using MOBAServer.Cache;
using MOBAServer.Room;
using MobaCommon.Code;
using LitJson;
using MobaCommon.Dto.Skill;
using MobaCommon.Config;
using MOBAServer.Model;

namespace MOBAServer.Logic
{
    public class FightHandler : SingeSend, IOpHandler
    {
        #region 缓存层

        public FightCache fightCache
        {
            get { return Caches.Fight; }
        }

        public PlayerCache playerCache
        {
            get { return Caches.Player; }
        }

        #endregion

        /// <summary>
        /// 开始战斗
        /// </summary>
        public void StartFight(List<SelectModel> team1, List<SelectModel> team2)
        {
            fightCache.CreateRoom(team1, team2);
        }

        public void OnDisconnect(MobaClient client)
        {
            fightCache.Offline(client, playerCache.GetId(client));
        }

        public void OnRequest(MobaClient client, byte subCode, OperationRequest request)
        {
            switch (subCode)
            {
                case OpFight.Enter:
                    onEnter(client, (int)request[0]);
                    break;
                case OpFight.Walk:
                    onWalk(client, (float)request[0], (float)request[1], (float)request[2]);
                    break;
                case OpFight.DogWalk:
                    onDogWalk(client, (int)request[0], (float)request[1], (float)request[2], (float)request[3]);
                    break;
                case OpFight.Skill:
                    onSkill(client, (int)request[0], (int)request[1], (int)request[2], (float)request[3], (float)request[4], (float)request[5]);
                    break;
                case OpFight.Damage:
                    onDamage(client, (int)request[0], (int)request[1], (int[])request[2]);
                    break;
                //买装备 服务器收到的请求参数：装备的ID
                case OpFight.Buy:
                    onBuy(client, (int)request[0]);
                    break;
                case OpFight.Sale:
                    onSale(client, (int)request[0]);
                    break;
                case OpFight.SkillUp:
                    onSkillUp(client, (int)request[0]);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 技能升级
        /// </summary>
        /// <param name="client"></param>
        /// <param name="v"></param>
        private void onSkillUp(MobaClient client, int skillId)
        {
            //获取房间模型
            int playerId = playerCache.GetId(client);
            FightRoom room = fightCache.GetRoom(playerId);
            if (room == null)
                return;
            //获取英雄数据模型
            HeroModel hero = room.GetHeroModel(playerId);
            if (hero == null)
                return;
            //没有技能点数了 就不作处理
            if (hero.Points <= 0)
                return;
            //可以加点
            foreach (var item in hero.Skills)
            {
                //不是我们想加点的技能 就开始下一轮
                if (item.Id != skillId)
                    continue;
                //如果玩家等级 没有到达 技能学习的要求 或者 技能已满级
                if (item.LearnLevel > hero.Level || item.LearnLevel == -1)
                    return;
                //扣点数
                hero.Points--;
                //先获取技能下一级的数据
                SkillLevelDataModel data = SkillData.GetSkillData(skillId).LvModels[++item.Level];
                //修改技能
                item.LearnLevel = data.LearnLv;
                item.Distance = data.Distance;
                item.CoolDown = data.CoolDown;//等等 在这里修改想修改的数据
                //广播 谁 更新了 什么技能
                room.Brocast(OpCode.FightCode, OpFight.SkillUp, 0, "有人点技能了", null, playerId, JsonMapper.ToJson(item));
                break;
            }
        }

        /// <summary>
        /// 卖装备
        /// </summary>
        /// <param name="client"></param>
        /// <param name="itemId"></param>
        private void onSale(MobaClient client, int itemId)
        {
            //检测有没有装备
            ItemModel item = ItemData.GetItem(itemId);
            if (item == null)
                return;
            //获取房间模型
            int playerId = playerCache.GetId(client);
            FightRoom room = fightCache.GetRoom(playerId);
            if (room == null)
                return;
            //获取英雄数据模型
            HeroModel hero = room.GetHeroModel(playerId);
            //开始遍历
            for (int i = 0; i < hero.Equipments.Length; i++)
            {
                if (hero.Equipments[i] == itemId)
                {
                    //开始出售
                    hero.Money += item.Price;
                    //给他装备的ID
                    hero.Equipments[i] = -1;
                    //增加属性
                    hero.Attack -= item.Attack;
                    hero.Defense -= item.Defense;
                    hero.MaxHp -= item.Hp;
                    //给房间内所有客户端发消息了 HeroModel
                    room.Brocast(OpCode.FightCode, OpFight.Sale, 0, "有人卖装备了", null, JsonMapper.ToJson(hero));
                    return;
                }
            }
            //走到这里 就代表出售失败了
            Send(client, OpCode.FightCode, OpFight.Sale, -1, "出售失败");
            return;
        }

        /// <summary>
        /// 买装备
        /// </summary>
        /// <param name="client"></param>
        /// <param name="itemId"></param>
        private void onBuy(MobaClient client, int itemId)
        {
            //检测有没有装备
            ItemModel item = ItemData.GetItem(itemId);
            if (item == null)
                return;
            //获取房间模型
            int playerId = playerCache.GetId(client);
            FightRoom room = fightCache.GetRoom(playerId);
            if (room == null)
                return;
            //获取英雄数据模型
            HeroModel hero = room.GetHeroModel(playerId);
            //检测钱够不够
            if (hero.Money < item.Price)
            {
                Send(client, OpCode.FightCode, OpFight.Buy, -1, "金币不足");
                return;
            }
            //添加装备
            for (int i = 0; i < hero.Equipments.Length; i++)
            {
                //-1 代表 没有装备
                if (hero.Equipments[i] == -1)
                {
                    //开始购买
                    hero.Money -= item.Price;
                    //给他装备的ID
                    hero.Equipments[i] = itemId;
                    //增加属性
                    hero.Attack += item.Attack;
                    hero.Defense += item.Defense;
                    hero.MaxHp += item.Hp;
                    //给房间内所有客户端发消息了 HeroModel
                    room.Brocast(OpCode.FightCode, OpFight.Buy, 0, "有人购买装备了", null, JsonMapper.ToJson(hero));
                    return;
                }
            }
            //如果走到这里 就代表没买成功 肯定是一种结果 没有格子了 装备已满
            Send(client, OpCode.FightCode, OpFight.Buy, -2, "装备已满");
            return;
        }

        /// <summary>
        /// 计算伤害
        /// </summary>
        private void onDamage(MobaClient client, int attackId, int skillId, int[] targetId)
        {
            //1.获取房间模型
            int playerId = playerCache.GetId(client);
            FightRoom room = fightCache.GetRoom(playerId);

            //2.判断是谁攻击 谁被攻击
            //攻击者的数据模型
            DogModel attackModel = null;
            if (attackId >= 0)
            {
                //攻击者的ID大于0 就是英雄攻击
                attackModel = room.GetHeroModel(attackId);
            }
            else if (attackId <= -10 && attackId >= -30)
            {
                //id为 -10~-30 就是防御塔攻击
                attackModel = room.GetBuildModel(attackId);
            }
            else if (attackId <= -1000)
            {
                //就是小兵攻击
                attackModel = room.GetDogModel(attackId);
            }
            else if (attackId <= -500 && attackId > -1000)
            {
                //就是野怪攻击
                attackModel = room.GetMonsterModel(attackId);
            }
            //被攻击者的数据模型
            DogModel[] targetModels = new DogModel[targetId.Length];
            for (int i = 0; i < targetId.Length; i++)
            {
                if (targetId[i] >= 0)
                {
                    //攻击者的ID大于0 就是英雄被攻击
                    targetModels[i] = room.GetHeroModel(targetId[i]);
                }
                else if (targetId[i] <= -10 && targetId[i] >= -30)
                {
                    //id为 -10~-30 就是防御塔被攻击
                    targetModels[i] = room.GetBuildModel(targetId[i]);
                }
                else if (targetId[i] <= -1000)
                {
                    //就是小兵被攻击
                    targetModels[i] = room.GetDogModel(targetId[i]);
                }
                else if (targetId[i] <= -500 && targetId[i] > -1000)
                {
                    //就是野怪被攻击
                    targetModels[i] = room.GetMonsterModel(targetId[i]);
                }
            }
            //3.根据技能ID 判断出 是 普通攻击 还是 特殊技能
            //4.根据伤害表 根据技能id 获取ISKILL 调用damage 计算伤害
            ISkill skill = null;
            List<DamageModel> damages = null;
            //获取Skill
            skill = DamageData.GetSkill(skillId);
            //计算出伤害
            damages = skill.Damage(skillId, 0, attackModel, targetModels);
            //6.给房间内的客户端广播数据模型
            room.Brocast(OpCode.FightCode, OpFight.Damage, 0, "有伤害产生", null, JsonMapper.ToJson(damages.ToArray()));

            //结算
            foreach(var item in targetModels)
            {
                if (item.CurrHp <= 0)
                {
                    switch (item.ModelType)
                    {
                        #region 小兵
                        case ModelType.DOG:
                            //如果是英雄攻击导致死亡 给钱和经验
                            if (attackModel.Id >= 0)
                            {
                                //加钱
                                ((HeroModel)attackModel).Money += 20;
                                //加经验
                                ((HeroModel)attackModel).Exp += 20;
                                //检测是否升级
                                if (((HeroModel)attackModel).Exp >= ((HeroModel)attackModel).Level*100)
                                {
                                    //升级
                                    ((HeroModel)attackModel).Level++;
                                    //技能点数
                                    ((HeroModel)attackModel).Points++;
                                    //重置经验值
                                    ((HeroModel)attackModel).Exp = 0;

                                    HeroDataModel data = HeroData.GetHeroData(attackModel.Id);
                                    //英雄成长属性增加
                                    ((HeroModel)attackModel).Attack += data.GrowAttack;
                                    ((HeroModel)attackModel).Defense += data.GrowDefense;
                                    ((HeroModel)attackModel).MaxHp += data.GrowHp;
                                    ((HeroModel)attackModel).MaxMp += data.GrowMp;
                                    ((HeroModel)attackModel).Sp += data.GrowSp;
                                }
                                //给客户端发送数据模型attackModel 客户端更新
                                room.Brocast(OpCode.FightCode, OpFight.UpdataModel, 0, "更新数据模型", null, JsonMapper.ToJson((HeroModel)attackModel));
                            }
                            //移除小兵
                            room.RemoveDog(item);
                            break;
                        #endregion

                        #region 建筑
                        case ModelType.BUILD:
                            if (attackModel.Id >= 0)
                            {
                                //加钱
                                ((HeroModel)attackModel).Money += 300;
                                //给客户端发attackModel
                                //TODO
                            }
                            //检测防御塔是否可重生
                            if (((BuildModel)item).Rebirth)
                            {
                                //开启一个定时任务 在指定的时间之后 复活
                                room.StartSchedule(DateTime.UtcNow.AddSeconds((double)((BuildModel)item).RebirthTime),
                                    () =>
                                    {
                                        //满状态
                                        ((BuildModel)item).CurrHp = ((BuildModel)item).MaxHp;
                                        //给客户端发送一个复活的消息 参数 item
                                        //用RetCode区分谁复活了
                                        room.Brocast(OpCode.FightCode, OpFight.Resurge, 1, "有模型复活", null, JsonMapper.ToJson((BuildModel)item));
                                    });
                            }
                            //不重生 直接移除数据模型
                            else
                                room.RemoveBuild((BuildModel)item);

                            //游戏结束的判断
                            if (item.Id == -10)
                            {
                                onGameOver(room, 2);
                            }
                            else if (item.Id == -20)
                            {
                                onGameOver(room, 1);
                            }
                            break;
                        #endregion

                        #region 英雄
                        case ModelType.HERO:
                            //发奖励
                            if (attackModel.Id >= 0)
                            {
                                //加杀人数
                                ((HeroModel)item).Kill++;
                                //加钱
                                ((HeroModel)attackModel).Money += 300;
                                //加经验
                                ((HeroModel)attackModel).Exp += 50;
                                //检测是否升级
                                if (((HeroModel)attackModel).Exp >= ((HeroModel)attackModel).Level * 100)
                                {
                                    //升级
                                    ((HeroModel)attackModel).Level++;
                                    //技能点数
                                    ((HeroModel)attackModel).Points++;
                                    //重置经验值
                                    ((HeroModel)attackModel).Exp = 0;

                                    HeroDataModel data = HeroData.GetHeroData(attackModel.Id);
                                    //英雄成长属性增加
                                    ((HeroModel)attackModel).Attack += data.GrowAttack;
                                    ((HeroModel)attackModel).Defense += data.GrowDefense;
                                    ((HeroModel)attackModel).MaxHp += data.GrowHp;
                                    ((HeroModel)attackModel).MaxMp += data.GrowMp;
                                    ((HeroModel)attackModel).Sp += data.GrowSp;
                                }
                                //给客户端发attackModel
                                room.Brocast(OpCode.FightCode, OpFight.UpdataModel, 0, "更新数据模型", null, JsonMapper.ToJson((HeroModel)attackModel));
                            }
                            //目标英雄死亡
                            ((HeroModel)item).Dead++;
                            //开启一个定时任务 在指定的时间之后 复活
                            room.StartSchedule(DateTime.UtcNow.AddSeconds(((HeroModel)item).Level*2),
                                () =>
                                {
                                        //满状态
                                        ((HeroModel)item).CurrHp = ((HeroModel)item).MaxHp;
                                        ((HeroModel)item).CurrMp = ((HeroModel)item).MaxMp;
                                        ((HeroModel)item).Sp = ((HeroModel)item).Sp;
                                    //给客户端发送一个复活的消息 参数 item
                                    //用RetCode区分谁复活了
                                    room.Brocast(OpCode.FightCode, OpFight.Resurge, 0, "有模型复活", null, JsonMapper.ToJson((HeroModel)item));
                                });
                            break;
                        #endregion

                        #region 野怪
                        case ModelType.MONSTER:
                            //如果是英雄攻击导致死亡 给钱和经验
                            if (attackModel.Id >= 0)
                            {
                                //加钱
                                ((HeroModel)attackModel).Money += 100;
                                //加经验
                                ((HeroModel)attackModel).Exp += 100;
                                //检测是否升级
                                if (((HeroModel)attackModel).Exp >= ((HeroModel)attackModel).Level * 100)
                                {
                                    //升级
                                    ((HeroModel)attackModel).Level++;
                                    //技能点数
                                    ((HeroModel)attackModel).Points++;
                                    //重置经验值
                                    ((HeroModel)attackModel).Exp = 0;

                                    HeroDataModel data = HeroData.GetHeroData(attackModel.Id);
                                    //英雄成长属性增加
                                    ((HeroModel)attackModel).Attack += data.GrowAttack;
                                    ((HeroModel)attackModel).Defense += data.GrowDefense;
                                    ((HeroModel)attackModel).MaxHp += data.GrowHp;
                                    ((HeroModel)attackModel).MaxMp += data.GrowMp;
                                    ((HeroModel)attackModel).Sp += data.GrowSp;
                                }
                                //给客户端发送数据模型attackModel 客户端更新
                                room.Brocast(OpCode.FightCode, OpFight.UpdataModel, 0, "更新数据模型", null, JsonMapper.ToJson((HeroModel)attackModel));
                            }
                            //开启一个定时任务 在指定的时间之后 复活
                            room.StartSchedule(DateTime.UtcNow.AddSeconds(((MonsterModel)item).RebirthTime),
                                () =>
                                {
                                    //满状态
                                    ((MonsterModel)item).CurrHp = ((MonsterModel)item).MaxHp;
                                    //给客户端发送一个复活的消息 参数 item
                                    //用RetCode区分谁复活了
                                    room.Brocast(OpCode.FightCode, OpFight.Resurge, 2, "有模型复活", null, JsonMapper.ToJson((MonsterModel)item));
                                });
                            break;
                        #endregion
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="room"></param>
        /// <param name="winTeam"></param>
        private void onGameOver(FightRoom room, int winTeam)
        {
            //广播胜利的队伍
            room.Brocast(OpCode.FightCode, OpFight.GameOver, 0, "游戏结束", null, winTeam);

            //更新玩家数据
            foreach(MobaClient client in room.ClientList)
            {
                //获取玩家数据模型
                PlayerModel model = playerCache.GetModel(client);
                if (room.LeaveClient.Contains(client))
                {
                    //更新逃跑的场次
                    playerCache.UpdateModel(model, 2);
                }
                HeroModel hero = room.GetHeroModel(model.Id);
                if (hero.Team == winTeam)
                {
                    //赢了
                    playerCache.UpdateModel(model, 0);
                }
                else
                {
                    //输了
                    playerCache.UpdateModel(model, 1);
                }
            }
            //销毁房间
            fightCache.Destroy(room.Id);
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        private void onSkill(MobaClient client, int skillId, int attackId, int targetId, float x, float y, float z)
        {
            int playerId = playerCache.GetId(client);
            FightRoom room = fightCache.GetRoom(playerId);
            //先判断 是不是普通攻击
            if (skillId == 1)
            {
                //参数：1、攻击者ID，2、被攻击者ID
                room.Brocast
                    (OpCode.FightCode, OpFight.Skill, 0, "有人普通攻击", null, attackId, targetId);
            }
            //是技能 从技能配置表里面通过技能ID获取到技能信息 然后再广播
            else
            {
                if (targetId == -1)
                {
                    //指定点的技能
                    room.Brocast
                        (OpCode.FightCode, OpFight.Skill, 1, "点释放技能", null, skillId, attackId, -1, x, y, z);
                }
                else
                {
                    //指定目标的技能
                    room.Brocast
                        (OpCode.FightCode, OpFight.Skill, 1, "选择目标技能", null, skillId, attackId, targetId);
                }
            }
        }

        /// <summary>
        /// 玩家移动
        /// </summary>
        /// <param name="client"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void onWalk(MobaClient client, float x, float y, float z)
        {
            int playerId = playerCache.GetId(client);
            FightRoom room = fightCache.GetRoom(playerId);
            if (room == null)
                return;
            //给每一个客户端发送信息：谁 移动到 哪
            room.Brocast(OpCode.FightCode, OpFight.Walk, 0, "有玩家移动", null, playerId, x, y, z);
        }

        /// <summary>
        /// 小兵移动
        /// </summary>
        /// <param name="client"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void onDogWalk(MobaClient client, int dogId, float x, float y, float z)
        {
            int playerId = playerCache.GetId(client);
            FightRoom room = fightCache.GetRoom(playerId);
            if (room == null)
                return;
            //给每一个客户端发送信息：谁 移动到 哪
            room.Brocast(OpCode.FightCode, OpFight.Walk, 0, "有小兵移动", null, dogId, x, y, z);
        }

        /// <summary>
        /// 玩家进入
        /// </summary>
        /// <param name="playerId">玩家Id</param>
        private void onEnter(MobaClient client, int playerId)
        {
            FightRoom room = fightCache.Enter(playerId, client);
            if (room == null)
                return;
            //首先要判断是否全部进入了，
            //作用：保证竞技游戏的公平
            if (!room.IsAllEnter)
                return;
            //给每一个客户端发送战斗房间的信息
            room.Brocast(OpCode.FightCode, OpFight.GetInfo, 0, "加载战斗场景数据", null,
                JsonMapper.ToJson(room.Heros), JsonMapper.ToJson(room.Builds), JsonMapper.ToJson(room.Monsters));
        }
    }
}
