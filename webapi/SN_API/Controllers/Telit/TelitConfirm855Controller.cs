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
    public class TelitConfirm855Controller : ApiController
    {
        [System.Web.Http.Route("GetDataConfirm855")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetDataConfirm855(string databaseName, string F_PO)
        {
            try
            {
                string strGetData = "";
                if (string.IsNullOrEmpty(F_PO))
                {
                    strGetData = $"select distinct F_PO, F_PIP_TYPE, F_TIMESTAMP, F_VENDOR, F_PO_ITEM, F_CONF_CTG, F_DEL_DATE, F_QTY, F_REFERENCE, F_CREATION_DATE, F_LASTEDIT_DT, F_FILENAME, F_TIMES from SFISM4.R_TB_EDI_855 order by F_LASTEDIT_DT desc";
                }
                else
                {
                    strGetData = $"select distinct F_PO, F_PIP_TYPE, F_TIMESTAMP, F_VENDOR, F_PO_ITEM, F_CONF_CTG, F_DEL_DATE, F_QTY, F_REFERENCE, F_CREATION_DATE, F_LASTEDIT_DT, F_FILENAME, F_TIMES from SFISM4.R_TB_EDI_855 WHERE F_PO = '{F_PO}' ";
                }
                DataTable dtCheck = DBConnect.GetData(strGetData, databaseName);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [System.Web.Http.Route("GetShowDetailConfirm855")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetShowDetailConfirm855(string databaseName, string F_PO, string F_PO_ITEM, string F_PIP_TYPE)
        {
            try
            {
                string strGetData = "";
                string strGetDataInput = "";
                strGetData = $@"SELECT F_PO, F_SITE, F_PIP_TYPE, F_MSGID, F_TIMESTAMP, F_VENDOR, F_PO_ITEM, F_CONF_CTG, F_REFERENCE, F_TIMES, F_FILENAME, F_LASTEDIT_DT FROM SFISM4.R_TB_EDI_855 where F_PO = '{F_PO}' and F_PO_ITEM = '{F_PO_ITEM}' and F_PIP_TYPE = '{F_PIP_TYPE}' ";
                strGetDataInput = $"SELECT F_ID, F_DEL_DATE, F_QTY FROM SFISM4.R_TB_EDI_855 WHERE F_PO = '{F_PO}' AND F_PO_ITEM = '{F_PO_ITEM}' and F_PIP_TYPE = '{F_PIP_TYPE}' ";
                DataTable dtCheck = DBConnect.GetData(strGetData, databaseName);
                DataTable dtCheckInput = DBConnect.GetData(strGetDataInput, databaseName);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dataInput = dtCheckInput });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        [Route("InsertConfirm855")]
        [HttpPost]
        public async Task<HttpResponseMessage> InsertConfirm855([FromBody] JObject model)
        {
            try
            {
                string emp_no = model["empNo"].ToString();
                string databaseName = model["databaseName"].ToString();
                string strPrivilege = $"SELECT emp FROM  SFIS1.C_PRIVILEGE  WHERE PRG_NAME='WEB_CONFIG' AND FUN = 'TELIT_CONFIRM' AND EMP='{emp_no}'";

                if (DBConnect.GetData(strPrivilege, databaseName).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "This emp has no privilege" });
                }

                JArray data = (JArray)model["data"];
                JArray additionalRows = (JArray)model["additionalRows"];
                List<string> logF_ID = new List<string>();

                string connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // get ROWID
                            string selectQuery = @"SELECT ROWIDTOCHAR(A.ROWID) as ROWID_TOCHAR 
                                           FROM SFISM4.R_TB_EDI_855 a 
                                           WHERE F_PO=:fPo AND F_PO_ITEM=:fPoItem";
                            foreach (var item in data)
                            {
                                string fPo = item["F_PO"]?.ToString() ?? string.Empty;
                                string fPoItem = item["F_PO_ITEM"]?.ToString() ?? string.Empty;

                                using (var selectCommand = new OracleCommand(selectQuery, connection))
                                {
                                    selectCommand.Parameters.Add(new OracleParameter("fPo", fPo));
                                    selectCommand.Parameters.Add(new OracleParameter("fPoItem", fPoItem));

                                    using (var reader = await selectCommand.ExecuteReaderAsync())
                                    {
                                        while (await reader.ReadAsync())
                                        {
                                            logF_ID.Add(reader["ROWID_TOCHAR"].ToString());
                                        }
                                    }
                                }
                            }

                            // insert data
                            foreach (var rows in additionalRows)
                            {
                                foreach (var item in data)
                                {
                                    await HandleTelitConfirm855(emp_no, rows, item, databaseName);
                                }
                            }

                            // delete ROWID
                            string deleteQuery = "DELETE FROM SFISM4.R_TB_EDI_855 WHERE ROWID = :item";
                            foreach (var item in logF_ID)
                            {
                                using (var deleteCommand = new OracleCommand(deleteQuery, connection))
                                {
                                    deleteCommand.Parameters.Add(new OracleParameter("item", item));
                                    await deleteCommand.ExecuteNonQueryAsync();
                                }
                            }
                           // ORA - 01745: invalid host/ bind variable name

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            //Debug.WriteLine($"Error: {ex.Message}");
                            //Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = ex.Message });
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Error: {ex.Message}");
                //Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { result = ex.Message });
            }
        }
        private async Task HandleTelitConfirm855(string emp_no, JToken rows, JToken item, string databaseName)
        {
            try
            {//16 rows
                var fSite = item["F_SITE"]?.ToString() ?? string.Empty;
                var fPipType = item["F_PIP_TYPE"]?.ToString() ?? string.Empty;
                var fMsgId = item["F_MSGID"]?.ToString() ?? string.Empty;
                //TIMESTAMP
                var fVendor = item["F_VENDOR"]?.ToString() ?? string.Empty;
                var fPo = item["F_PO"]?.ToString() ?? string.Empty;
                var fPoItem = item["F_PO_ITEM"]?.ToString() ?? string.Empty;
                var fConfCtg = item["F_CONF_CTG"]?.ToString() ?? string.Empty;
                var fReference = item["F_REFERENCE"]?.ToString() ?? string.Empty;
                //sysdate
                //TIMESTAMP
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

                string insertTbEdi855Details = $@" INSERT INTO SFISM4.R_TB_EDI_855_DETAIl (
                                F_ID, F_SITE, F_PIP_TYPE, F_MSGID, F_TIMESTAMP, F_VENDOR, F_PO, F_PO_ITEM, F_CONF_CTG, F_REFERENCE, F_CREATION_DATE, F_LASTEDIT_DT, F_FILENAME, F_TIMES, F_DEL_DATE, F_QTY) 
                                VALUES ((SELECT NVL(MAX(F_ID), 0) + 1 FROM SFISM4.R_TB_EDI_855_DETAIl), :fSite, :fPipType, :fMsgId, systimestamp, :fVendor, :fPo, :fPoItem, :fConfCtg, :fReference, sysdate, 
                                systimestamp, :fFileName, :fTimes, to_date(:minTime,'YYYY/MM/DD HH24:MI:SS'), :scheduleQty)";
                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using(var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new OracleCommand(insertTbEdi855, connection))
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

                            using (var command = new OracleCommand(insertTbEdi855Details, connection))
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
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Error: {ex.Message}");
                //Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

    }
}
