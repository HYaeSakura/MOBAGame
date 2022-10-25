using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MOBAServer;

namespace MOBAServer.Model
{
    /// <summary>
    /// 类PlayerModel。
    /// </summary>
    [Serializable]
    public partial class PlayerModel
    {
        public PlayerModel() { }

        #region Model
        private int _id;

        private string _name;
        private int _accountid;

        private int _lv = 1;
        private int _exp = 0;
        private int _power = 0;
        private int _wincount = 0;
        private int _losecount = 0;
        private int _runcount = 0;
        private string _heroidlist = "1,2";
        private string _friendidlist = "9";
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int lv
        {
            set { _lv = value; }
            get { return _lv; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Exp
        {
            set { _exp = value; }
            get { return _exp; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Power
        {
            set { _power = value; }
            get { return _power; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int WinCount
        {
            set { _wincount = value; }
            get { return _wincount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LoseCount
        {
            set { _losecount = value; }
            get { return _losecount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RunCount
        {
            set { _runcount = value; }
            get { return _runcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeroIdList
        {
            set { _heroidlist = value; }
            get { return _heroidlist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FriendIdList
        {
            set { _friendidlist = value; }
            get { return _friendidlist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AccountId
        {
            set { _accountid = value; }
            get { return _accountid; }
        }
        #endregion Model


        #region  Method

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public PlayerModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,lv,Exp,Power,WinCount,LoseCount,RunCount,HeroIdList,FriendIdList,AccountId ");
            strSql.Append(" FROM [PlayerModel] ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            DataSet ds = DbHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"] != null && ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    this.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Name"] != null)
                {
                    this.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["lv"] != null && ds.Tables[0].Rows[0]["lv"].ToString() != "")
                {
                    this.lv = int.Parse(ds.Tables[0].Rows[0]["lv"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Exp"] != null && ds.Tables[0].Rows[0]["Exp"].ToString() != "")
                {
                    this.Exp = int.Parse(ds.Tables[0].Rows[0]["Exp"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Power"] != null && ds.Tables[0].Rows[0]["Power"].ToString() != "")
                {
                    this.Power = int.Parse(ds.Tables[0].Rows[0]["Power"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WinCount"] != null && ds.Tables[0].Rows[0]["WinCount"].ToString() != "")
                {
                    this.WinCount = int.Parse(ds.Tables[0].Rows[0]["WinCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoseCount"] != null && ds.Tables[0].Rows[0]["LoseCount"].ToString() != "")
                {
                    this.LoseCount = int.Parse(ds.Tables[0].Rows[0]["LoseCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RunCount"] != null && ds.Tables[0].Rows[0]["RunCount"].ToString() != "")
                {
                    this.RunCount = int.Parse(ds.Tables[0].Rows[0]["RunCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HeroIdList"] != null)
                {
                    this.HeroIdList = ds.Tables[0].Rows[0]["HeroIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["FriendIdList"] != null)
                {
                    this.FriendIdList = ds.Tables[0].Rows[0]["FriendIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["AccountId"] != null && ds.Tables[0].Rows[0]["AccountId"].ToString() != "")
                {
                    this.AccountId = int.Parse(ds.Tables[0].Rows[0]["AccountId"].ToString());
                }
            }
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsByAccId(int accountId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PlayerModel]");
            strSql.Append(" where AccountId=@accountId ");

            SqlParameter[] parameters = {
                    new SqlParameter("@accountId", SqlDbType.Int,4)};
            parameters[0].Value = accountId;

            return DbHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsByName(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PlayerModel]");
            strSql.Append(" where Name=@Name ");

            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,16)};
            parameters[0].Value = name;

            return DbHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PlayerModel]");
            strSql.Append(" where Id=@Id ");

            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PlayerModel] (");
            strSql.Append("Name,lv,Exp,Power,WinCount,LoseCount,RunCount,HeroIdList,FriendIdList,AccountId)");
            strSql.Append(" values (");
            strSql.Append("@Name,@lv,@Exp,@Power,@WinCount,@LoseCount,@RunCount,@HeroIdList,@FriendIdList,@AccountId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,16),
                    new SqlParameter("@lv", SqlDbType.Int,4),
                    new SqlParameter("@Exp", SqlDbType.Int,4),
                    new SqlParameter("@Power", SqlDbType.Int,4),
                    new SqlParameter("@WinCount", SqlDbType.Int,4),
                    new SqlParameter("@LoseCount", SqlDbType.Int,4),
                    new SqlParameter("@RunCount", SqlDbType.Int,4),
                    new SqlParameter("@HeroIdList", SqlDbType.VarChar,64),
                    new SqlParameter("@FriendIdList", SqlDbType.VarChar,64),
                    new SqlParameter("@AccountId", SqlDbType.Int,4)};
            parameters[0].Value = Name;
            parameters[1].Value = lv;
            parameters[2].Value = Exp;
            parameters[3].Value = Power;
            parameters[4].Value = WinCount;
            parameters[5].Value = LoseCount;
            parameters[6].Value = RunCount;
            parameters[7].Value = HeroIdList;
            parameters[8].Value = FriendIdList;
            parameters[9].Value = AccountId;

            object obj = DbHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PlayerModel] set ");
            strSql.Append("Name=@Name,");
            strSql.Append("lv=@lv,");
            strSql.Append("Exp=@Exp,");
            strSql.Append("Power=@Power,");
            strSql.Append("WinCount=@WinCount,");
            strSql.Append("LoseCount=@LoseCount,");
            strSql.Append("RunCount=@RunCount,");
            strSql.Append("HeroIdList=@HeroIdList,");
            strSql.Append("FriendIdList=@FriendIdList,");
            strSql.Append("AccountId=@AccountId");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,16),
                    new SqlParameter("@lv", SqlDbType.Int,4),
                    new SqlParameter("@Exp", SqlDbType.Int,4),
                    new SqlParameter("@Power", SqlDbType.Int,4),
                    new SqlParameter("@WinCount", SqlDbType.Int,4),
                    new SqlParameter("@LoseCount", SqlDbType.Int,4),
                    new SqlParameter("@RunCount", SqlDbType.Int,4),
                    new SqlParameter("@HeroIdList", SqlDbType.VarChar,64),
                    new SqlParameter("@FriendIdList", SqlDbType.VarChar,64),
                    new SqlParameter("@AccountId", SqlDbType.Int,4),
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Name;
            parameters[1].Value = lv;
            parameters[2].Value = Exp;
            parameters[3].Value = Power;
            parameters[4].Value = WinCount;
            parameters[5].Value = LoseCount;
            parameters[6].Value = RunCount;
            parameters[7].Value = HeroIdList;
            parameters[8].Value = FriendIdList;
            parameters[9].Value = AccountId;
            parameters[10].Value = Id;

            int rows = DbHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [PlayerModel] ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            int rows = DbHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public void GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,lv,Exp,Power,WinCount,LoseCount,RunCount,HeroIdList,FriendIdList,AccountId ");
            strSql.Append(" FROM [PlayerModel] ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            DataSet ds = DbHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"] != null && ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    this.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Name"] != null)
                {
                    this.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["lv"] != null && ds.Tables[0].Rows[0]["lv"].ToString() != "")
                {
                    this.lv = int.Parse(ds.Tables[0].Rows[0]["lv"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Exp"] != null && ds.Tables[0].Rows[0]["Exp"].ToString() != "")
                {
                    this.Exp = int.Parse(ds.Tables[0].Rows[0]["Exp"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Power"] != null && ds.Tables[0].Rows[0]["Power"].ToString() != "")
                {
                    this.Power = int.Parse(ds.Tables[0].Rows[0]["Power"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WinCount"] != null && ds.Tables[0].Rows[0]["WinCount"].ToString() != "")
                {
                    this.WinCount = int.Parse(ds.Tables[0].Rows[0]["WinCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoseCount"] != null && ds.Tables[0].Rows[0]["LoseCount"].ToString() != "")
                {
                    this.LoseCount = int.Parse(ds.Tables[0].Rows[0]["LoseCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RunCount"] != null && ds.Tables[0].Rows[0]["RunCount"].ToString() != "")
                {
                    this.RunCount = int.Parse(ds.Tables[0].Rows[0]["RunCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HeroIdList"] != null)
                {
                    this.HeroIdList = ds.Tables[0].Rows[0]["HeroIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["FriendIdList"] != null)
                {
                    this.FriendIdList = ds.Tables[0].Rows[0]["FriendIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["AccountId"] != null && ds.Tables[0].Rows[0]["AccountId"].ToString() != "")
                {
                    this.AccountId = int.Parse(ds.Tables[0].Rows[0]["AccountId"].ToString());
                }
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public void GetModelByAccId(int accountId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,lv,Exp,Power,WinCount,LoseCount,RunCount,HeroIdList,FriendIdList,AccountId ");
            strSql.Append(" FROM [PlayerModel] ");
            strSql.Append(" where AccountId=@accountId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@accountId", SqlDbType.Int,4)};
            parameters[0].Value = accountId;

            DataSet ds = DbHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"] != null && ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    this.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Name"] != null)
                {
                    this.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["lv"] != null && ds.Tables[0].Rows[0]["lv"].ToString() != "")
                {
                    this.lv = int.Parse(ds.Tables[0].Rows[0]["lv"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Exp"] != null && ds.Tables[0].Rows[0]["Exp"].ToString() != "")
                {
                    this.Exp = int.Parse(ds.Tables[0].Rows[0]["Exp"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Power"] != null && ds.Tables[0].Rows[0]["Power"].ToString() != "")
                {
                    this.Power = int.Parse(ds.Tables[0].Rows[0]["Power"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WinCount"] != null && ds.Tables[0].Rows[0]["WinCount"].ToString() != "")
                {
                    this.WinCount = int.Parse(ds.Tables[0].Rows[0]["WinCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoseCount"] != null && ds.Tables[0].Rows[0]["LoseCount"].ToString() != "")
                {
                    this.LoseCount = int.Parse(ds.Tables[0].Rows[0]["LoseCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RunCount"] != null && ds.Tables[0].Rows[0]["RunCount"].ToString() != "")
                {
                    this.RunCount = int.Parse(ds.Tables[0].Rows[0]["RunCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HeroIdList"] != null)
                {
                    this.HeroIdList = ds.Tables[0].Rows[0]["HeroIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["FriendIdList"] != null)
                {
                    this.FriendIdList = ds.Tables[0].Rows[0]["FriendIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["AccountId"] != null && ds.Tables[0].Rows[0]["AccountId"].ToString() != "")
                {
                    this.AccountId = int.Parse(ds.Tables[0].Rows[0]["AccountId"].ToString());
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public void GetModelByName(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Name,lv,Exp,Power,WinCount,LoseCount,RunCount,HeroIdList,FriendIdList,AccountId ");
            strSql.Append(" FROM [PlayerModel] ");
            strSql.Append(" where Name=@Name ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.VarChar,16)};
            parameters[0].Value = name;

            DataSet ds = DbHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"] != null && ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    this.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Name"] != null)
                {
                    this.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["lv"] != null && ds.Tables[0].Rows[0]["lv"].ToString() != "")
                {
                    this.lv = int.Parse(ds.Tables[0].Rows[0]["lv"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Exp"] != null && ds.Tables[0].Rows[0]["Exp"].ToString() != "")
                {
                    this.Exp = int.Parse(ds.Tables[0].Rows[0]["Exp"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Power"] != null && ds.Tables[0].Rows[0]["Power"].ToString() != "")
                {
                    this.Power = int.Parse(ds.Tables[0].Rows[0]["Power"].ToString());
                }
                if (ds.Tables[0].Rows[0]["WinCount"] != null && ds.Tables[0].Rows[0]["WinCount"].ToString() != "")
                {
                    this.WinCount = int.Parse(ds.Tables[0].Rows[0]["WinCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LoseCount"] != null && ds.Tables[0].Rows[0]["LoseCount"].ToString() != "")
                {
                    this.LoseCount = int.Parse(ds.Tables[0].Rows[0]["LoseCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["RunCount"] != null && ds.Tables[0].Rows[0]["RunCount"].ToString() != "")
                {
                    this.RunCount = int.Parse(ds.Tables[0].Rows[0]["RunCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HeroIdList"] != null)
                {
                    this.HeroIdList = ds.Tables[0].Rows[0]["HeroIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["FriendIdList"] != null)
                {
                    this.FriendIdList = ds.Tables[0].Rows[0]["FriendIdList"].ToString();
                }
                if (ds.Tables[0].Rows[0]["AccountId"] != null && ds.Tables[0].Rows[0]["AccountId"].ToString() != "")
                {
                    this.AccountId = int.Parse(ds.Tables[0].Rows[0]["AccountId"].ToString());
                }
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM [PlayerModel] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelper.Query(strSql.ToString());
        }

        #endregion  Method
    }
}