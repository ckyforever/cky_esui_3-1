using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Webjdatademo;

namespace cky_esui_3_1
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["name"] != null)
            {

                string str = read(context.Request.QueryString["name"].ToString(), context.Request.QueryString["password"].ToString(), context);
                context.Response.Write(str);
            }
        }
        private string read(string name, string password, HttpContext context)
        {
            string strret = "error";
            //     读取并判断登录人密码
            string strsql = "";
            DataSet ds1 = new DataSet();
            strsql = "SELECT  用户姓名  FROM  cky_管理员信息表  where 用户姓名='" + name.Replace(" ", "").Replace("'", "") + "' and 用户密码='" + password.Replace(" ", "").Replace("'", "") + "'";
            ds1 = datamysql.Query(strsql);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                //               context.Session["登录名称"] = ds1.Tables[0].Rows[0]["登录名称"].ToString().Trim();
                strret = "OK";
            }
            //strret = "OK";//简化。
            return strret;
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