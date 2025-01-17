﻿using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using SN_API.Models.Config;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfigController : ApiController
    {
        // GET: Config
        [System.Web.Http.Route("GetConfigPrivilge")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigPrivilge(ConfigElement model)
        {
            string strGetData = $" select PRIVILEGE,FUN from sfis1.C_PRIVILEGE  where EMP = '{model.emp_no}' AND PRG_NAME = 'CONFIG' AND PRIVILEGE = 2 ";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("CheckConfigPrivilege")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CheckConfigPrivilege(ConfigElement model)
        {
            string strGetData = $" select PRIVILEGE,FUN from sfis1.C_PRIVILEGE  where EMP = '{model.emp_no}' AND PRG_NAME = 'CONFIG' AND PRIVILEGE = 2 and fun LIKE '{model.fun}%' and rownum =1 ";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }


        [System.Web.Http.Route("GetConfigPrivilge_Group")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetConfigPrivilge_Group(ConfigElement model)
        {
            string strGetData = $" select PRIVILEGE,FUN from sfis1.C_PRIVILEGE  where EMP = '{model.emp_no}' AND PRG_NAME = 'CONFIG' AND PRIVILEGE = 2 ";
            //  string strGetData = $"select * from table(PKG_RETURN_TABLE.F_GETLISTRESOURCE('{model.emp_no}')) ";

            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        [System.Web.Http.Route("GetEmp_ClassName")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetEmp_ClassName(ConfigElement model)
        {
            string strGetData = $" SELECT * FROM SFIS1.C_EMP_DESC_T where (CLASS_NAME like '%FTI%' or CLASS_NAME like '%BFIH%') and EMP_NO = '{model.emp_no}' ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("GetPrivilegeShowIcon")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetPrivilegeShowIcon(string database_name, string emp_no)
        {
            try
            {
                string strGetData = $"select PASSW from SFIS1.C_PRIVILEGE where emp = '{emp_no}' and fun ='PRIVILEGE' and PRG_NAME ='WEB_CONFIG' and rownum = 1";
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [System.Web.Http.Route("GetPrivilegeLeftNav")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetPrivilegeLeftNav(string databaseName, string empNo)
        {
            try
            {
                string strGetData = $@"select OWNER from SFIS1.C_EMP_DESC_T  where emp_no = '{empNo}' and rownum = 1";
                DataTable dtCheck = DBConnect.GetData(strGetData, databaseName);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }

    }
}