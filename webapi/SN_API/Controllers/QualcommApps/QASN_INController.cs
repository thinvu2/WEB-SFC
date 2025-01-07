using SN_API.Models;
using System;
using System.Data;
using System.Data.SqlClient;
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
                strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, decode(FLAG, 0, '1',1,'2',2,'0') as arrange, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED',2,'WAIT TO CONFIRM') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where flag in('0', '1', '2') and rownum < 51 order by arrange asc, CREATE_TIME desc";
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
        public async Task<HttpResponseMessage> GetQASN_IN(string database_name, string PACKSLIP_NO, string dateFrom, string dateTo, string showTimeForm)
        {
            try
            {
                string strGetData = "";
                if(string.IsNullOrEmpty(PACKSLIP_NO) && showTimeForm == "false")
                {
                    strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, decode(FLAG, 0, '1',1,'2',2,'0') as arrange, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED',2,'WAIT TO CONFIRM') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where flag in('0', '1', '2') and rownum < 51 order by arrange asc, CREATE_TIME desc";
                }
                else if (!string.IsNullOrEmpty(PACKSLIP_NO) && showTimeForm == "false")
                {
                    strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, decode(FLAG, 0, '1',1,'2',2,'0') as arrange, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED',2,'WAIT TO CONFIRM') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE flag in('0', '1', '2') and PACKSLIP_NO = '{PACKSLIP_NO}' order by arrange asc, CREATE_TIME desc";
                }
                else if (showTimeForm == "true" && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
                {
                    strGetData = $"SELECT distinct PACKSLIP_NO, FLAG, decode(FLAG, 0, '1',1,'2',2,'0') as arrange, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED',2,'WAIT TO CONFIRM') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN WHERE flag in('0', '1', '2') and to_char(CREAT_TIME,'YYYYMMDD') >= '{dateFrom}' and to_char(CREAT_TIME,'YYYYMMDD') <= '{dateTo}' order by arrange asc, CREATE_TIME desc";
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
        public async Task<HttpResponseMessage> GetShowDetail(string database_name, string PACKSLIP_NO)
        {
            try
            {
                string strGetData = "";
                string strGetDataTableOuterLPN = "";
                //string strGetDataTableInnerLPN = "";
                strGetData = $@"select distinct PACKSLIP_NO, FLAG, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, AIRWAYBILL, FREIGHT_CARRIER_CODE,SUPPLIER_NAME, SHIP_MCMN_LOCATIONNAME,
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
                                SHIP_MCMN_ESTIMATEDDOCKTIME, SHIP_MCMN_TOTALOUTERCONTAINERS, COUNTRY_CODE, POSTAL_CODE, SHIP_MCMN_CITY, SHIP_MCMN_STREET1, SHIP_MCMN_STREET2, 
                                SHIP_MCMN_STREET3, SHIP_MCMN_LPN, PAL_GROSSWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT , PAL_NETWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT, 
                                PAL_WIDTH || ' ' || PAL_DSUNITOFMEASURE as PAL_WIDTH, PAL_LENGTH || ' ' || PAL_DSUNITOFMEASURE as PAL_LENGTH, PAL_HEIGHT || ' ' || PAL_DSUNITOFMEASURE as PAL_HEIGHT, 
                                PO_NO, POLINE_NO, ITEM_NO, ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, FAB_SITE, ASSEMBLY_SITE, TEST_SITE, BUMP_SITE, SLT_SITE, 
                                PROBE_SITE, BE1_SITE, BE2_SITE, DATE_CODE, SEAL_DATE, WAFERQTY, MODULE_ASSY_SUBCONTRACTORS1, MODULE_ASSY_SUBCONTRACTORS2, MODULE_ASSY_SUBCONTRACTORS3, 
                                LOT_GRADE, MANUFACTURER, COUNTRY_OF_FABRICATION, COUNTRY_OF_ASSEMBLY, COUNTRY_OF_ORIGIN
                                FROM SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' ";
                strGetDataTableOuterLPN = $"SELECT F_ID, LOT_NO, OUTERBOX_LPN, INNERBOX_LPN, INNERBOX_SHIPPEDQTY FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = '{PACKSLIP_NO}' ORDER BY F_ID desc";
                //strGetDataTableInnerLPN = $"select INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and flag in('0', '1', '2') group by INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
                DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
                DataTable dtCheckTableOuterLPN = DBConnect.GetData(strGetDataTableOuterLPN, database_name);
               // DataTable dtCheckTableInnerLPN = DBConnect.GetData(strGetDataTableInnerLPN, database_name);

                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dataTableOuterLPN = dtCheckTableOuterLPN });
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
        //insert
        [System.Web.Http.Route("InsertQasn_in")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> InsertQasn_in(Qasn_in_Element model)
        {
            try
            {
                //check privilege
                string strPrivilege = $"SELECT EMP FROM  sfis1.C_PRIVILEGE  where PRG_NAME = 'WEB_CONFIG' and fun = 'QASN_IN' and PRIVILEGE = '0' and EMP = '{model.EMP_NO}'";
                if (DBConnect.GetData(strPrivilege, model.database_name).Rows.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "notPrivilege" });
                }
                StringBuilder sb = new StringBuilder();
                StringBuilder sbLog = new StringBuilder();
                string flagStatus = "";
                if(model.actionBtn == "actionSubmit")
                {
                    flagStatus = "0";
                    sb.Append(" update SFISM4.R_QASN_IN ");
                    sb.Append(" SET ");
                    sb.Append($" FLAG = '0' ");
                    sb.Append($"where PACKSLIP_NO = '{model.PACKSLIP_NO}' and FLAG = '{model.FLAG}' and F_ID = '{model.F_ID}' ");
                }else
                {
                    flagStatus = "3";
                    sb.Append(" update SFISM4.R_QASN_IN ");
                    sb.Append(" SET ");
                    sb.Append($" FLAG = '3' ");
                    sb.Append($"where PACKSLIP_NO = '{model.PACKSLIP_NO}' and FLAG = '{model.FLAG}' and F_ID = '{model.F_ID}' ");
                }

                //insert log
                sbLog.Append("INSERT INTO sfism4.r_system_log_t (EMP_NO, PRG_NAME, ACTION_TYPE, ACTION_DESC) ");
                sbLog.Append("VALUES ( ");
                sbLog.Append($"'{model.EMP_NO}', ");
                sbLog.Append($"'QASN_IN', ");
                sbLog.Append($"'UPDATE', ");
                sbLog.Append($"'QASN_IN: PACKSLIP_NO: {model.PACKSLIP_NO}, FLAG_OLD: {model.FLAG}, FLAG_NEW: {flagStatus}, F_ID: {model.F_ID}, IP: {AuthorizationController.UserIP()}'");
                sbLog.Append(") ");

                string strUpdate = sb.ToString();
                string strSbLog = sbLog.ToString();
                DBConnect.ExecuteNoneQuery(strUpdate, model.database_name);
                DBConnect.ExecuteNoneQuery(strSbLog, model.database_name);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = ex.Message });
            }
        }
        public class Qasn_in_Element
        {
            public string database_name { get; set; }
            public string EMP_NO { get; set; }
            public string PACKSLIP_NO { get; set; }
            public string FLAG { get; set; }
            public string F_ID { get; set; }
            public string actionBtn { get; set; }
        }
    }
}
