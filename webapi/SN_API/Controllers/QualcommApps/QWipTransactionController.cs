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
                    strGetData = $@"SELECT LOT_NO,PO_NO,SCHEDULE_COMPLETION_QTY,FULL_JOB_QTY,TRANS_NAME,F_id,CREATE_TIME_UTC,CREATE_TIME,SENDER_NAME,SENDER_DUNS,SENDER_DUNS4,RECEIVER_DUNS,
                                SSM_PROCESS_VER,FLAG,EVENT_DATE,EVENT_DATE_UTC,TRANS_TYPE,LOT_STATE,SEQ_NO,REASON,PROCESS_NAME,STAGE_NAME,
                                OPERATION_STEP,MANUFACTURING_TYPE,SUPPLIER_OPERATION,LOT_ITEM,LOT_QTY,BIN_NO,KGD,DATE_CODE,TEST_PRG,ISPARTIAL,
                                SCHEDULE_COMPLETION_DATETIME,P_LOT_NO,P_LOT_ITEMS,P_LOT_QTY,SSM_ITEM,NOTES,LAST_EDIT_TIME,FILENAME
                                FROM SFISM4.R_QWIP_DATA where upper(TRANS_NAME) = '{trans_name.ToUpper()}'";
                }
                else
                {
                    strGetData = $@"SELECT LOT_NO,PO_NO,SCHEDULE_COMPLETION_QTY,FULL_JOB_QTY,TRANS_NAME,F_id,CREATE_TIME_UTC,CREATE_TIME,SENDER_NAME,SENDER_DUNS,SENDER_DUNS4,RECEIVER_DUNS,
                                SSM_PROCESS_VER,FLAG,EVENT_DATE,EVENT_DATE_UTC,TRANS_TYPE,LOT_STATE,SEQ_NO,REASON,PROCESS_NAME,STAGE_NAME,
                                OPERATION_STEP,MANUFACTURING_TYPE,SUPPLIER_OPERATION,LOT_ITEM,LOT_QTY,BIN_NO,KGD,DATE_CODE,TEST_PRG,ISPARTIAL,
                                SCHEDULE_COMPLETION_DATETIME,P_LOT_NO,P_LOT_ITEMS,P_LOT_QTY,SSM_ITEM,NOTES,LAST_EDIT_TIME,FILENAME
                                FROM SFISM4.R_QWIP_DATA where upper(TRANS_NAME) = '{trans_name.ToUpper()}' and upper(lot_no) = '{lot_no.ToUpper()}'";
                }
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);

                if (dtCheck.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
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
        [System.Web.Http.Route("GetLoadLotNoQWip")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetLoadLotNoQWip(string database_name,string type)
        {
            try
            {
                string strGetData = "";
                strGetData = $"select distinct LOT_NO as RECEIVELOT from SFISM4.R_QWIP_DATA";

                if (type == "CONSUME_GET_LOTNO")
                {
                    strGetData = @" select distinct lot_no as RECEIVELOT from SFISM4.R_QWIP_DATA where trans_name='WIPStartTransaction'";
                }
                else if (type == "MOVE_GET_LOTNO")
                {
                    strGetData = @" select distinct lot_no as RECEIVELOT from SFISM4.R_QWIP_DATA where trans_name='WIPConsumeTransaction'";
                }
                else if (type == "SCRAP_GET_LOTNO")
                {
                    strGetData = @" select distinct lot_no as RECEIVELOT from SFISM4.R_QWIP_DATA where ";
                }
                else if (type == "RELEASE_GET_LOTNO")
                {
                    strGetData = @" select distinct lot_no as RECEIVELOT from SFISM4.R_QWIP_DATA where trans_name='WIPHoldTransaction'";
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
        [System.Web.Http.Route("CheckQty_Qwip")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CheckQty_Qwip(Qwip_Element qwip)
        {
            try
            {
                string strGetData = "";
                strGetData = $@"  select * from SFISM4.R_QWIP_DATA 
                     where lot_no = '{qwip.lot_no}' and SCHEDULE_COMPLETION_QTY >= '{qwip.qty}' ";

                if (qwip.transaction == "WIPHoldTransaction")
                {
                    strGetData = $@"  select * from SFISM4.R_QWIP_DATA 
                     where lot_no = '{qwip.lot_no}' and SCHEDULE_COMPLETION_QTY >= '{qwip.qty}' ";
                }else if (qwip.transaction == "WIPReleaseTransaction")
                {
                    strGetData = $@"  select * from SFISM4.R_QWIP_DATA 
                     where lot_no = '{qwip.lot_no}' and SCHEDULE_COMPLETION_QTY >= '{qwip.qty}' and TRANS_NAME = 'WIPHoldTransaction' ";
                }

                DataTable dtCheck = DBConnect.GetData(strGetData, qwip.database_name);
                if (dtCheck.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
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
                        command.Parameters.Add("IN_TRANSACTIONS", OracleDbType.Varchar2).Value = model.transaction == null?"": model.transaction;
                        command.Parameters.Add("IN_REASON", OracleDbType.Varchar2).Value = model.reason == null ? "" : model.reason;
                        command.Parameters.Add("IN_LOTNO", OracleDbType.Varchar2).Value = model.lot_no == null ? "" : model.lot_no;
                        command.Parameters.Add("IN_QTY", OracleDbType.Int32).Value = Int32.Parse(model.qty == null ? "0" : model.qty);
                        command.Parameters.Add("IN_TYPE", OracleDbType.Varchar2).Value = model.IN_MOVE_TYPE == null ? "" : model.IN_MOVE_TYPE;
                        command.Parameters.Add("IN_STAGE", OracleDbType.Varchar2).Value = model.IN_STAGE == null ? "" : model.IN_STAGE;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        procedureResult = command.Parameters["RES"].Value.ToString();

                    }
                }
                if(model.IN_MOVE_TYPE == "GET_STAGE")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = procedureResult });
                }
                if (model.IN_MOVE_TYPE == "CONSUME_GET_QTY")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = procedureResult });
                }
                if (procedureResult == "OK")
                {
                    strGetData = $@"SELECT TRANS_NAME,LOT_NO,PO_NO,SCHEDULE_COMPLETION_QTY,FULL_JOB_QTY,F_id,CREATE_TIME_UTC,CREATE_TIME,SENDER_NAME,SENDER_DUNS,SENDER_DUNS4,RECEIVER_DUNS,
                                SSM_PROCESS_VER,FLAG,EVENT_DATE,EVENT_DATE_UTC,TRANS_TYPE,LOT_STATE,SEQ_NO,REASON,PROCESS_NAME,STAGE_NAME,
                                OPERATION_STEP,MANUFACTURING_TYPE,SUPPLIER_OPERATION,LOT_ITEM,LOT_QTY,BIN_NO,KGD,DATE_CODE,TEST_PRG,ISPARTIAL,
                                SCHEDULE_COMPLETION_DATETIME,P_LOT_NO,P_LOT_ITEMS,P_LOT_QTY,SSM_ITEM,NOTES,LAST_EDIT_TIME,FILENAME
                                FROM SFISM4.R_QWIP_DATA where upper(TRANS_NAME) = '{model.transaction.ToUpper()}' and upper(lot_no) = '{model.lot_no.ToUpper()}'
                                order by EVENT_DATE desc";
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
            public string IN_MOVE_TYPE { get; set; }
            public string IN_STAGE { get; set; }
        }
    }
}