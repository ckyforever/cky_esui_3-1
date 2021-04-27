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
    /// produce_data 的摘要说明
    /// </summary>
    public class produce_data : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //datamysql data = new datamysql(); 
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["type"] == "edit")//获取部门信息
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);

                DataTable dt = datamysql.getdatatable("select Id,产品名称,产品类型,库存数量,单位名称 from  cky_产品基本信息表  where Id=" + Id);

                if (dt != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dt.Rows[0]["Id"].ToString() + ",");
                    sb.Append(dt.Rows[0]["产品名称"].ToString() + ",");
                    sb.Append(dt.Rows[0]["产品类型"].ToString() + ",");
                    sb.Append(dt.Rows[0]["库存数量"].ToString() + ",");
                    sb.Append(dt.Rows[0]["单位名称"].ToString() + ",");
                    sb.Append(DateTime.Now.ToString());

                    context.Response.Write(sb.ToString());
                }
            }
            else if (context.Request.QueryString["type"] == "del")//删除部门信息
            {
                string Id = context.Request.QueryString["Id"];

                datamysql.ExecuteSql("delete from   cky_产品基本信息表  where id=" + Id);

            }
            else if (context.Request.QueryString["type"] == "combox")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select id,产品名称,产品类型,库存数量,单位名称 from  cky_产品基本信息表 order by  库存数量");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);

            }
            else if (context.Request.QueryString["type"] == "combox1")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select id,产品名称,产品类型,库存数量,单位名称 from  cky_产品基本信息表 order by  库存数量");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);
            }
            else if (context.Request.QueryString["type"] == "save")//保存修改或添加信息
            {
                //txt_name,combogrid_gg,txt_sl,txt_danweimingcheng,date_cs
                string strsql = "";
                string txt_name = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["txt_name"]) + "");
                string combogrid_gg = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["gg2"]) + "");
                string txt_sl = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["txt_sl"]) + "");
                string txt_danweimingcheng = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["gg"]) + "");

                if (context.Request.QueryString["Id"] != null && context.Request.QueryString["Id"] != "")
                {
                    int Id = int.Parse(context.Request.QueryString["Id"]);
                    strsql = "update cky_产品基本信息表 set 产品名称='" + txt_name + "',产品类型='" + combogrid_gg + "',单位名称='" + txt_danweimingcheng + "',库存数量=" + txt_sl + "   where Id=" + Id;

                }
                else
                {
                    strsql = "insert into cky_产品基本信息表(产品名称,产品类型,单位名称,库存数量) values('" + txt_name + "','" + combogrid_gg + "','" + txt_danweimingcheng + "'," + txt_sl + ")";

                }
                datamysql.ExecuteSql(strsql);
                context.Response.Write("true");
            }

            else if (Convert.ToString(context.Request.Form["action"]) == "query")
            {
                //GetListByPageColumn_tojson(string columslist, string strWhere, string orderby, string datatablename)
                //string strret = datamysql.GetListByPageColumn_tojson("id,产品名称,库存数量,产品规格,备注", "", "库存数量", "姓名_产品基本信息表");
                string strret = datamysql.GetListByPageColumn_tojson(context, "Id,产品名称,库存数量,产品类型,单位名称", "", "库存数量", "cky_产品基本信息表");
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