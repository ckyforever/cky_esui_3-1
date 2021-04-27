using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Webjdatademo;

namespace cky_esui_3_1
{
    /// <summary>
    /// admin_data 的摘要说明
    /// </summary>
    public class admin_data : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //datamysql data = new datamysql(); 
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["type"] == "edit")//获取部门信息
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                DataTable dt = datamysql.getdatatable("select Id,用户姓名,用户密码,联系电话,添加时间 from  cky_管理员信息表  where Id=" + Id);

                if (dt != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dt.Rows[0]["Id"].ToString() + ",");
                    sb.Append(dt.Rows[0]["用户姓名"].ToString() + ",");
                    sb.Append(dt.Rows[0]["用户密码"].ToString() + ",");
                    sb.Append(dt.Rows[0]["联系电话"].ToString() + ",");
                    sb.Append(dt.Rows[0]["添加时间"].ToString() + ",");
                    //sb.Append(DateTime.Now.ToString());

                    context.Response.Write(sb.ToString());
                }
            }
            else if (context.Request.QueryString["type"] == "del")//删除部门信息
            {
                string Id = context.Request.QueryString["Id"];

                datamysql.ExecuteSql("delete from cky_管理员信息表  where Id=" + Id);

            }
            else if (context.Request.QueryString["type"] == "combox")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select Id,用户姓名,用户密码,联系电话,添加时间 from  cky_管理员信息表 order by Id");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);

            }
            else if (context.Request.QueryString["type"] == "combox1")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select Id,用户姓名,用户密码,联系电话,添加时间 from  cky_管理员信息表 order by  Id");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);
            }
            else if (context.Request.QueryString["type"] == "save")//保存修改或添加信息
            {
                //txt_name,combogrid_gg,txt_sl,txt_danweimingcheng,date_cs
                string strsql = "";
                string user_name = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["user_name"]) + "");
                string password = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["password"]) + "");
                string iphone = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["iphone"]) + "");
                string add_time = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["add_time"]) + "");

                if (context.Request.QueryString["Id"] != null && context.Request.QueryString["Id"] != "")
                {
                    int Id = int.Parse(context.Request.QueryString["Id"]);
                    strsql = "update cky_管理员信息表 set 用户姓名='" + user_name + "',用户密码='" + password + "',联系电话='" + iphone + "',添加时间=" + add_time + "   where id=" + Id;

                }
                else
                {
                    strsql = "insert into cky_管理员信息表(用户姓名,用户密码,联系电话,添加时间) values('" + user_name + "','" + password + "','" + iphone + "'," + add_time + ")";

                }
                datamysql.ExecuteSql(strsql);
                context.Response.Write("true");
            }

            else if (Convert.ToString(context.Request.Form["action"]) == "query")
            {
                //GetListByPageColumn_tojson(string columslist, string strWhere, string orderby, string datatablename)
                //string strret = datamysql.GetListByPageColumn_tojson("id,产品名称,库存数量,产品规格,备注", "", "库存数量", "姓名_产品基本信息表");
                string strret = datamysql.GetListByPageColumn_tojson(context, "Id,用户姓名,用户密码,联系电话,添加时间", "", "Id", "cky_管理员信息表");
                context.Response.Write(strret);
                return;
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}