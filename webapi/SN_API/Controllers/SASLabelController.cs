using SN_API.Models;
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

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SASLabelController : ApiController
    {
        [System.Web.Http.Route("GetModelName_SaS")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetModelName_SaS(Config19Element model)
        {
            string strGetData = "select MODEL_NAME from SFIS1.C_MODEL_DESC_T ";
            DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        }
    }
}