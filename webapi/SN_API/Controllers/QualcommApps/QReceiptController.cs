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

namespace SN_API.Controllers.QualcommApps
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QReceiptController : ApiController
    {
            [System.Web.Http.Route("GetLoadFormQReceipt")]
            [System.Web.Http.HttpGet]
            public async Task<HttpResponseMessage> GetLoadFormQReceipt(string database_name)
            {
                try
                {
                    string strGetData = "";
                    strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where flag in('0', '1') and rownum < 51 order by STATUS desc, F_ID desc, CREATE_TIME desc";
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
            [System.Web.Http.Route("GetDataQReciept")]
            [System.Web.Http.HttpGet]
            public async Task<HttpResponseMessage> GetDataQReciept(string database_name, string PACKSLIP_NO, string dateFrom, string dateTo, string showTimeForm)
            {
                try
                {
                    string strGetData = "";
                    if (!string.IsNullOrEmpty(PACKSLIP_NO) && showTimeForm == "false")
                    {
                        strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE flag in('0', '1') and PACKSLIP_NO = '{PACKSLIP_NO}' order by PACKSLIP_NO desc";
                    }
                    else if (string.IsNullOrEmpty(PACKSLIP_NO) && showTimeForm == "false")
                    {
                        strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where flag in('0', '1') and rownum < 51 order by STATUS desc, F_ID desc, CREATE_TIME desc";
                    }
                    else if (showTimeForm == "true" && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                    {
                        strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE flag in('0', '1') and to_char(CREAT_TIME,'YYYYMMDD') >= '{dateFrom}' and to_char(CREAT_TIME,'YYYYMMDD') <= '{dateTo}' order by STATUS desc, F_ID desc, CREATE_TIME desc";
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

            [System.Web.Http.Route("GetShowDetailQReceipt")]
            [System.Web.Http.HttpGet]
            public async Task<HttpResponseMessage> GetShowDetailQReceipt(string database_name, string PACKSLIP_NO, string FLAG, string F_ID, string value)
            {
                try
                {
                    string strGetData = "";
                    string strGetDataTableOuterLPN = "";
                    string strGetDataTableInnerLPN = "";
                    string strGetDataReceiptAddress = "";
                    strGetData = $@"select PACKSLIP_NO, FLAG, F_ID, PIP_TYPE,SITE,PO_TYPE,CREAT_TIME, MSG_SENDER_NAME,MSG_ID, MSG_RECEIVER_NAME,MSG_SENDER_DUNS,RECEIVER_DUNS,RECEIVER_DUNS4,SHIP_MCMN_LOCATIONNAME,LAST_EDIT_TIME, AIRWAYBILL,MSG_RECEIVER_DUNS, FREIGHT_CARRIER_CODE,SUPPLIER_NAME, SHIP_MCMN_LOCATIONNAME,
                                SHIP_MCMN_GROSSWEIGHT ||' ' ||SHIP_MCMN_WEIGHTUNITOFMEASURE as GROSSWEIGHT,SHIP_MCMN_NETWEIGHT || ' ' || SHIP_MCMN_WEIGHTUNITOFMEASURE as NETWEIGHT, 
                                SHIP_MCMN_STREET1, SHIP_MCMN_STREET2, SHIP_MCMN_CITY,RECEIVER_LOCATIONNAME, COUNTRY_CODE, POSTAL_CODE,
                                SHIP_MCMN_STREET3, SHIP_MCMN_LPN, PAL_GROSSWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT , PAL_NETWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT, 
                                PAL_WIDTH || ' ' || PAL_DSUNITOFMEASURE as PAL_WIDTH, PAL_LENGTH || ' ' || PAL_DSUNITOFMEASURE as PAL_LENGTH, PAL_HEIGHT || ' ' || PAL_DSUNITOFMEASURE as PAL_HEIGHT, 
                                PO_NO, POLINE_NO, LOT_NO, ITEM_NO, ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY,ITEM_UNITOFMEASURE, ITEM_MPN, INNERBOX_LPN
                                FROM SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' and F_ID = '{F_ID}' ";
                    strGetDataTableOuterLPN = $"select PACKSLIP_NO, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE, count(*) as OUTERBOX_qty from SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' and flag in('0', '1', '2') group by PACKSLIP_NO, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE";
                    if (value == "COMPLETED")
                    {
                        strGetDataTableInnerLPN = $"select F_ID, INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' group by F_ID, INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                    }else
                    {
                        strGetDataTableInnerLPN = $"select F_ID, INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '1' group by F_ID, INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                    }
                    
                strGetDataTableInnerLPN = $"select F_ID, INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' group by F_ID, INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                    
                strGetDataReceiptAddress = $"select SHIP_ADDRESS from SFISM4.R_SAP_DN_DETAIL_T where MODEL_NAME ='{PACKSLIP_NO}' ";
                    DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                    DataTable dtCheckTableOuterLPN = DBConnect.GetData(strGetDataTableOuterLPN, database_name);
                    DataTable dtCheckTableInnerLPN = DBConnect.GetData(strGetDataTableInnerLPN, database_name);
                    DataTable dtCheckReceiptAddress = DBConnect.GetData(strGetDataReceiptAddress, database_name);
                    if (dtCheck.Rows.Count == 0 || dtCheckTableOuterLPN.Rows.Count == 0 || dtCheckTableInnerLPN.Rows.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dataTableOuterLPN = dtCheckTableOuterLPN, dataTableInnerLPN = dtCheckTableInnerLPN, dataReceiveAddress = dtCheckReceiptAddress });
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
        //[System.Web.Http.Route("InsertQReceipt")]
        //[System.Web.Http.HttpPost]
        //public async Task<HttpResponseMessage> InsertQReceipt(QReceipt_Element model)
        //{
        //    try
        //    {
        //        //check privilege
        //        //string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'MODEL_EDIT' AND EMP='{model.EMP_NO}'";
        //        //if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
        //        //{
        //        //    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
        //        //}
        //        //check exist
        //        //string strCheckExist = $" select model_name from SFIS1.C_MODEL_DESC_T where model_name = '{model.MODEL_NAME}' ";
        //        //string actionString = "";
        //        StringBuilder sb = new StringBuilder();
        //        //StringBuilder sbTest = new StringBuilder();
        //        StringBuilder sbColor = new StringBuilder();
        //        StringBuilder sbLog = new StringBuilder();
        //        //if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
        //        //{
        //        // not exist => insert
        //        sb.Append(" Insert into SFISM4.R_QRECEIPT_OUT ");
        //        sb.Append(" (SITE, PIP_TYPE, PO_TYPE, CREAT_TIME, MSG_ID, MSG_SENDER_NAME, MSG_SENDER_DUNS, MSG_RECEIVER_NAME, MSG_RECEIVER_DUNS, PACKSLIP_NO, RECEIVER_DUNS, RECEIVER_DUNS4, SHIP_MCMN_LOCATIONNAME, SHIP_MCMN_CITY, PO_NO, LOT_NO, SHIP_MCMN_AIRWAYBILL, SHIP_MCMN_FREIGHT_CARRIER_CODE, LOCATION_CODE, SHIP_MCMN_COUNTRYCODE, SHIP_MCMN_POSTALCODE, SHIP_MCMN_ADDR1, SHIP_MCMN_ADDR2, SHIP_MCMN_ADDR3, PO_LINE, ITEMSHIP_MCMN_UNITOFMEASURE, ITEMSHIP_MCMN_NO, INNERBOX_SHIP_MCMN_LPN, INNERBOX_SHIP_MCMN_ACCEPTEDQTY, INNERBOX_MCMN_REJECTQTY, REASON, LAST_EDIT_TIME) ");
        //        sb.Append(" values ( ");
        //        sb.Append($"'{model.SITE}', ");
        //        sb.Append($"'{model.PIP_TYPE}', ");
        //        sb.Append($"'{model.PO_TYPE}', ");
        //        sb.Append($"to_date('{model.CREAT_TIME}','YYYY/MM/DD HH24:MI:SS'), ");
        //        sb.Append($"'{model.MSG_ID}', ");
        //        sb.Append($"'{model.MSG_SENDER_NAME}', ");
        //        sb.Append($"'{model.MSG_SENDER_DUNS}', ");
        //        sb.Append($"'{model.MSG_RECEIVER_NAME}', ");
        //        sb.Append($"'{model.MSG_RECEIVER_DUNS}', ");
        //        sb.Append($"'{model.PACKSLIP_NO}', ");
        //        sb.Append($"'{model.RECEIVER_DUNS}', ");
        //        sb.Append($"'{model.RECEIVER_DUNS4}', ");
        //        sb.Append($"'{model.SHIP_MCMN_LOCATIONNAME}', ");
        //        sb.Append($"'{model.SHIP_MCMN_CITY}', ");
        //        sb.Append($"'{model.PO_NO}', ");
        //        sb.Append($"'{model.LOT_NO}', ");
        //        sb.Append($"'{model.AIRWAYBILL}', ");
        //        sb.Append($"'{model.SHIP_MCMN_FREIGHT_CARRIER_CODE}', ");
        //        sb.Append($"'{model.LOCATIONNAME}', ");
        //        sb.Append($"'{model.SHIP_MCMN_COUNTRYCODE}', ");
        //        sb.Append($"'{model.SHIP_MCMN_POSTALCODE}', ");
        //        sb.Append($"'{model.SHIP_MCMN_ADDR1}', ");
        //        sb.Append($"'{model.SHIP_MCMN_ADDR2}', ");
        //        sb.Append($"'{model.SHIP_MCMN_ADDR3}', ");
        //        sb.Append($"'{model.POLINE_NO}', ");
        //        sb.Append($"'{model.ITEM_UNITOFMEASURE}', ");
        //        sb.Append($"'{model.ITEM_NO}', ");
        //        sb.Append($"'{model.INNERBOX_LPN}', ");
        //        sb.Append($"'{model.selectReceiveQty}', ");
        //        sb.Append($"'{model.selectRejectQty}', ");
        //        sb.Append($"'{model.selectReason}', ");
        //    sb.Append($"to_date('{model.LAST_EDIT_TIME}','YYYY/MM/DD HH24:MI:SS')");
        //        sb.Append(" )");

        //        sbColor.Append(" update SFISM4.R_QASN_IN ");
        //        sbColor.Append(" SET ");
        //        sbColor.Append($" FLAG = '1' ");
        //        sbColor.Append($"where PACKSLIP_NO = '{model.PACKSLIP_NO}' and FLAG = '{model.FLAG}' and F_ID = '{model.F_ID}' ");

        //    //sbTest.Append("Insert into SFISM4.R_BPCS_TMP_INVOICE_T");
        //    //sbTest.Append(" (BNAME, BAD1, BAD2, BAD3) ");
        //    //sbTest.Append(" values ( ");
        //    //sbTest.Append($"'{model.selectedItems}', ");
        //    //sbTest.Append($"'{model.selectOptions}', ");
        //    //sbTest.Append($"'{model.selectReceiptQty}', ");
        //    //sbTest.Append($"'{model.selectReason}'");
        //    //sbTest.Append(" )");

        //    sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
        //        sbLog.Append(" VALUES ( ");
        //        sbLog.Append($" '{model.EMP_NO}', ");
        //        sbLog.Append($" 'QRECEIPT', ");
        //        sbLog.Append($" 'INSERT', ");
        //        sbLog.Append($"  'QASN_IN: PACKSLIP_NO {model.PACKSLIP_NO}, SITE {model.SITE}, PIP_TYPE: {model.PIP_TYPE}, IP: {AuthorizationController.UserIP()}' ");
        //        sbLog.Append(" ) ");

        //        //string strTest = sbTest.ToString();
        //        string strUpdate = sbColor.ToString();
        //        string strInserUpdate = sb.ToString();
        //        string strSbLog = sbLog.ToString();
        //        //DBConnect.ExecuteNoneQuery(strTest, model.database_name);
        //        DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);
        //        DBConnect.ExecuteNoneQuery(strUpdate, model.database_name);
        //        DBConnect.ExecuteNoneQuery(strSbLog, model.database_name);
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
        //    }
        //}
        [Route("InsertQReceipt")]
        [HttpPost]
        public async Task<HttpResponseMessage> InsertQReceipt([FromBody] JObject model)
        {
            try
            {
                string emp_no = model["EMP_NO"].ToString();
                string databaseName = model["database_name"].ToString();
                string strPrivilege = $" SELECT emp FROM  SFIS1.C_PRIVILEGE  WHERE PRG_NAME='CONFIG'  AND FUN = 'MODEL_EDIT' AND EMP='{emp_no}'";

                if (DBConnect.GetData(strPrivilege, databaseName).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "This emp has no privilege" });
                }

                JArray data = (JArray)model["data"];
                JArray rowsInnerBox = (JArray)model["rowsInnerBox"];
                foreach (var rows in rowsInnerBox)
                {
                    foreach (var item in data)
                    {
                        try
                        {
                            await HandleQReceipt(rows, item, databaseName);
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
        private async Task HandleQReceipt(JToken rows, JToken item, string databaseName)
        {
            try
            {
                var site = item["SITE"]?.ToString() ?? string.Empty;
                var pipType = item["PIP_TYPE"]?.ToString() ?? string.Empty;
                var poType = item["PO_TYPE"]?.ToString() ?? string.Empty;
                var creatTime = item["CREAT_TIME"]?.ToString() ?? string.Empty;
                var msgId = rows["F_ID"]?.ToString() ?? string.Empty;
                var msgSenderName = item["MSG_SENDER_NAME"]?.ToString() ?? string.Empty;
                var msgSenderDuns = item["MSG_SENDER_DUNS"]?.ToString() ?? string.Empty;
                var msgReceiverName = item["MSG_RECEIVER_NAME"]?.ToString() ?? string.Empty;
                var msgReceiverDuns = item["MSG_RECEIVER_DUNS"]?.ToString() ?? string.Empty;
                var packSlipNo = item["PACKSLIP_NO"]?.ToString();
                var receiverDuns = item["RECEIVER_DUNS"]?.ToString();
                var receiverDuns4 = item["RECEIVER_DUNS4"]?.ToString();
                var shipmcmnLocationName = item["SHIP_MCMN_LOCATIONNAME"]?.ToString();
                var shipmcmnCity = item["SHIP_MCMN_CITY"]?.ToString();
                var poNo = item["PO_NO"]?.ToString();
                var lotNo = item["LOT_NO"]?.ToString();
                var shipmcmnAirwayBill = item["SHIP_MCMN_AIRWAYBILL"]?.ToString();
                var shipmcmnFreightCarrierCode = item["SHIP_MCMN_FREIGHT_CARRIER_CODE"]?.ToString();
                var locationCode = item["LOCATION_CODE"]?.ToString();
                var shipmcmnCountryCode = item["SHIP_MCMN_COUNTRYCODE"]?.ToString();
                var shipmcmnPostalCode = item["SHIP_MCMN_POSTALCODE"]?.ToString();
                var shipmcmnAddr1 = item["SHIP_MCMN_ADDR1"]?.ToString();
                var shipmcmnAddr2 = item["SHIP_MCMN_ADDR2"]?.ToString();
                var shipmcmnAddr3 = item["SHIP_MCMN_ADDR3"]?.ToString();
                var poLine = item["PO_LINE"]?.ToString();
                var itemShipmcmnUnitOfMeasure = item["ITEMSHIP_MCMN_UNITOFMEASURE"]?.ToString();
                var itemShipmcmnNo = item["ITEMSHIP_MCMN_NO"]?.ToString();
                var innerBoxShipmcmnLpn = rows["INNERBOX_LPN"]?.ToString();
                var innerBoxShipmcmnAcceptedQty = rows["receiveQty"]?.ToString();
                var innerBoxmcmnRejectQty = rows["rejectQty"]?.ToString();
                var reason = rows["reason"]?.ToString();
                var lastEditTime = item["LAST_EDIT_TIME"]?.ToString();
                string updateQAsn_in = $"update SFISM4.R_QASN_IN set flag = '1' where PACKSLIP_NO = :packSlipNo and F_ID = :msgId ";
                string insertQReceipt = $@" INSERT INTO SFISM4.R_QRECEIPT_OUT (
                                SITE, PIP_TYPE, PO_TYPE, CREAT_TIME, MSG_ID, MSG_SENDER_NAME, MSG_SENDER_DUNS, MSG_RECEIVER_NAME, MSG_RECEIVER_DUNS, PACKSLIP_NO, RECEIVER_DUNS, RECEIVER_DUNS4, SHIP_MCMN_LOCATIONNAME, SHIP_MCMN_CITY, 
                                PO_NO, LOT_NO, SHIP_MCMN_AIRWAYBILL, SHIP_MCMN_FREIGHT_CARRIER_CODE, LOCATION_CODE, SHIP_MCMN_COUNTRYCODE, SHIP_MCMN_POSTALCODE, SHIP_MCMN_ADDR1, SHIP_MCMN_ADDR2, SHIP_MCMN_ADDR3, PO_LINE, 
                                ITEMSHIP_MCMN_UNITOFMEASURE, ITEMSHIP_MCMN_NO, INNERBOX_SHIP_MCMN_LPN, INNERBOX_SHIP_MCMN_ACCEPTEDQTY, INNERBOX_MCMN_REJECTQTY, REASON, LAST_EDIT_TIME
                                ) 
                                VALUES (:site, :pipType, :poType, to_date(:creatTime,'YYYY/MM/DD HH24:MI:SS'), :msgId, :msgSenderName, :msgSenderDuns, :msgReceiverName, :msgReceiverDuns, :packSlipNo, :receiverDuns, :receiverDuns4,
                                :shipmcmnLocationName, :shipmcmnCity, :poNo, :lotNo, :shipmcmnAirwayBill, :shipmcmnFreightCarrierCode, :locationCode, :shipmcmnCountryCode, :shipmcmnPostalCode, :shipmcmnAddr1,
                                :shipmcmnAddr2, :shipmcmnAddr3, :poLine, :itemShipmcmnUnitOfMeasure, :itemShipmcmnNo, :innerBoxShipmcmnLpn, :innerBoxShipmcmnAcceptedQty, :innerBoxmcmnRejectQty, :reason, to_date(:lastEditTime,'YYYY/MM/DD HH24:MI:SS')
                                )";
                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var commandQAsn = new OracleCommand(updateQAsn_in, connection))
                    {
                        commandQAsn.Parameters.Add(new OracleParameter("packSlipNo", packSlipNo));
                        commandQAsn.Parameters.Add(new OracleParameter("msgId", msgId));
                        await commandQAsn.ExecuteNonQueryAsync();
                    }
                    using (var command = new OracleCommand(insertQReceipt, connection))
                    {
                        command.Parameters.Add(new OracleParameter("site", site));
                        command.Parameters.Add(new OracleParameter("pipType", pipType));
                        command.Parameters.Add(new OracleParameter("poType", poType));
                        command.Parameters.Add(new OracleParameter("creatTime", creatTime));
                        command.Parameters.Add(new OracleParameter("msgId", msgId));
                        command.Parameters.Add(new OracleParameter("msgSenderName", msgSenderName));
                        command.Parameters.Add(new OracleParameter("msgSenderDuns", msgSenderDuns));
                        command.Parameters.Add(new OracleParameter("msgReceiverName", msgReceiverName));
                        command.Parameters.Add(new OracleParameter("msgReceiverDuns", msgReceiverDuns));
                        command.Parameters.Add(new OracleParameter("packSlipNo", packSlipNo));
                        command.Parameters.Add(new OracleParameter("receiverDuns", receiverDuns));
                        command.Parameters.Add(new OracleParameter("receiverDuns4", receiverDuns4));
                        command.Parameters.Add(new OracleParameter("shipmcmnLocationName", shipmcmnLocationName));
                        command.Parameters.Add(new OracleParameter("shipmcmnCity", shipmcmnCity));
                        command.Parameters.Add(new OracleParameter("poNo", poNo));
                        command.Parameters.Add(new OracleParameter("lotNo", lotNo));
                        command.Parameters.Add(new OracleParameter("shipmcmnAirwayBill", shipmcmnAirwayBill));
                        command.Parameters.Add(new OracleParameter("shipmcmnFreightCarrierCode", shipmcmnFreightCarrierCode));
                        command.Parameters.Add(new OracleParameter("locationCode", locationCode));
                        command.Parameters.Add(new OracleParameter("shipmcmnCountryCode", shipmcmnCountryCode));
                        command.Parameters.Add(new OracleParameter("shipmcmnPostalCode", shipmcmnPostalCode));
                        command.Parameters.Add(new OracleParameter("shipmcmnAddr1", shipmcmnAddr1));
                        command.Parameters.Add(new OracleParameter("shipmcmnAddr2", shipmcmnAddr2));
                        command.Parameters.Add(new OracleParameter("shipmcmnAddr3", shipmcmnAddr3));
                        command.Parameters.Add(new OracleParameter("poLine", poLine));
                        command.Parameters.Add(new OracleParameter("itemShipmcmnUnitOfMeasure", itemShipmcmnUnitOfMeasure));
                        command.Parameters.Add(new OracleParameter("itemShipmcmnNo", itemShipmcmnNo));
                        command.Parameters.Add(new OracleParameter("innerBoxShipmcmnLpn", innerBoxShipmcmnLpn));
                        command.Parameters.Add(new OracleParameter("innerBoxShipmcmnAcceptedQty", innerBoxShipmcmnAcceptedQty));
                        command.Parameters.Add(new OracleParameter("innerBoxmcmnRejectQty", innerBoxmcmnRejectQty));
                        command.Parameters.Add(new OracleParameter("reason", reason));
                        command.Parameters.Add(new OracleParameter("lastEditTime", lastEditTime));
                        await command.ExecuteNonQueryAsync();
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
        public class QReceipt_Element
            {
                public string database_name { get; set; }
                public string EMP_NO { get; set; }
                public string SITE { get; set; }
                public string PIP_TYPE { get; set; }
                public string PO_TYPE { get; set; }
                public string CREAT_TIME { get; set; }
                public string MSG_ID { get; set; }
                public string MSG_SENDER_NAME { get; set; }
                public string MSG_SENDER_DUNS { get; set; }
                public string MSG_RECEIVER_NAME { get; set; }
                public string MSG_RECEIVER_DUNS { get; set; }
                public string PACKSLIP_NO { get; set; }
                public string RECEIVER_DUNS { get; set; }
                public string RECEIVER_DUNS4 { get; set; }
                public string SHIP_MCMN_LOCATIONNAME { get; set; }
                public string SHIP_MCMN_CITY { get; set; }
                public string PO_NO { get; set; }
                public string LOT_NO { get; set; }
                public string AIRWAYBILL { get; set; }
                public string SHIP_MCMN_FREIGHT_CARRIER_CODE { get; set; }
                public string LOCATIONNAME { get; set; }
                public string SHIP_MCMN_COUNTRYCODE { get; set; }
                public string SHIP_MCMN_POSTALCODE { get; set; }
                public string SHIP_MCMN_ADDR1 { get; set; }
                public string SHIP_MCMN_ADDR2 { get; set; }
                public string SHIP_MCMN_ADDR3 { get; set; }
                public string LAST_EDIT_TIME { get; set; }
                public string FLAG { get; set; }
                public string F_ID { get; set; }
                public string POLINE_NO { get; set; }
                public string ITEM_UNITOFMEASURE { get; set; }
                public string ITEM_NO { get; set; }
                public string INNERBOX_LPN { get; set; }
                public string selectOptions{ get; set; }
                public string selectReceiveQty { get; set; }
                public string selectRejectQty { get; set; }
                public string selectReason{ get; set; }
                public string selectedItems{ get; set; }
       }
    }
}
