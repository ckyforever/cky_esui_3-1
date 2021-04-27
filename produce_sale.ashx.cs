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
    /// produce_sale 的摘要说明
    /// </summary>
    public class produce_sale : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //datamysql data = new datamysql(); 
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            context.Response.ContentType = "text/plain";
            if (context.Request.QueryString["type"] == "edit")//获取部门信息
            {
                int Id = int.Parse(context.Request.QueryString["Id"]);
                DataTable dt = datamysql.getdatatable("select Id,公司名称,销售时间,销售人员,送货地址 from  cky_产品销售主表  where Id=" + Id);

                if (dt != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(dt.Rows[0]["Id"].ToString() + ",");
                    sb.Append(dt.Rows[0]["公司名称"].ToString() + ",");
                    sb.Append(dt.Rows[0]["销售时间"].ToString() + ",");
                    sb.Append(dt.Rows[0]["销售人员"].ToString() + ",");
                    sb.Append(dt.Rows[0]["送货地址"].ToString() + ",");
                    //sb.Append(DateTime.Now.ToString());

                    context.Response.Write(sb.ToString());
                }
            }
            else if (context.Request.QueryString["type"] == "del")//删除部门信息
            {
                string Id = context.Request.QueryString["Id"];

                datamysql.ExecuteSql("delete from cky_产品销售主表  where Id=" + Id);

            }
            else if (context.Request.QueryString["type"] == "combox")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select Id,公司名称,销售时间,销售人员,送货地址 from  cky_产品销售主表 order by Id");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);

            }
            else if (context.Request.QueryString["type"] == "combox1")//保存修改或添加部门信息
            {
                DataSet ds = new DataSet();
                ds = datamysql.Query("select Id,公司名称,销售时间,销售人员,送货地址 from  cky_产品销售主表 order by Id");
                string strret = datamysql.ToJson_combox(ds);
                context.Response.Write(strret);
            }
            else if (context.Request.QueryString["type"] == "save")//保存修改或添加信息
            {
                //txt_name,combogrid_gg,txt_sl,txt_danweimingcheng,date_cs
                string strsql = "";
                string company_name = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["company_name"]) + "");
                string sale_time = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["sale_time"]) + "");
                string sale_people = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["sale_people"]) + "");
                string address = HttpUtility.UrlDecode(Convert.ToString(context.Request.Params["address"]) + "");

                if (context.Request.QueryString["Id"] != null && context.Request.QueryString["Id"] != "")
                {
                    int Id = int.Parse(context.Request.QueryString["Id"]);
                    strsql = "update cky_产品销售主表 set 公司名称='" + company_name + "',销售时间=" + sale_time + ",销售人员='" + sale_people + "',送货地址='" + address + "' where Id=" + Id;

                }
                else
                {
                    strsql = "insert into cky_产品销售主表(公司名称,销售时间,销售人员,送货地址) values('" + company_name + "'," + sale_time + ",'" + sale_people + "','" + address + "')";

                }
                datamysql.ExecuteSql(strsql);
                context.Response.Write("true");
            }

            else if (Convert.ToString(context.Request.Form["action"]) == "query")
            {
                //GetListByPageColumn_tojson(string columslist, string strWhere, string orderby, string datatablename)
                //string strret = datamysql.GetListByPageColumn_tojson("id,产品名称,库存数量,产品规格,备注", "", "库存数量", "姓名_产品基本信息表");
                string strret = datamysql.GetListByPageColumn_tojson(context, "Id,公司名称,销售时间,销售人员,送货地址", "", "Id", "cky_产品销售主表");
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