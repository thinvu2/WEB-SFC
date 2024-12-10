using SN_API.Models;
using SN_API.Models.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using static SN_API.Models.LineElement;

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config5Controller : ApiController
    {
        [System.Web.Http.Route("getRouteCode")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> getRouteCode(string database_name)
        {
            try
            {
                string getPrivilege = "";
                getPrivilege = $"SELECT EMP_NO ||'_'|| EMP_NAME AS EMP_NO_NAME FROM SFIS1.C_EMP_DESC_T";
                DataTable dtPrivilege = DBConnect.GetData(getPrivilege, database_name);
                if (dtPrivilege.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtPrivilege });
                }
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
    }
}