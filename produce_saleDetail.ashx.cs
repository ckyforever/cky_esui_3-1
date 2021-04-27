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
    /// produce_saleDetail 的摘要说明
    /// </summary>
    public class produce_saleDetail : IHttpHandler
    {
        datamysql me = new datamysql();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            context.Response.ContentType = "text/plain";
            //string main_id = "0";
            //if (context.Request.QueryString["main_id"] != null)
            //{
            //    main_id = context.Request.QueryString["main_id"].ToString();
            //}
            if (context.Request.QueryString["type"] == "edit")//获取部门信息
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                DataTable dt = datamysql.getdatatable("select Id,产品名称,主表id,销售数量,公司名称 from  cky_产品销售明细表  where Id=" + Id);

                if (dt != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dt.Rows[0]["Id"].ToString() + ",");
                    sb.Append(dt.Rows[0]["主表id"].ToString() + ",");
                    sb.Append(dt.Rows[0]["公司名称"].ToString() + ",");
                    sb.Append(dt.Rows[0]["产品名称"].ToString() + ",");
                    sb.Append(dt.Rows[0]["销售数量"].ToString() + ",");
                    sb.Append(DateTime.Now.ToString());

                    context.Response.Write(sb.ToString());
                }
            }
            else if (context.Request.QueryString["type"] == "del")//删除部门信息
            {
                string Id = context.Request.QueryString["Id"];
                string produce_name = datamysql.getvalue("select 产品名称 from  cky_产品销售明细表  where Id=" + Id);
                string sale_number_mx = datamysql.getvalue("select 销售数量 from  cky_产品销售明细表  where Id=" + Id);
                string strsql = "update cky_产品基本信息表 set 库存数量= 库存数量+" + sale_number_mx + " where 产品名称='" + produce_name + "'";
                datamysql.ExecuteSql(strsql);
                datamysql.ExecuteSql("delete  from  cky_产品销售明细表  where Id=" + Id);

            }
            else if (context.Request.QueryString["type"] == "combox")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select Id,产品名称,主表id,销售数量,公司名称 from  cky_产品销售明细表");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);

            }
            else if (context.Request.QueryString["type"] == "combox1")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select Id,产品名称,主表ID,销售数量,公司名称 from  cky_产品销售明细表");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);
            }
            else if (context.Request.QueryString["type"] == "save")//保存修改或添加信息
            {
                string strsql = "";
                string strsql2 = "";
                string company_name = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["combogrids_company_name"]) + "");
                string produce_name = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["combogrids_produce_name"]) + "");
                string sale_number = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["sale_number"]) + "");
                string main_id = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["main_id"]) + "");

                if (context.Request.QueryString["Id"] != null && context.Request.QueryString["Id"] != "")
                {
                    int id = int.Parse(context.Request.QueryString["Id"]);
                    string before = datamysql.getvalue("select 销售数量 from cky_产品销售明细表 where Id=" + id);
                    strsql = "update cky_产品销售明细表 set 产品名称='" + produce_name + "',主表ID='" + main_id + "',公司名称='" + company_name + "',销售数量=" + sale_number + "   where id=" + id;
                    strsql2 = "update cky_产品基本信息表 set 库存数量=库存数量+" + before + "-" + sale_number + "  where 产品名称 ='" + produce_name + "'";
                }
                else
                {
                    strsql = "insert into cky_产品销售明细表(产品名称,主表ID,公司名称,销售数量) values('" + produce_name + "','" + main_id + "','" + company_name + "'," + sale_number + ")";
                    strsql2 = "update cky_产品基本信息表 set 库存数量=库存数量-" + sale_number + " where 产品名称='" + produce_name + "'";

                }
                datamysql.ExecuteSql(strsql);
                datamysql.ExecuteSql(strsql2);
                context.Response.Write("true");
            }

            else if (Convert.ToString(context.Request.Form["action"]) == "query")
            {
                //GetListByPageColumn_tojson(string columslist, string strWhere, string orderby, string datatablename)
                //string strret = datamysql.GetListByPageColumn_tojson("id,产品名称,库存数量,产品规格,备注", "", "库存数量", "姓名_产品基本信息表");
                string main_id = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["main_id"]) + "");
                string strret = datamysql.GetListByPageColumn_tojson(context, "Id,产品名称,主表ID,销售数量,公司名称", "主表id=" + main_id, "Id", "cky_产品销售明细表");
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

        public string txt_ZId { get; set; }
    }
}