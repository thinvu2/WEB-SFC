using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using SN_API.Models;
using SN_API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SN_API.Controllers.Telit
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TelitEDIController : ApiController
    {
        [System.Web.Http.Route("GetDataTelitEDI")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetDataTelitEDI(string database_name, string F_PO, string dateFrom, string dateTo, string showTimeForm)
        {
            try
            {
                string strGetData = "";
                if (!string.IsNullOrEmpty(F_PO) && showTimeForm == "false")
                {
                    strGetData = $"SELECT DISTINCT PO as PURCHASE_ORDER, DECODE(PIP_TYPE,'850','NEW','CHANGE') as PO_TYPE, ITEM_INT as ITEM_NO, DOC_DATE as TIME, INCOTERMS, INCO_2, ITEM_PO, ITEM, DELET_IND,CHANGED_ON as CHANGE_TIME, QUANTITY, NET_PRICE, SHIPPING_NAME as SHIPPING_ADDRESS, FILENAME FROM SFISM4.R_TB_EDI_850_860 where PO = '{F_PO}'";
                }
                else if (string.IsNullOrEmpty(F_PO) && showTimeForm == "false")
                {
                    strGetData = $"SELECT DISTINCT PO as PURCHASE_ORDER, DECODE(PIP_TYPE,'850','NEW','CHANGE') as PO_TYPE, ITEM_INT as ITEM_NO, DOC_DATE as TIME, INCOTERMS, INCO_2, ITEM_PO, ITEM, DELET_IND,CHANGED_ON as CHANGE_TIME, QUANTITY, NET_PRICE, SHIPPING_NAME as SHIPPING_ADDRESS, FILENAME FROM SFISM4.R_TB_EDI_850_860";
                }
                else if (showTimeForm == "true" && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    strGetData = $"SELECT DISTINCT PO as PURCHASE_ORDER, DECODE(PIP_TYPE,'850','NEW','CHANGE') as PO_TYPE, ITEM_INT as ITEM_NO, DOC_DATE as TIME, INCOTERMS, INCO_2, ITEM_PO, ITEM, DELET_IND,CHANGED_ON as CHANGE_TIME, QUANTITY, NET_PRICE, SHIPPING_NAME as SHIPPING_ADDRESS, FILENAME FROM SFISM4.R_TB_EDI_850_860 WHERE to_char(F_ON,'YYYYMMDD') >= '{dateFrom}' and to_char(F_ON,'YYYYMMDD') <= '{dateTo}'";
                }
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }

        [System.Web.Http.Route("GetShowDetailTelitEDI")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetShowDetailTelitEDI(string database_name, string F_PO)
        {
            try
            {
                string strGetData = "";
                strGetData = $@"SELECT DISTINCT F_ID, SITE AS F_SITE, PIP_TYPE AS F_PIP_TYPE, '1' AS F_MSGID, SYSDATE  AS F_TIMESTAMP, VENDOR AS F_VENDOR, PO AS F_PO, ITEM AS F_PO_ITEM,
                                '' AS F_CONF_CTG, '' AS F_REFERENCE, SYSDATE AS F_CREATION_DATE, SYSDATE AS F_LASTEDIT_DT, FILENAME AS F_FILENAME, '' AS F_TIMES, ITEM_INT, DOC_DATE,
                                INCOTERMS, INCO_2, ITEM_PO, DELET_IND, CHANGED_ON, 
                                CASE 
                                WHEN substr(QUANTITY, 1, 1) = 0 then  TO_CHAR(QUANTITY, 'FM99990D99', 'NLS_NUMERIC_CHARACTERS = ''. ''' )
                                ELSE replace(TO_CHAR(QUANTITY), '.','') 
                                END as QUANTITY, 
                                NET_PRICE, SHIPPING_NAME FROM SFISM4.R_TB_EDI_850_860 where PO = '{F_PO}'";
               
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck});
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [Route("InsertTelitEDI")]
        [HttpPost]
        public async Task<HttpResponseMessage> InsertTelitEDI([FromBody] JObject model)
        {
            try
            {
                string emp_no = model["EMP_NO"].ToString();
                string databaseName = model["database_name"].ToString();
                string strPrivilege = $"SELECT emp FROM  SFIS1.C_PRIVILEGE  WHERE PRG_NAME='WEB_CONFIG' AND FUN = 'TELIT_INSERT' AND EMP='{emp_no}'";

                if (DBConnect.GetData(strPrivilege, databaseName).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "This emp has no privilege" });
                }

                JArray data = (JArray)model["data"];
                JArray additionalRows = (JArray)model["additionalRows"];
                foreach (var rows in additionalRows)
                {
                    foreach (var item in data)
                    {
                        try
                        {
                            await HandleTelitEDI(emp_no,rows, item, databaseName);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error: {ex.Message}");
                            Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = ex.Message });
            }
        }
        private async Task HandleTelitEDI(string emp_no, JToken rows, JToken item, string databaseName)
        {
            try
            {//16 rows
                //var fID = item["F_ID"]?.ToString() ?? string.Empty;
                var fSite = item["F_SITE"]?.ToString() ?? string.Empty;
                var fPipType = item["F_PIP_TYPE"]?.ToString() ?? string.Empty;
                var fMsgId = item["F_MSGID"]?.ToString() ?? string.Empty;
                var fTimesTamp = item["F_TIMESTAMP"]?.ToString() ?? string.Empty;
                var fVendor = item["F_VENDOR"]?.ToString() ?? string.Empty;
                var fPo = item["F_PO"]?.ToString() ?? string.Empty;
                var fPoItem = item["F_PO_ITEM"]?.ToString() ?? string.Empty;
                var fConfCtg = item["F_CONF_CTG"]?.ToString() ?? string.Empty;
                var fReference = item["F_REFERENCE"]?.ToString() ?? string.Empty;
                var fCreationDate = item["F_CREATION_DATE"]?.ToString() ?? string.Empty;
                var fLasteditDt = item["F_LASTEDIT_DT"]?.ToString() ?? string.Empty;
                var fFileName = item["F_FILENAME"]?.ToString() ?? string.Empty;
                var fTimes = item["F_TIMES"]?.ToString() ?? string.Empty;
                var minTime = rows["minTime"]?.ToString() ?? string.Empty;
                var scheduleQty = rows["scheduleQty"]?.ToString() ?? string.Empty;
                string insertLog = $@"insert into SFISM4.R_SYSTEM_LOG_T
                                    (EMP_NO, PRG_NAME, ACTION_TYPE, ACTION_DESC, TIME) 
                                    values
                                    ('{emp_no}', 'TELIT_EDI', 'INSERT', :actionDesc, sysdate)";
                var actionDesc = $"Po: {fPo}, Del_Date: {minTime}, Schedule_qty: {scheduleQty}";
                string insertTbEdi855 = $@" INSERT INTO SFISM4.R_TB_EDI_855 (
                                F_ID, F_SITE, F_PIP_TYPE, F_MSGID, F_TIMESTAMP, F_VENDOR, F_PO, F_PO_ITEM, F_CONF_CTG, F_REFERENCE, F_CREATION_DATE, F_LASTEDIT_DT, F_FILENAME, F_TIMES, F_DEL_DATE, F_QTY) 
                                VALUES ((SELECT NVL(MAX(F_ID), 0) + 1 FROM SFISM4.R_TB_EDI_855), :fSite, :fPipType, :fMsgId, systimestamp, :fVendor, :fPo, :fPoItem, :fConfCtg, :fReference, sysdate, 
                                systimestamp, :fFileName, :fTimes, to_date(:minTime,'YYYY/MM/DD HH24:MI:SS'), :scheduleQty)";
                var connectionString = new GetString().Get()[databaseName];
                 using(var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                     using(var command = new OracleCommand(insertTbEdi855, connection))
                    {
                        //command.Parameters.Add(new OracleParameter("fID", fID));
                        command.Parameters.Add(new OracleParameter("fSite", fSite));
                        command.Parameters.Add(new OracleParameter("fPipType", fPipType));
                        command.Parameters.Add(new OracleParameter("fMsgId", fMsgId));
                        //TIMESTAMP
                        command.Parameters.Add(new OracleParameter("fVendor", fVendor));
                        command.Parameters.Add(new OracleParameter("fPo", fPo));
                        command.Parameters.Add(new OracleParameter("fPoItem", fPoItem));
                        command.Parameters.Add(new OracleParameter("fConfCtg", fConfCtg));
                        command.Parameters.Add(new OracleParameter("fReference", fReference));
                        //sysdate
                        //TIMESTAMP
                        command.Parameters.Add(new OracleParameter("fFileName", fFileName));
                        command.Parameters.Add(new OracleParameter("fTimes", fTimes));
                        command.Parameters.Add(new OracleParameter("minTime", minTime));
                        command.Parameters.Add(new OracleParameter("scheduleQty", scheduleQty));
                        await command.ExecuteNonQueryAsync();
                    }
                    using (var commandLog = new OracleCommand(insertLog, connection))
                    {
                        commandLog.Parameters.Add(new OracleParameter("actionDesc", actionDesc));
                        await commandLog.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
