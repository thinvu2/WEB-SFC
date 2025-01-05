using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using SN_API.Models;
using SN_API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SN_API.Controllers.QualcommApps
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QWipTransactionController : ApiController
    {
        [System.Web.Http.Route("GetLoadFormQWip")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetLoadFormQWip(string database_name, string trans_name, string lot_no)
        {
            try
            {
                string strGetData = "";
                if (string.IsNullOrEmpty(lot_no))
                {
                    strGetData = $"SELECT  * FROM SFISM4.R_QWIP_DATA where upper(TRANS_NAME) = '{trans_name.ToUpper()}'";
                }
                else
                {
                    strGetData = $"SELECT  * FROM SFISM4.R_QWIP_DATA where upper(TRANS_NAME) = '{trans_name.ToUpper()}' and upper(lot_no) = '{lot_no.ToUpper()}'";
                }
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
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
        [System.Web.Http.Route("GetLoadLotNoQWip")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetLoadLotNoQWip(string database_name)
        {
            try
            {
                string strGetData = "";
                strGetData = $"SELECT  distinct mo_number as RECEIVELOT from SFISM4.R_BPCS_MOPLAN_T";
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
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
        [System.Web.Http.Route("InsertQWipTrans")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertQWipTrans(Qwip_Element model)
        {
            try
            {
                string strGetData = "";
                string checkExistsProcedure = "SFIS1.SP_ADD_TRANS";
                string procedureResult = "";
                List<Dictionary<string, object>> cursorData = new List<Dictionary<string, object>>();
                var connectionString = new GetString().Get()[model.database_name];
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = new OracleCommand(checkExistsProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("IN_TRANSACTIONS", OracleDbType.Varchar2).Value = model.transaction;
                        command.Parameters.Add("IN_REASON", OracleDbType.Varchar2).Value = model.reason;
                        command.Parameters.Add("IN_LOTNO", OracleDbType.Varchar2).Value = model.lot_no;
                        command.Parameters.Add("IN_STAGE_OUT", OracleDbType.Varchar2).Value = model.IN_STAGE_OUT;
                        command.Parameters.Add("IN_STAGE_IN", OracleDbType.Varchar2).Value = model.IN_STAGE_IN;
                        command.Parameters.Add("IN_QTY", OracleDbType.Int32).Value = Int32.Parse(model.qty);
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        procedureResult = command.Parameters["RES"].Value.ToString();

                    }
                }
                if (procedureResult == "OK")
                {
                    strGetData = $"SELECT  * FROM SFISM4.R_QWIP_DATA where upper(TRANS_NAME) = '{model.transaction.ToUpper()}' and upper(lot_no) = '{model.lot_no.ToUpper()}'";
                    DataTable dtCheck = DBConnect.GetData(strGetData, model.database_name);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = procedureResult.ToLower(), data = dtCheck });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = procedureResult });
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

        public class Qwip_Element
        {
            public string database_name { get; set; }
            public string lot_no { get; set; }
            public string reason { get; set; }
            public string transaction { get; set; }
            public string qty { get; set; }
            public string IN_STAGE_OUT { get; set; }
            public string IN_STAGE_IN { get; set; }
        }
    }
}