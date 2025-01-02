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
        public object Z { get; private set; }
        public object T { get; private set; }

        [System.Web.Http.Route("GetLoadFormQReceipt")]
            [System.Web.Http.HttpGet]
            public async Task<HttpResponseMessage> GetLoadFormQReceipt(string database_name)
            {
                try
                {
                    string strGetData = "";
                    strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where flag in('0', '1') and rownum < 51 order by STATUS desc, CREATE_TIME desc";
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
                        strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE flag in('0', '1') and PACKSLIP_NO = '{PACKSLIP_NO}' order by PACKSLIP_NO desc";
                    }
                    else if (string.IsNullOrEmpty(PACKSLIP_NO) && showTimeForm == "false")
                    {
                        strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where flag in('0', '1') and rownum < 51 order by STATUS desc, CREATE_TIME desc";
                    }
                    else if (showTimeForm == "true" && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                    {
                        strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE flag in('0', '1') and to_char(CREAT_TIME,'YYYYMMDD') >= '{dateFrom}' and to_char(CREAT_TIME,'YYYYMMDD') <= '{dateTo}' order by STATUS desc, CREATE_TIME desc";
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
            public async Task<HttpResponseMessage> GetShowDetailQReceipt(string database_name, string PACKSLIP_NO)
            {
                try
                {
                    string strGetData = "";
                    string strGetDataTableOuterLPN = "";
                    //string strGetDataTableInnerLPN = "";
                    //string strGetDataReceiptAddress = "";
                    strGetData = $@"select distinct SITE, PIP_TYPE, PO_TYPE, CREAT_TIME, MSG_ID, 'FOXCONN' MSG_SENDER_NAME, MSG_RECEIVER_DUNS as MSG_SENDER_DUNS, 'QUALCOMM'MSG_RECEIVER_NAME, MSG_SENDER_DUNS as MSG_RECEIVER_DUNS,
                              PACKSLIP_NO, AIRWAYBILL as SHIP_MCMN_AIRWAYBILL, FREIGHT_CARRIER_CODE as SHIP_MCMN_FREIGHT_CARRIER_CODE, ''IMPORT_PERMIT_NO,
                             SHIP_MCMN_LOCATIONNAME as LOCATION_CODE, 'Jordan Li / 852-39749099'F_NAME,RECEIVER_DUNS, RECEIVER_DUNS4, sysdate + 1/24 as DATE_TIME, 'Bang Tai International Logistics Co., Limited' SHIP_MCMN_LOCATIONNAME,
                             'Yuen Long'SHIP_MCMN_CITY, 'HK'SHIP_MCMN_COUNTRYCODE, '000000'SHIP_MCMN_POSTALCODE, 'Ware House A, DD118, LOT 1120 RP Yau Cha Po Village, Tai Shu Ha West Road, Yuen Long, N.T., HK'SHIP_MCMN_ADDR1, 
                             ''SHIP_MCMN_ADDR2, ''SHIP_MCMN_ADDR3, PO_NO, POLINE_NO as PO_LINE, ITEM_UNITOFMEASURE as ITEMSHIP_MCMN_UNITOFMEASURE, ITEM_NO as ITEMSHIP_MCMN_NO,
                            ''ITEMSHIP_MCMN_RECEIVEDQTY, ''ITEMSHIP_MCMN_ACCEPTEDQTY, INNERBOX_LPN as INNERBOX_SHIP_MCMN_LPN, ''INNERBOX_SHIP_MCMN_RECEIVEDQTY, LAST_EDIT_TIME,
                                CASE 
                                WHEN INSTR(SHIP_MCMN_GROSSWEIGHT, '.') > 0 THEN 
                                RTRIM(SHIP_MCMN_GROSSWEIGHT, '0') || SHIP_MCMN_WEIGHTUNITOFMEASURE
                                ELSE 
                                SHIP_MCMN_GROSSWEIGHT || SHIP_MCMN_WEIGHTUNITOFMEASURE
                                END AS GROSSWEIGHT,
                                CASE
                                WHEN INSTR(SHIP_MCMN_NETWEIGHT, '.') > 0 THEN
                                RTRIM(SHIP_MCMN_NETWEIGHT, '0') || SHIP_MCMN_WEIGHTUNITOFMEASURE
                                ELSE 
                                SHIP_MCMN_NETWEIGHT || SHIP_MCMN_WEIGHTUNITOFMEASURE
                                END AS NETWEIGHT,
                               COUNTRY_CODE, POSTAL_CODE,
                                SHIP_MCMN_LPN, PAL_GROSSWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT , PAL_NETWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT, 
                                PAL_WIDTH || ' ' || PAL_DSUNITOFMEASURE as PAL_WIDTH, PAL_LENGTH || ' ' || PAL_DSUNITOFMEASURE as PAL_LENGTH, PAL_HEIGHT || ' ' || PAL_DSUNITOFMEASURE as PAL_HEIGHT, 
                                ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_NO FROM SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' ";
                //strGetDataTableOuterLPN = $"SELECT F_ID, LOT_NO, OUTERBOX_LPN, INNERBOX_LPN, INNERBOX_SHIPPEDQTY FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = '{PACKSLIP_NO}' ORDER BY F_ID desc";
                strGetDataTableOuterLPN = $"SELECT a.F_ID, a.LOT_NO, a.OUTERBOX_LPN as OUTERBOX, a.INNERBOX_LPN as INNERBOX, a.INNERBOX_SHIPPEDQTY as SHIPPEDQTY, b.INNERBOX_SHIP_MCMN_ACCEPTEDQTY as RECEIVEQTY, b.INNERBOX_MCMN_REJECTQTY as REJECTQTY, b.REASON FROM SFISM4.R_QASN_IN a,SFISM4.R_QRECEIPT_OUT b where a.PACKSLIP_NO = '{PACKSLIP_NO}' and a.LOT_NO = b.LOT_NO (+) ORDER BY F_ID";
                //if (value == "COMPLETED")
                    //{
                    //    strGetDataTableInnerLPN = $"select F_ID, INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' group by F_ID, INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                    //}else
                    //{
                    //    strGetDataTableInnerLPN = $"select F_ID, INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '1' group by F_ID, INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                    //}
                    
                //strGetDataTableInnerLPN = $"select F_ID, INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' group by F_ID, INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                    
                //strGetDataReceiptAddress = $"select SHIP_ADDRESS from SFISM4.R_SAP_DN_DETAIL_T where MODEL_NAME ='{PACKSLIP_NO}' ";
                    DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                    DataTable dtCheckTableOuterLPN = DBConnect.GetData(strGetDataTableOuterLPN, database_name);
                    //DataTable dtCheckTableInnerLPN = DBConnect.GetData(strGetDataTableInnerLPN, database_name);
                    //DataTable dtCheckReceiptAddress = DBConnect.GetData(strGetDataReceiptAddress, database_name);
                     return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dataTableOuterLPN = dtCheckTableOuterLPN});
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
        [Route("InsertQReceipt")]
        [HttpPost]
        public async Task<HttpResponseMessage> InsertQReceipt([FromBody] JObject model)
        {
            try
            {
                string emp_no = model["EMP_NO"].ToString();
                string databaseName = model["database_name"].ToString();
                string strPrivilege = $"SELECT emp FROM SFIS1.C_PRIVILEGE WHERE PRG_NAME='WEB_CONFIG' AND FUN = 'QRECEIPT_INSERT' AND EMP = '{emp_no}'";

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
                            //Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
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
                var creatTime = item["CREAT_TIME"]?.ToString();
                var msgId = item["MSG_ID"]?.ToString() ?? string.Empty;
                var msgSenderName = item["MSG_SENDER_NAME"]?.ToString() ?? string.Empty;
                var msgSenderDuns = item["MSG_SENDER_DUNS"]?.ToString() ?? string.Empty;
                var msgReceiverName = item["MSG_RECEIVER_NAME"]?.ToString() ?? string.Empty;
                var msgReceiverDuns = item["MSG_RECEIVER_DUNS"]?.ToString() ?? string.Empty;
                var packSlipNo = item["PACKSLIP_NO"]?.ToString() ?? string.Empty;
                var shipmcmnAirwayBill = item["SHIP_MCMN_AIRWAYBILL"]?.ToString() ?? string.Empty;
                var shipmcmnFreightCarrierCode = item["SHIP_MCMN_FREIGHT_CARRIER_CODE"]?.ToString() ?? string.Empty;
                var importPermitNo = item["IMPORT_PERMIT_NO"]?.ToString() ?? string.Empty;
                var locationCode = item["LOCATION_CODE"]?.ToString() ?? string.Empty;
                var fName = item["F_NAME"]?.ToString() ?? string.Empty;
                var receiverDuns = item["RECEIVER_DUNS"]?.ToString() ?? string.Empty;
                var receiverDuns4 = item["RECEIVER_DUNS4"]?.ToString() ?? string.Empty;
                var dateTime = item["DATE_TIME"]?.ToString();
                var shipmcmnLocationName = item["SHIP_MCMN_LOCATIONNAME"]?.ToString() ?? string.Empty;
                var shipmcmnCity = item["SHIP_MCMN_CITY"]?.ToString() ?? string.Empty;
                var shipmcmnCountryCode = item["SHIP_MCMN_COUNTRYCODE"]?.ToString() ?? string.Empty;
                var shipmcmnPostalCode = item["SHIP_MCMN_POSTALCODE"]?.ToString() ?? string.Empty;
                var shipmcmnAddr1 = item["SHIP_MCMN_ADDR1"]?.ToString() ?? string.Empty;
                var shipmcmnAddr2 = item["SHIP_MCMN_ADDR2"]?.ToString() ?? string.Empty;
                var shipmcmnAddr3 = item["SHIP_MCMN_ADDR3"]?.ToString() ?? string.Empty;
                var poNo = item["PO_NO"]?.ToString() ?? string.Empty;
                var poLine = item["PO_LINE"]?.ToString() ?? string.Empty;
                var itemShipmcmnUnitOfMeasure = item["ITEMSHIP_MCMN_UNITOFMEASURE"]?.ToString() ?? string.Empty;
                var itemShipmcmnNo = item["ITEMSHIP_MCMN_NO"]?.ToString() ?? string.Empty;
                var itemShipmcmnReceivedQty = item["ITEMSHIP_MCMN_RECEIVEDQTY"]?.ToString();
                var itemShipmcmnAcceptedQty = item["ITEMSHIP_MCMN_ACCEPTEDQTY"]?.ToString();
                var innerBoxShipmcmnLpn = item["INNERBOX_SHIP_MCMN_LPN"]?.ToString() ?? string.Empty;
                var innerBoxShipmcmnReceivedQty = item["INNERBOX_SHIP_MCMN_RECEIVEDQTY"]?.ToString();
                var lastEditTime = item["LAST_EDIT_TIME"]?.ToString();
                var lotNo = rows["LOT_NO"]?.ToString() ?? string.Empty;
                var innerBoxShipmcmnAcceptedQty = rows["receiveQty"]?.ToString() ?? string.Empty;
                var innerBoxmcmnRejectQty = rows["rejectQty"]?.ToString() ?? string.Empty;
                var reason = rows["reason"]?.ToString() ?? string.Empty;
                var shippedQty = rows["SHIPPEDQTY"]?.ToString() ?? string.Empty;
                string updateQAsn_in = $"update SFISM4.R_QASN_IN set flag = '1' where PACKSLIP_NO = :packSlipNo";
                string insertQReceipt = $@"INSERT INTO SFISM4.R_QRECEIPT_OUT (SITE, PIP_TYPE, PO_TYPE, CREAT_TIME, MSG_ID, MSG_SENDER_NAME, MSG_SENDER_DUNS, MSG_RECEIVER_NAME, MSG_RECEIVER_DUNS, PACKSLIP_NO, SHIP_MCMN_AIRWAYBILL, SHIP_MCMN_FREIGHT_CARRIER_CODE, IMPORT_PERMIT_NO, 
                                                LOCATION_CODE, F_NAME, RECEIVER_DUNS, RECEIVER_DUNS4, DATE_TIME, SHIP_MCMN_LOCATIONNAME, SHIP_MCMN_CITY, SHIP_MCMN_COUNTRYCODE, SHIP_MCMN_POSTALCODE, SHIP_MCMN_ADDR1, SHIP_MCMN_ADDR2, SHIP_MCMN_ADDR3, 
                                                PO_NO, PO_LINE, ITEMSHIP_MCMN_UNITOFMEASURE, ITEMSHIP_MCMN_NO, ITEMSHIP_MCMN_RECEIVEDQTY, ITEMSHIP_MCMN_ACCEPTEDQTY, INNERBOX_SHIP_MCMN_LPN, INNERBOX_SHIP_MCMN_RECEIVEDQTY, LAST_EDIT_TIME, LOT_NO, INNERBOX_SHIP_MCMN_ACCEPTEDQTY, INNERBOX_MCMN_REJECTQTY, REASON) 
                                           VALUES (:site, :pipType, :poType, TO_CHAR(systimestamp, 'YYYY-MM-DD""T""HH24:MI:SS.FF3""Z""'), :msgId, :msgSenderName, :msgSenderDuns, :msgReceiverName, :msgReceiverDuns, :packSlipNo, :shipmcmnAirwayBill, :shipmcmnFreightCarrierCode, :importPermitNo,
                                                :locationCode, :fName, :receiverDuns, :receiverDuns4, to_date(:dateTime,'YYYY/MM/DD HH24:MI:SS'), :shipmcmnLocationName, :shipmcmnCity, :shipmcmnCountryCode, :shipmcmnPostalCode, :shipmcmnAddr1, :shipmcmnAddr2, :shipmcmnAddr3, 
                                                :poNo, :poLine, :itemShipmcmnUnitOfMeasure, :itemShipmcmnNo, :itemShipmcmnReceivedQty, :itemShipmcmnAcceptedQty, :innerBoxShipmcmnLpn, :innerBoxShipmcmnReceivedQty, sysdate, :lotNo, :innerBoxShipmcmnAcceptedQty, :innerBoxmcmnRejectQty, :reason)";
                string checkExistsProcedure = "SFIS1.SP_ADD_TRANS";
                string procedureResult = "";
                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var commandQAsn = new OracleCommand(updateQAsn_in, connection))
                    {
                        commandQAsn.Parameters.Add(new OracleParameter("packSlipNo", packSlipNo));
                        await commandQAsn.ExecuteNonQueryAsync();
                    }

                        using (var command = new OracleCommand(checkExistsProcedure, connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add("IN_TRANSACTIONS", OracleDbType.Varchar2).Value = "InventoryReceiptTransaction";
                            command.Parameters.Add("IN_REASON", OracleDbType.Varchar2).Value = null;
                            command.Parameters.Add("IN_LOTNO", OracleDbType.Varchar2).Value = lotNo;
                            command.Parameters.Add("IN_QTY", OracleDbType.Int32).Value = shippedQty;
                            command.Parameters.Add("IN_STAGE_OUT", OracleDbType.Varchar2).Value = null;
                            command.Parameters.Add("IN_STAGE_IN", OracleDbType.Varchar2).Value = null;
                            command.Parameters.Add("RES", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                            await command.ExecuteNonQueryAsync();
                            procedureResult = command.Parameters["RES"].Value.ToString();
                        }

                    using (var command = new OracleCommand(insertQReceipt, connection))
                    {
                        command.Parameters.Add(new OracleParameter("site", site));
                        command.Parameters.Add(new OracleParameter("pipType", pipType));
                        command.Parameters.Add(new OracleParameter("poType", poType));
                        //sysdate - 7/24
                        //command.Parameters.Add(new OracleParameter("creatTime", creatTime));
                        command.Parameters.Add(new OracleParameter("msgId", msgId));
                        command.Parameters.Add(new OracleParameter("msgSenderName", msgSenderName));
                        command.Parameters.Add(new OracleParameter("msgSenderDuns", msgSenderDuns));
                        command.Parameters.Add(new OracleParameter("msgReceiverName", msgReceiverName));
                        command.Parameters.Add(new OracleParameter("msgReceiverDuns", msgReceiverDuns));
                        command.Parameters.Add(new OracleParameter("packSlipNo", packSlipNo));
                        command.Parameters.Add(new OracleParameter("shipmcmnAirwayBill", shipmcmnAirwayBill));
                        command.Parameters.Add(new OracleParameter("shipmcmnFreightCarrierCode", shipmcmnFreightCarrierCode));
                        command.Parameters.Add(new OracleParameter("importPermitNo", importPermitNo));
                        command.Parameters.Add(new OracleParameter("locationCode", locationCode));
                        command.Parameters.Add(new OracleParameter("fName", fName));
                        command.Parameters.Add(new OracleParameter("receiverDuns", receiverDuns));
                        command.Parameters.Add(new OracleParameter("receiverDuns4", receiverDuns4));
                        command.Parameters.Add(new OracleParameter("dateTime", dateTime));
                        command.Parameters.Add(new OracleParameter("shipmcmnLocationName", shipmcmnLocationName));
                        command.Parameters.Add(new OracleParameter("shipmcmnCity", shipmcmnCity));
                        command.Parameters.Add(new OracleParameter("shipmcmnCountryCode", shipmcmnCountryCode));
                        command.Parameters.Add(new OracleParameter("shipmcmnPostalCode", shipmcmnPostalCode));
                        command.Parameters.Add(new OracleParameter("shipmcmnAddr1", shipmcmnAddr1));
                        command.Parameters.Add(new OracleParameter("shipmcmnAddr2", shipmcmnAddr2));
                        command.Parameters.Add(new OracleParameter("shipmcmnAddr3", shipmcmnAddr3));
                        command.Parameters.Add(new OracleParameter("poNo", poNo));
                        command.Parameters.Add(new OracleParameter("poLine", poLine));
                        command.Parameters.Add(new OracleParameter("itemShipmcmnUnitOfMeasure", itemShipmcmnUnitOfMeasure));
                        command.Parameters.Add(new OracleParameter("itemShipmcmnNo", itemShipmcmnNo));
                        command.Parameters.Add(new OracleParameter("itemShipmcmnReceivedQty", itemShipmcmnReceivedQty));
                        command.Parameters.Add(new OracleParameter("itemShipmcmnAcceptedQty", itemShipmcmnAcceptedQty));
                        command.Parameters.Add(new OracleParameter("innerBoxShipmcmnLpn", innerBoxShipmcmnLpn));
                        command.Parameters.Add(new OracleParameter("innerBoxShipmcmnReceivedQty", innerBoxShipmcmnReceivedQty));
                        //sysdate
                        //command.Parameters.Add(new OracleParameter("lastEditTime", lastEditTime));
                        command.Parameters.Add(new OracleParameter("lotNo", lotNo));
                        command.Parameters.Add(new OracleParameter("innerBoxShipmcmnAcceptedQty", innerBoxShipmcmnAcceptedQty));
                        command.Parameters.Add(new OracleParameter("innerBoxmcmnRejectQty", innerBoxmcmnRejectQty));
                        command.Parameters.Add(new OracleParameter("reason", reason));
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
