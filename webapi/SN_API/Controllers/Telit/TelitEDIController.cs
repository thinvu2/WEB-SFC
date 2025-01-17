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
        public async Task<HttpResponseMessage> GetDataTelitEDI(string database_name, string F_PO)
        {
            try
            {
                string strGetData = "";
                if(string.IsNullOrEmpty(F_PO))
                {
                    strGetData = $"SELECT PO, SITE, PIP_TYPE, COCODE, DOC_TYPE, F_ON, CREATED, ITEM_INT, LAST_ITEM, VENDOR, VENDOR_NAME, VENDOR_STREET, VENDOR_CITY, VENDOR_COUNTRY, VENDOR_PHONE, VENDOR_FAX, F_LANGUAGE, PAYT_TERMS, PURCH_ORG, PUR_GROUP, CURRENCY, EXCH_RATE, DOC_DATE, INCOTERMS, INCO_2, VAT_REG_NO, F_VERSION, CONTACT_PERSON_NAME, CONTACT_PERSON_PHONE, CONTACT_PERSON_FAX, CONTACT_PERSON_EMAIL, HEADER_TEXT, ITEM_PO, ITEM, DELET_IND, CHANGED_ON, SHORT_TEXT, MATERIAL, ITEM_COCODE, PLANT, STOR_LOC, MATL_GROUP, QUANTITY, ORDER_UNIT, ORDERPR_UN, NET_PRICE, F_PER, NET_VALUE, GROSS_VAL, F_VALUE, ADDRESS, SHIPPING_NAME, SHIPPING_STREET, SHIPPING_CITY, SHIPPING_COUNTRY, ITEM_TEXT, SCHED_PURCH_DOC, SCHED_ITEM, SCHED_LINE, SCHED_DELIV_DATE, SCHED_QTY, ACK1_FILE_NAME, ACK2_FILE_NAME, LASTEDIT_DT, FILENAME FROM SFISM4.R_TB_EDI_850_860 where (PO, ITEM) not in (select F_PO, F_PO_ITEM from SFISM4.R_TB_EDI_855)";
                }
                else
                {
                    strGetData = $"SELECT PO, SITE, PIP_TYPE, COCODE, DOC_TYPE, F_ON, CREATED, ITEM_INT, LAST_ITEM, VENDOR, VENDOR_NAME, VENDOR_STREET, VENDOR_CITY, VENDOR_COUNTRY, VENDOR_PHONE, VENDOR_FAX, F_LANGUAGE, PAYT_TERMS, PURCH_ORG, PUR_GROUP, CURRENCY, EXCH_RATE, DOC_DATE, INCOTERMS, INCO_2, VAT_REG_NO, F_VERSION, CONTACT_PERSON_NAME, CONTACT_PERSON_PHONE, CONTACT_PERSON_FAX, CONTACT_PERSON_EMAIL, HEADER_TEXT, ITEM_PO, ITEM, DELET_IND, CHANGED_ON, SHORT_TEXT, MATERIAL, ITEM_COCODE, PLANT, STOR_LOC, MATL_GROUP, QUANTITY, ORDER_UNIT, ORDERPR_UN, NET_PRICE, F_PER, NET_VALUE, GROSS_VAL, F_VALUE, ADDRESS, SHIPPING_NAME, SHIPPING_STREET, SHIPPING_CITY, SHIPPING_COUNTRY, ITEM_TEXT, SCHED_PURCH_DOC, SCHED_ITEM, SCHED_LINE, SCHED_DELIV_DATE, SCHED_QTY, ACK1_FILE_NAME, ACK2_FILE_NAME, LASTEDIT_DT, FILENAME FROM SFISM4.R_TB_EDI_850_860 WHERE PO = '{F_PO}' and (PO, ITEM) not in (select F_PO, F_PO_ITEM from SFISM4.R_TB_EDI_855)";
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
        public async Task<HttpResponseMessage> GetShowDetailTelitEDI(string database_name, string F_PO, string F_ITEM_INT, string F_PIP_TYPE)
        {
            try
            {
                string strGetData = "";
                strGetData = $@"SELECT DISTINCT F_ID, SITE AS F_SITE, PIP_TYPE AS F_PIP_TYPE, '1' AS F_MSGID, SYSDATE  AS F_TIMESTAMP, VENDOR AS F_VENDOR, PO AS F_PO, ITEM AS F_PO_ITEM,
                                'AB' AS F_CONF_CTG, '' AS F_REFERENCE, SYSDATE AS F_CREATION_DATE, SYSDATE AS F_LASTEDIT_DT, FILENAME AS F_FILENAME, '' AS F_TIMES, ITEM_INT, DOC_DATE,
                                INCOTERMS, INCO_2, ITEM_PO, CHANGED_ON, 
                                CASE 
                                WHEN substr(QUANTITY, 1, 1) = 0 then  TO_CHAR(QUANTITY, 'FM99990D99', 'NLS_NUMERIC_CHARACTERS = ''. ''' )
                                ELSE replace(TO_CHAR(QUANTITY), '.','') 
                                END as QUANTITY, 
                                NET_PRICE, SCHED_DELIV_DATE, SHIPPING_NAME, SHORT_TEXT, MATERIAL, STOR_LOC
                                FROM SFISM4.R_TB_EDI_850_860 where PO = '{F_PO}' and ITEM_INT = '{F_ITEM_INT}' and PIP_TYPE = '{F_PIP_TYPE}' ";
               
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
                        await HandleTelitEDI(emp_no,rows, item, databaseName);
                       
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
        private async Task HandleTelitEDI(string emp_no, JToken rows, JToken item, string databaseName)
        {
            try
            { //16 rows
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
                                    ('{emp_no}', 'TELIT_850_860', 'INSERT', :actionDesc, sysdate)";
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

                    using (var transaction = connection.BeginTransaction())
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
