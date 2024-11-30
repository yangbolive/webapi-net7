using SqlSugar;
using System.Data;
using webapi.net7.sqlsugar.Model;
using static Org.BouncyCastle.Math.EC.ECCurve;
using DbType = SqlSugar.DbType;

namespace webapi.net7.sqlsugar
{
    public class SqlSugarHelp
    {
        //static  U8Login.clsLogin? wlzhlogin = Login.wlzh_login("888", "2023-10-21", "192.168.0.121", "ssgw", "@wlzh1234");



        //数据库链接字符串_富岭
        public static string con = JsonHelper.GetAppSettings("Connectionstrings_fl:DBConnection");
        //数据库连接字符串_房源管理系统
        public static string con1 = JsonHelper.GetAppSettings("Connectionstrings:DBConnection");
        #region mysql连接代码 
        //连接本机mysql数据库
        public static string conset = "server=127.0.0.1;user id=root;password=root;database=diygw";

        public static SqlSugarClient dbmysql = new SqlSugarClient(new ConnectionConfig()
        {

            ConnectionString = conset,
            DbType = DbType.MySql,
            IsAutoCloseConnection = true//自动释放
        });

        /// <summary>
        /// 通过sql语句查询生成table数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable mySqlSugarTable(string sql)
        {


            var dt = dbmysql.Ado.GetDataTable(sql);
            return dt;

        }
        #endregion
        //创建数据库对象 SqlSugarClient 连接富岭  
        public static SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            
            ConnectionString = con,
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true//自动释放
        });

        //创建数据库对象 SqlSugarClient 连接房源管理系统
        public static SqlSugarClient db1 = new SqlSugarClient(new ConnectionConfig()
        {

            ConnectionString = con1,
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true//自动释放
        });
        //创建数据库对象 SqlSugarClient 连接ufdata
        public static SqlSugarClient dbdata = new SqlSugarClient(new ConnectionConfig()
        {

            ConnectionString = con,
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true//自动释放
        });
        //写代码就不需要考虑 open close 直接用就行了

        //无实体无返回增删改查
        public static void SqlSugarExecuteCommand(string sql)
        {
            db.Ado.ExecuteCommand(sql);
            

        }
        //无实体无返回增删改查
        public static void SqlSugarExecuteCommand1(string sql)
        {
            db1.Ado.ExecuteCommand(sql);


        }

        /// <summary>
        /// 通过sql语句查询生成table数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable SqlSugarTable(string sql)
        {


            var dt = db1.Ado.GetDataTable(sql);
            return dt;

        }

        /// <summary>
        /// 通过sql语句查询生成table数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable SqlSugarTableFL(string sql)
        {


            var dt = db.Ado.GetDataTable(sql);
            return dt;

        }


        /// <summary>
        /// 存储过程不确定参数传参获取table数据
        /// </summary>
        /// <param name="procname"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static DataTable SqlSugarTableProc(string procname, params Parameter[] ps )
        {

            var dt = db.Ado.UseStoredProcedure().GetDataTable(procname, ps);

            return dt;

        }


        /// <summary>
        /// 生成数据库中全部实体类
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="spacename"></param>
        public static void SqlSugarClient(string patch, string spacename)
        {
            
            
            db.DbFirst.IsCreateAttribute().CreateClassFile(patch, spacename);
            //参数1：路径  参数2：命名空间
            //IsCreateAttribute 代表生成SqlSugar特性
        }

       /// <summary>
       /// 生成指定名称的实体类
       /// </summary>
       /// <param name="tablename"></param>
       /// <param name="ph"></param>
       /// <param name="spacename"></param>
        public static void SqlSugarClient(string tablename,string ph,string spacename)
        {

            // 2.生成实体并且带有筛选
            db.DbFirst.Where(tablename).CreateClassFile( ph,spacename);
            //db.DbFirst.Where(it => it.ToLower().StartsWith("view")).CreateClassFile("c:\\Demo\\3", "Models");
            //db.DbFirst.Where(it => it.ToLower().StartsWith("view")).CreateClassFile("c:\\Demo\\4", "Models");

        }


        /// <summary>
        /// 查询对象返回表结果数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static List<T> SqlSugarList<T>(T tablename) where T : class
        {

            List<T> list = db.Queryable<T>().ToList();
            return list;

        }
        /// <summary>
        /// 对象查询数据库表带条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="id"></param>
        /// <param name="sbzt">设备状态，暂时作废</param>
        /// <returns></returns>
        public static List<T> SqlSugarListTop1<T>(T tablename, string id, string sbzt) where T : wlzh_v_device_record
        {
          
                var list = db.Queryable<T>().Where(it => it.id == id).ToList();
                return list;


        }


        /// <summary>
        /// 对象查询数据库表带条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="id"></param>
        /// <param name="sbzt">设备状态，暂时作废</param>
        /// <returns></returns>
        public static List<T> SqlSugarListTopversion_app<T>(T tablename) where T : version_app
        {

            var list = db.Queryable<T>().Where(it => it.description == "app版本").ToList();
            return list;


        }
        /// <summary>
        /// 查询人员对应设备班次
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="jtname"></param>
        /// <param name="bz"></param>
        /// <returns></returns>
        public static List<T> SqlSugarListPerson<T>(T tablename, string jtname) where T : wlzh_DevicePerson
        {

            var list = db.Queryable<T>().Where(it => it.jtname == jtname ).ToList();
            return list;


        }
        /// <summary>
        /// 扫码获取生产订单信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<T> SqlSugarListProduction<T>(T tablename,string code) where T : wlzh_v_Production
        {

            var list = db.Queryable<T>().Where(it => it.cbsysbarcode == code).ToList();
            return list;


        }

        /// <summary>
        /// 扫码获取生产订单信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<T> SqlSugarListProductInspectionList<T>(T tablename, string code) where T : wlzh_v_ProductInspectionList
        {

            var list = db.Queryable<T>().Where(it => it.tm == code).OrderBy(it=> SqlFunc.Desc(it.ccode)).ToList();
            return list;


        }

        /// <summary>
        /// 扫码获取生产订单信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<T> SqlSugarListProductInspectionListSY<T>(T tablename, string code) where T : wlzh_v_ProductInspectionList_sy
        {

            var list = db.Queryable<T>().Where(it => it.tm == code).ToList();
            return list;


        }

        /// <summary>
        /// 扫码获取检验单列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<T> SqlSugarListInspectionForm<T>(T tablename, string code) where T : wlzh_v_InspectionForm
        {

            var list = db.Queryable<T>().Where(it => it.tm == code).OrderBy(it=>SqlFunc.Desc(it.jyrq)).ToList();
            return list;


        }
        /// <summary>
        /// 查询所有人员档案
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablename"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<T> SqlSugarListAllPerson<T>(T tablename, string id) where T : wlzh_v_person
        {

            var list = db.Queryable<T>().Where(it => it.cdepcode.Contains(id) || it.cdepname.Contains(id) || it.cPsn_Num.Contains(id) || it.cpsn_name.Contains(id)).ToList(); ;
            return list;


        }
        /// <summary>
        /// 实体对象直接转json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string SqlSugarJson<T>(T table) where T : class
        {

            var json = db.Queryable<T>().ToJson();
            return json.ToString();

        }




    }
}
