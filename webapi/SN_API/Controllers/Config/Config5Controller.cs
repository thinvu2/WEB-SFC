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

namespace SN_API.Controllers.Config
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Config5Controller : ApiController
    {
        [System.Web.Http.Route("getRouteCode")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> getRouteCode(string database_name, string routeCode)
        {
            try
            {
                //string strGetData = "";
                string strGetLineMain = "";
                string strGetLine_R = "";
                //strGetData = $"select * from SFIS1.C_ROUTE_CONTROL_T where route_code ='{routeCode}'";
                strGetLineMain = $"select GROUP_NEXT from (select GROUP_NEXT, min(STEP_SEQUENCE) as STEP_SEQUENCE  from SFIS1.C_ROUTE_CONTROL_T a where route_code ='{routeCode}' and STATE_FLAG ='0' and GROUP_NAME not like 'R_%'and GROUP_NEXT not like 'R_%' group by GROUP_NEXT order by STEP_SEQUENCE)";
                strGetLine_R = $"select GROUP_NAME, GROUP_NEXT from SFIS1.C_ROUTE_CONTROL_T where route_code ='{routeCode}' and STATE_FLAG = '1'";
                DataTable dtGetLineMain = DBConnect.GetData(strGetLineMain, database_name);
                DataTable dtGetLine_R = DBConnect.GetData(strGetLine_R, database_name);
                if (dtGetLineMain.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", dataLineMain = dtGetLineMain, dataLineR = dtGetLine_R });
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