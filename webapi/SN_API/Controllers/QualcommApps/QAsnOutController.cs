using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using SN_API.Models;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using SN_API.Models.Config;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using SN_API.Services;
using Oracle.ManagedDataAccess.Types;


namespace SN_API.Controllers.QualcommApps
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QAsnOutController : ApiController
    {
        [System.Web.Http.Route("QAsnOutDataTable")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> QAsnOutDataTable(string databaseName, string inFunc, string inData, string empNo, string dateFrom, string dateTo)
        {
            try
            {
                string checkExistsProcedure = "SFIS1.SP_QASN_OUT";
                string procedureResult = "";
                List<Dictionary<string, object>> cursorData = new List<Dictionary<string, object>>();
                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = new OracleCommand(checkExistsProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("IN_FUNC", OracleDbType.Varchar2).Value = inFunc;
                        command.Parameters.Add("IN_DATA", OracleDbType.Varchar2).Value = inData;
                        command.Parameters.Add("IN_EMPNO", OracleDbType.Varchar2).Value = empNo;
                        command.Parameters.Add("IN_FROMDATE", OracleDbType.Varchar2).Value = dateFrom;
                        command.Parameters.Add("IN_TODATE", OracleDbType.Varchar2).Value = dateTo;
                        command.Parameters.Add("OUT_DT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        procedureResult = command.Parameters["RES"].Value.ToString();
                        try
                        {
                            var reader = ((OracleRefCursor)command.Parameters["OUT_DT"].Value).GetDataReader();

                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                cursorData.Add(row);
                            }
                        }
                        catch
                        {
                            cursorData = new List<Dictionary<string, object>>();
                        }
                    }
                }
                if (procedureResult == "OK")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = cursorData });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "Error", data = procedureResult });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        //[System.Web.Http.Route("GetQAsnOut")]
        //[System.Web.Http.HttpGet]
        //public async Task<HttpResponseMessage> GetQAsnOut(string database_name, string PACKSLIP_NO, string dateFrom, string dateTo, string showTimeForm)
        //{
        //    try
        //    {
        //        string strGetData = "";
        //        if (string.IsNullOrEmpty(PACKSLIP_NO) && showTimeForm == "false")
        //        {
        //            strGetData = $"select F_PACKSLIPNUMBER as PACKSLIP_NO, F_CREATIONDATETIME as CREATE_TIME, F_SENDERNAME, F_SENDERDUNS, F_RECEIVERNAME, F_RECEIVERDUNS, F_SHIPMCMN_CREATIONDATETIME, F_SHIPMCMN_TOTALOUTERCONTAINER, F_SHIPMCMN_ESTIMATEDDOCKTIME, F_SHIPMCMN_INVOICENUMBER, F_SUPPLIERNAME, F_SUPPLIERDUNS4, F_SUPPLIERDUNS, F_RECEIVER_NAME, F_RECEIVER_DUNS, F_PO_NUMBER, F_POLINE_NUMBER, F_ITEM_NUMBER, F_ITEM_SHIPPEDQUANTITY, F_ITEM_UNITOFMEASURE, F_ITEM_MPN, F_LOT_NUMBER, F_INNERBOX_SHIPPEDQUANTITY from SFISM4.R_QASN_OUT";
        //        }
        //        else if (!string.IsNullOrEmpty(PACKSLIP_NO) && showTimeForm == "false")
        //        {
        //            strGetData = $"select F_PACKSLIPNUMBER as PACKSLIP_NO, F_CREATIONDATETIME, F_SENDERNAME as CREATE_TIME, F_SENDERDUNS, F_RECEIVERNAME, F_RECEIVERDUNS, F_SHIPMCMN_CREATIONDATETIME, F_SHIPMCMN_TOTALOUTERCONTAINER, F_SHIPMCMN_ESTIMATEDDOCKTIME, F_SHIPMCMN_INVOICENUMBER, F_SUPPLIERNAME, F_SUPPLIERDUNS4, F_SUPPLIERDUNS, F_RECEIVER_NAME, F_RECEIVER_DUNS, F_PO_NUMBER, F_POLINE_NUMBER, F_ITEM_NUMBER, F_ITEM_SHIPPEDQUANTITY, F_ITEM_UNITOFMEASURE, F_ITEM_MPN, F_LOT_NUMBER, F_INNERBOX_SHIPPEDQUANTITY from SFISM4.R_QASN_OUT WHERE F_PACKSLIPNUMBER = '{PACKSLIP_NO}' ";
        //        }
        //        else if (showTimeForm == "true" && !string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
        //        {
        //            strGetData = $"select F_PACKSLIPNUMBER as PACKSLIP_NO, F_CREATIONDATETIME, F_SENDERNAME as CREATE_TIME, F_SENDERDUNS, F_RECEIVERNAME, F_RECEIVERDUNS, F_SHIPMCMN_CREATIONDATETIME, F_SHIPMCMN_TOTALOUTERCONTAINER, F_SHIPMCMN_ESTIMATEDDOCKTIME, F_SHIPMCMN_INVOICENUMBER, F_SUPPLIERNAME, F_SUPPLIERDUNS4, F_SUPPLIERDUNS, F_RECEIVER_NAME, F_RECEIVER_DUNS, F_PO_NUMBER, F_POLINE_NUMBER, F_ITEM_NUMBER, F_ITEM_SHIPPEDQUANTITY, F_ITEM_UNITOFMEASURE, F_ITEM_MPN, F_LOT_NUMBER, F_INNERBOX_SHIPPEDQUANTITY from SFISM4.R_QASN_OUT WHERE to_char(F_CREATIONDATETIME,'YYYYMMDD') >= '{dateFrom}' and to_char(F_CREATIONDATETIME,'YYYYMMDD') <= '{dateTo}' ";
        //        }
        //        DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
        //    }
        //    catch (SqlException ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
        //    }
        //}

        //[System.Web.Http.Route("GetShowDetailAsnOut")]
        //[System.Web.Http.HttpGet]
        //public async Task<HttpResponseMessage> GetShowDetailAsnOut(string database_name, string PACKSLIP_NO)
        //{
        //    try
        //    {
        //        string strGetData = "";
        //        string strGetDataTableOuterLPN = "";
        //        //string strGetDataTableInnerLPN = "";
        //        strGetData = $@"select distinct PACKSLIP_NO, FLAG, SHIP_MCMN_CREATTIME, MSG_SENDER_NAME, MSG_RECEIVER_NAME, AIRWAYBILL, FREIGHT_CARRIER_CODE,SUPPLIER_NAME, SHIP_MCMN_LOCATIONNAME,
        //                        CASE 
        //                        WHEN INSTR(SHIP_MCMN_GROSSWEIGHT, '.') > 0 THEN 
        //                        RTRIM(SHIP_MCMN_GROSSWEIGHT, '0') || SHIP_MCMN_WEIGHTUNITOFMEASURE
        //                        ELSE 
        //                        SHIP_MCMN_GROSSWEIGHT || SHIP_MCMN_WEIGHTUNITOFMEASURE
        //                        END AS GROSSWEIGHT,
        //                        CASE 
        //                        WHEN INSTR(SHIP_MCMN_NETWEIGHT, '.') > 0 THEN 
        //                        RTRIM(SHIP_MCMN_NETWEIGHT, '0') || SHIP_MCMN_WEIGHTUNITOFMEASURE
        //                        ELSE 
        //                        SHIP_MCMN_NETWEIGHT || SHIP_MCMN_WEIGHTUNITOFMEASURE
        //                        END AS NETWEIGHT,
        //                        SHIP_MCMN_ESTIMATEDDOCKTIME, SHIP_MCMN_TOTALOUTERCONTAINERS, COUNTRY_CODE, POSTAL_CODE, SHIP_MCMN_CITY, SHIP_MCMN_STREET1, SHIP_MCMN_STREET2, 
        //                        SHIP_MCMN_STREET3, SHIP_MCMN_LPN, PAL_GROSSWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT , PAL_NETWEIGHT || ' ' || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT, 
        //                        PAL_WIDTH || ' ' || PAL_DSUNITOFMEASURE as PAL_WIDTH, PAL_LENGTH || ' ' || PAL_DSUNITOFMEASURE as PAL_LENGTH, PAL_HEIGHT || ' ' || PAL_DSUNITOFMEASURE as PAL_HEIGHT, 
        //                        PO_NO, POLINE_NO, ITEM_NO, ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, FAB_SITE, ASSEMBLY_SITE, TEST_SITE, BUMP_SITE, SLT_SITE, 
        //                        PROBE_SITE, BE1_SITE, BE2_SITE, DATE_CODE, SEAL_DATE, WAFERQTY, MODULE_ASSY_SUBCONTRACTORS1, MODULE_ASSY_SUBCONTRACTORS2, MODULE_ASSY_SUBCONTRACTORS3, 
        //                        LOT_GRADE, MANUFACTURER, COUNTRY_OF_FABRICATION, COUNTRY_OF_ASSEMBLY, COUNTRY_OF_ORIGIN
        //                        FROM SFISM4.R_QASN_IN where PACKSLIP_NO = '{PACKSLIP_NO}' ";
        //        strGetDataTableOuterLPN = $"SELECT F_ID, LOT_NO, OUTERBOX_LPN, INNERBOX_LPN, INNERBOX_SHIPPEDQTY FROM SFISM4.R_QASN_IN WHERE PACKSLIP_NO = '{PACKSLIP_NO}' ORDER BY F_ID desc";
        //        //strGetDataTableInnerLPN = $"select INNERBOX_LPN, 'LPN' LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT, count(*) as INNERBOX_qty from SFISM4.R_QASN_IN  where PACKSLIP_NO = '{PACKSLIP_NO}' and flag in('0', '1', '2') group by INNERBOX_LPN, INNERBOX_GROSSWEIGHT, INNERBOX_NETWEIGHT";
        //        DataTable dtCheck = DBConnect.GetData(strGetData, database_name);
        //        DataTable dtCheckTableOuterLPN = DBConnect.GetData(strGetDataTableOuterLPN, database_name);
        //        // DataTable dtCheckTableInnerLPN = DBConnect.GetData(strGetDataTableInnerLPN, database_name);

        //        return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, dataTableOuterLPN = dtCheckTableOuterLPN });
        //    }
        //    catch (SqlException ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "Database error", message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
        //    }
        //}
    }
}
