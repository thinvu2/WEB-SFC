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
        public async Task<HttpResponseMessage> GetDataTelitEDI(string database_name, string PURCHASE_ORDER, string dateFrom, string dateTo, string showTimeForm)
        {
            try
            {
                string strGetData = "";
                if (!string.IsNullOrEmpty(PURCHASE_ORDER) && showTimeForm == "false")
                {
                    strGetData = $"SELECT DISTINCT PO as PURCHASE_ORDER, DECODE(PIP_TYPE,'850','NEW','CHANGE') as PO_TYPE, ITEM_INT as ITEM_NO, DOC_DATE as TIME, INCOTERMS, INCO_2, ITEM_PO, ITEM, DELET_IND,CHANGED_ON as CHANGE_TIME, QUANTITY, NET_PRICE, SHIPPING_NAME as SHIPPING_ADDRESS FROM SFISM4.R_TB_EDI_850_860 where PO = '{PURCHASE_ORDER}'";
                }
                else if (string.IsNullOrEmpty(PURCHASE_ORDER) && showTimeForm == "false")
                {
                    strGetData = $"SELECT DISTINCT PO as PURCHASE_ORDER, DECODE(PIP_TYPE,'850','NEW','CHANGE') as PO_TYPE, ITEM_INT as ITEM_NO, DOC_DATE as TIME, INCOTERMS, INCO_2, ITEM_PO, ITEM, DELET_IND,CHANGED_ON as CHANGE_TIME, QUANTITY, NET_PRICE, SHIPPING_NAME as SHIPPING_ADDRESS FROM SFISM4.R_TB_EDI_850_860";
                }
                else if (showTimeForm == "true" && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    strGetData = $"SELECT DISTINCT PO as PURCHASE_ORDER, DECODE(PIP_TYPE,'850','NEW','CHANGE') as PO_TYPE, ITEM_INT as ITEM_NO, DOC_DATE as TIME, INCOTERMS, INCO_2, ITEM_PO, ITEM, DELET_IND,CHANGED_ON as CHANGE_TIME, QUANTITY, NET_PRICE, SHIPPING_NAME as SHIPPING_ADDRESS FROM SFISM4.R_TB_EDI_850_860 WHERE to_char(F_ON,'YYYYMMDD') >= '{dateFrom}' and to_char(F_ON,'YYYYMMDD') <= '{dateTo}'";
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
        public async Task<HttpResponseMessage> GetShowDetailTelitEDI(string database_name, string PURCHASE_ORDER)
        {
            try
            {
                string strGetData = "";
                strGetData = $@"SELECT DISTINCT PO as PURCHASE_ORDER, DECODE(PIP_TYPE,'850','NEW','CHANGE') as PO_TYPE,  ITEM_INT as ITEM_NO, DOC_DATE as TIME, INCOTERMS , INCO_2  ,  ITEM_PO ,  ITEM , DELET_IND ,CHANGED_ON as CHANGE_TIME, QUANTITY,  NET_PRICE, shipping_name as SHIPPING_ADDRESS FROM SFISM4.R_TB_EDI_850_860 where PO = '{PURCHASE_ORDER}' ";
               
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
                            await HandleTelitEDI(rows, item, databaseName);
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
        private async Task HandleTelitEDI(JToken rows, JToken item, string databaseName)
        {
            try
            {
                var site = item["SITE"]?.ToString() ?? string.Empty;
                var pipType = item["PIP_TYPE"]?.ToString() ?? string.Empty;
                var poType = item["PO_TYPE"]?.ToString() ?? string.Empty;
                var creatTime = item["CREAT_TIME"]?.ToString() ?? string.Empty;
                var msgId = item["MSG_ID"]?.ToString() ?? string.Empty;
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
                var lotNo = rows["LOT_NO"]?.ToString();
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
                var innerBoxShipmcmnLpn = item["INNERBOX_LPN"]?.ToString();
                var innerBoxShipmcmnAcceptedQty = rows["receiveQty"]?.ToString();
                var innerBoxmcmnRejectQty = rows["rejectQty"]?.ToString();
                var reason = rows["reason"]?.ToString();
                var lastEditTime = item["LAST_EDIT_TIME"]?.ToString();
                string updateQAsn_in = $"update SFISM4.R_QASN_IN set flag = '1' where PACKSLIP_NO = :packSlipNo and LOT_NO = :lotNo ";
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
                        commandQAsn.Parameters.Add(new OracleParameter("lotNo", lotNo));
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
    }
}
