using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace MOBAServer.Model
{
    /// <summary>
    /// 类AccountModel。
    /// </summary>
    [Serializable]
    public partial class AccountModel
    {
        public AccountModel() { }

        #region Model
        private int _id;
        private string _account;
        private string _password;
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
        public string Account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        #endregion Model


        #region  Method

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public AccountModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Account,Password ");
            strSql.Append(" FROM [AccountModel] ");
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
                if (ds.Tables[0].Rows[0]["Account"] != null)
                {
                    this.Account = ds.Tables[0].Rows[0]["Account"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Password"] != null)
                {
                    this.Password = ds.Tables[0].Rows[0]["Password"].ToString();
                }
            }
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [AccountModel]");
            strSql.Append(" where account=@account ");

            SqlParameter[] parameters = {
                    new SqlParameter("@account", SqlDbType.VarChar,32)};
            parameters[0].Value = account;

            return DbHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [AccountModel] (");
            strSql.Append("Account,Password)");
            strSql.Append(" values (");
            strSql.Append("@Account,@Password)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Account", SqlDbType.VarChar,32),
                    new SqlParameter("@Password", SqlDbType.VarChar,32)};
            parameters[0].Value = Account;
            parameters[1].Value = Password;

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
            strSql.Append("update [AccountModel] set ");
            strSql.Append("Account=@Account,");
            strSql.Append("Password=@Password");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Account", SqlDbType.VarChar,32),
                    new SqlParameter("@Password", SqlDbType.VarChar,32),
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Account;
            parameters[1].Value = Password;
            parameters[2].Value = Id;

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
            strSql.Append("delete from [AccountModel] ");
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
        public void GetModel(string account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,Account,Password ");
            strSql.Append(" FROM [AccountModel] ");
            strSql.Append(" where account=@account ");

            SqlParameter[] parameters = {
                    new SqlParameter("@account", SqlDbType.VarChar,32)};
            parameters[0].Value = account;

            DataSet ds = DbHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Id"] != null && ds.Tables[0].Rows[0]["Id"].ToString() != "")
                {
                    this.Id = int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Account"] != null)
                {
                    this.Account = ds.Tables[0].Rows[0]["Account"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Password"] != null)
                {
                    this.Password = ds.Tables[0].Rows[0]["Password"].ToString();
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
            strSql.Append(" FROM [AccountModel] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelper.Query(strSql.ToString());
        }

        #endregion  Method
    }
}