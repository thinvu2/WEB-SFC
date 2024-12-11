using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class QASN_INController : ApiController
    {
        [System.Web.Http.Route("GetLoadFormQASN_IN")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetLoadFormQASN_IN(string database_name)
        {
            try
            {
                string strGetData = "";
                strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where rownum < 100 order by F_ID desc, CREATE_TIME desc";
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
        [System.Web.Http.Route("GetQASN_IN")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetQASN_IN(string database_name, string PACKSLIP_NO, string dateFrom, string dateTo)
        {
            try
            {
                if (string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    dateFrom = "19000101";
                }

                if (string.IsNullOrEmpty(dateTo) && !string.IsNullOrEmpty(dateFrom))
                {
                    dateTo = DateTime.Now.ToString("yyyyMMdd");
                }
                string strGetData = "";
                if (string.IsNullOrEmpty(PACKSLIP_NO) && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE to_char(CREAT_TIME,'YYYYMMDD') >= '{dateFrom}' and to_char(CREAT_TIME,'YYYYMMDD') <= '{dateTo}' order by F_ID desc, CREATE_TIME desc";
                }
                else if (!string.IsNullOrEmpty(PACKSLIP_NO))
                {
                    strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = '{PACKSLIP_NO}' order by F_ID desc, CREATE_TIME desc";
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

        [System.Web.Http.Route("GetShowDetail")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetShowDetail(string database_name, string PACKSLIP_NO, string FLAG, string F_ID)
        {
            try
            {
                string strGetData = "";
                string strGetDataTableOuterLPN = "";
                string strGetDataTableInnerLPN = "";
                strGetData = $@"select PACKSLIP_NO, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, AIRWAYBILL, FREIGHT_CARRIER_CODE,SUPPLIER_NAME, SHIP_MCMN_LOCATIONNAME,
                                SHIP_MCMN_GROSSWEIGHT ||' ' ||SHIP_MCMN_WEIGHTUNITOFMEASURE as GROSSWEIGHT,SHIP_MCMN_NETWEIGHT || ' ' || SHIP_MCMN_WEIGHTUNITOFMEASURE as NETWEIGHT, 
                                SHIP_MCMN_ESTIMATEDDOCKTIME, SHIP_MCMN_TOTALOUTERCONTAINERS, COUNTRY_CODE, POSTAL_CODE, SHIP_MCMN_CITY, SHIP_MCMN_STREET1, SHIP_MCMN_STREET2, 
                                SHIP_MCMN_STREET3, SHIP_MCMN_LPN, PAL_GROSSWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT , PAL_NETWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT, 
                                PAL_WIDTH || ' ' || PAL_DSUNITOFMEASURE as PAL_WIDTH, PAL_LENGTH || ' ' || PAL_DSUNITOFMEASURE as PAL_LENGTH, PAL_HEIGHT || ' ' || PAL_DSUNITOFMEASURE as PAL_HEIGHT, 
                                PO_NO, POLINE_NO, LOT_NO, ITEM_NO, ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, FAB_SITE, ASSEMBLY_SITE, TEST_SITE, BUMP_SITE, SLT_SITE, 
                                PROBE_SITE, BE1_SITE, BE2_SITE, DATE_CODE, SEAL_DATE, WAFERQTY, MODULE_ASSY_SUBCONTRACTORS1, MODULE_ASSY_SUBCONTRACTORS2, MODULE_ASSY_SUBCONTRACTORS3, 
                                LOT_GRADE, MANUFACTURER, COUNTRY_OF_FABRICATION, COUNTRY_OF_ASSEMBLY, COUNTRY_OF_ORIGIN
                                FROM SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' and F_ID = '{F_ID}' ";
                strGetDataTableOuterLPN = $"select PACKSLIP_NO, flag, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE, count(*) as OUTERBOX_qty from SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' group by PACKSLIP_NO, flag, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE";
                strGetDataTableInnerLPN = $"select INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' group by INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                DataTable dtCheckTableOuterLPN = DBConnect.GetData(strGetDataTableOuterLPN, database_name);
                DataTable dtCheckTableInnerLPN = DBConnect.GetData(strGetDataTableInnerLPN, database_name);
                if (dtCheck.Rows.Count == 0 || dtCheckTableOuterLPN.Rows.Count == 0 || dtCheckTableInnerLPN.Rows.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dataTableOuterLPN = dtCheckTableOuterLPN, dataTableInnerLPN = dtCheckTableInnerLPN });
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
        //QReceipt
        [System.Web.Http.Route("GetLoadFormQReceipt")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetLoadFormQReceipt(string database_name)
        {
            try
            {
                string strGetData = "";
                strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, DECODE(FLAG, 0, 'WAITING',1,'CONFIRMED','WAITING') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where rownum < 100 order by F_ID desc, CREATE_TIME desc";
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
        public async Task<HttpResponseMessage> GetDataQReciept(string database_name, string PACKSLIP_NO, string dateFrom, string dateTo)
        {
            try
            {
                if (string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    dateFrom = "19000101";
                }

                if (string.IsNullOrEmpty(dateTo) && !string.IsNullOrEmpty(dateFrom))
                {
                    dateTo = DateTime.Now.ToString("yyyyMMdd");
                }
                string strGetData = "";

                if (string.IsNullOrEmpty(PACKSLIP_NO) && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, DECODE(FLAG, 0, 'WAITING',1,'CONFIRMED','WAITING') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE to_char(CREAT_TIME,'YYYYMMDD') >= '{dateFrom}' and to_char(CREAT_TIME,'YYYYMMDD') <= '{dateTo}' order by F_ID desc, CREATE_TIME desc";
                }
                else if (!string.IsNullOrEmpty(PACKSLIP_NO))
                {
                    strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, DECODE(FLAG, 0, 'WAITING',1,'CONFIRMED','WAITING') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = '{PACKSLIP_NO}' order by PACKSLIP_NO desc";
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
        public async Task<HttpResponseMessage> GetShowDetailQReceipt(string database_name, string PACKSLIP_NO, string FLAG, string F_ID)
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
                strGetDataTableOuterLPN = $"select PACKSLIP_NO, flag, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE, count(*) as OUTERBOX_qty from SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' and FLAG = '{FLAG}' group by PACKSLIP_NO, flag, OUTERBOX_LPN, OUTERBOX_GROSSWEIGHT, OUTERBOX_NETWEIGHT, OUTERBOX_WEIGHTUNITOFMEASURE, INNERBOX_WEIGHTUNITOFMEASURE";
                strGetDataTableInnerLPN = $"select INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' group by INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
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
        [System.Web.Http.Route("InsertQReceipt")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertQReceipt(QASN_IN_Element model)
        {
            try
            {
                //check privilege
                //string strPrivilege = $" SELECT * FROM  sfis1.C_PRIVILEGE  where PRG_NAME='CONFIG'  AND FUN = 'MODEL_EDIT' AND EMP='{model.EMP_NO}'";
                //if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new { result = "privilege" });
                //}
                //check exist
                //string strCheckExist = $" select model_name from SFIS1.C_MODEL_DESC_T where model_name = '{model.MODEL_NAME}' ";
                //string actionString = "";
                StringBuilder sb = new StringBuilder();
                StringBuilder sbColor = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                //if (DBConnect.GetData(strCheckExist, model.database_name).Rows.Count <= 0)
                //{
                // not exist => insert
                sb.Append(" Insert into SFISM4.R_QRECEIPT_OUT ");
                    sb.Append(" (SITE, PIP_TYPE, PO_TYPE, CREAT_TIME, MSG_ID, MSG_SENDER_NAME, MSG_SENDER_DUNS, MSG_RECEIVER_NAME, MSG_RECEIVER_DUNS, PACKSLIP_NO, RECEIVER_DUNS, RECEIVER_DUNS4, SHIP_MCMN_LOCATIONNAME, SHIP_MCMN_CITY, PO_NO, LOT_NO, SHIP_MCMN_AIRWAYBILL, SHIP_MCMN_FREIGHT_CARRIER_CODE, LOCATION_CODE, SHIP_MCMN_COUNTRYCODE, SHIP_MCMN_POSTALCODE, SHIP_MCMN_ADDR1, SHIP_MCMN_ADDR2, SHIP_MCMN_ADDR3, PO_LINE, ITEMSHIP_MCMN_UNITOFMEASURE, ITEMSHIP_MCMN_NO, INNERBOX_SHIP_MCMN_LPN, LAST_EDIT_TIME) ");
                    sb.Append(" values ( ");
                    sb.Append($"'{model.SITE}', ");
                    sb.Append($"'{model.PIP_TYPE}', ");
                    sb.Append($"'{model.PO_TYPE}', ");
                    sb.Append($"to_date('{model.CREAT_TIME}','YYYY/MM/DD HH24:MI:SS'), "); 
                    sb.Append($"'{model.MSG_ID}', "); 
                    sb.Append($"'{model.MSG_SENDER_NAME}', ");
                    sb.Append($"'{model.MSG_SENDER_DUNS}', ");
                    sb.Append($"'{model.MSG_RECEIVER_NAME}', ");
                    sb.Append($"'{model.MSG_RECEIVER_DUNS}', ");
                    sb.Append($"'{model.PACKSLIP_NO}', ");
                    sb.Append($"'{model.RECEIVER_DUNS}', ");
                    sb.Append($"'{model.RECEIVER_DUNS4}', ");
                    sb.Append($"'{model.SHIP_MCMN_LOCATIONNAME}', ");
                    sb.Append($"'{model.SHIP_MCMN_CITY}', ");
                    sb.Append($"'{model.PO_NO}', ");
                    sb.Append($"'{model.LOT_NO}', ");
                    sb.Append($"'{model.AIRWAYBILL}', ");
                    sb.Append($"'{model.SHIP_MCMN_FREIGHT_CARRIER_CODE}', ");
                    sb.Append($"'{model.LOCATIONNAME}', ");
                    sb.Append($"'{model.SHIP_MCMN_COUNTRYCODE}', ");
                    sb.Append($"'{model.SHIP_MCMN_POSTALCODE}', ");
                    sb.Append($"'{model.SHIP_MCMN_ADDR1}', ");
                    sb.Append($"'{model.SHIP_MCMN_ADDR2}', ");
                    sb.Append($"'{model.SHIP_MCMN_ADDR3}', ");
                    sb.Append($"'{model.POLINE_NO}', ");
                    sb.Append($"'{model.ITEM_UNITOFMEASURE}', ");
                    sb.Append($"'{model.ITEM_NO}', ");
                    sb.Append($"'{model.INNERBOX_LPN}', ");
                sb.Append($"to_date('{model.LAST_EDIT_TIME}','YYYY/MM/DD HH24:MI:SS')");
                    sb.Append(" )");

                sbColor.Append(" update SFISM4.R_QASN_IN ");
                sbColor.Append(" SET ");
                sbColor.Append($" FLAG = '1' ");
                sbColor.Append($"where PACKSLIP_NO = '{model.PACKSLIP_NO}' and FLAG = '{model.FLAG}' and F_ID = '{model.F_ID}' ");

                sbLog.Append(" INSERT INTO sfism4.r_system_log_t (EMP_NO,PRG_NAME,ACTION_TYPE,ACTION_DESC) ");
                sbLog.Append(" VALUES ( ");
                sbLog.Append($" '{model.EMP_NO}', ");
                sbLog.Append($" 'QRECEIPT', ");
                sbLog.Append($" 'INSERT', ");
                sbLog.Append($"  'QASN_IN: PACKSLIP_NO {model.PACKSLIP_NO}, SITE {model.SITE}, PIP_TYPE: {model.PIP_TYPE}, IP:{AuthorizationController.UserIP()}' ");
                sbLog.Append(" ) ");

                string strUpdate = sbColor.ToString();
                string strInserUpdate = sb.ToString();
                string strSbLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strInserUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strSbLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        public class QASN_IN_Element
        {
            public string database_name { get; set; }
            public string EMP_NO { get; set; }
            public string SITE {get; set;}
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
        }
    }
}
