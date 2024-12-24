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

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MappingMoController : ApiController
    {
        [System.Web.Http.Route("GetMappingMo")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetMappingMo(string database_name, string EMP_NO, string IN_ACTION_TYPE, string LOT_NO, string MO_NUMBER)
        {
            try
            {
                string checkExistsProcedure = "SFIS1.SP_LOT_LINK_MO";
                string procedureResult = "";
                string strSqlResult = "";
                var connectionString = new GetString().Get()[database_name];
                if(string.IsNullOrEmpty(LOT_NO))
                {
                    LOT_NO = "a";
                }
                if (string.IsNullOrEmpty(MO_NUMBER))
                {
                    MO_NUMBER = "a";
                }
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = new OracleCommand(checkExistsProcedure, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("IN_ACTION_TYPE", OracleDbType.Varchar2).Value = IN_ACTION_TYPE;
                        command.Parameters.Add("IN_EMP", OracleDbType.Varchar2).Value = EMP_NO;
                        command.Parameters.Add("IN_LOTNO", OracleDbType.Varchar2).Value = LOT_NO;
                        command.Parameters.Add("IN_MO", OracleDbType.Varchar2).Value = MO_NUMBER;

                        command.Parameters.Add("STRSQL", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        strSqlResult = command.Parameters["STRSQL"].Value?.ToString();
                        procedureResult = command.Parameters["RES"].Value.ToString();
                    }
                }
                if (procedureResult == "OK")
                {
                    //return Request.CreateResponse(HttpStatusCode.OK, new { data = strSqlResult });
                    //strGetData = $"SELECT PACKSLIP_NO, FLAG, F_ID, decode(FLAG, 0, '1',1,'2',2,'0') as arrange, DECODE(FLAG, 0, 'WAIT TO RECEIVE',1,'COMPLETED',2,'WAIT TO CONFIRM') as Status, CREAT_TIME as CREATE_TIME, PO_NO, POLINE_NO as PO_LINE, ITEM_NO,ITEM_SHIPPEDQTY || ' ' || ITEM_UNITOFMEASURE as ITEM_SHIPPEDQTY, ITEM_MPN, ITEM_DESCRIPTION, LOT_NO, RECEIVER_LOCATIONNAME as RECEIVER_LOCATION, RECEIVER_NAME,SHIP_MCMN_LPN as PALLET_LPN,PAL_GROSSWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_GROSSWEIGHT, PAL_NETWEIGHT || PAL_WEIGHTUNITOFMEASURE as PAL_NETWEIGHT FROM SFISM4.R_QASN_IN where flag in('0', '1', '2') and rownum < 51 order by arrange asc, F_ID desc, CREATE_TIME desc";
                    DataTable dtCheck = DBConnect.GetData(strSqlResult, database_name);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "Error", data = strSqlResult });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { error = "An error occurred", message = ex.Message });
            }
        }
        //IN_ACTION_TYPE IN     VARCHAR2,
        //IN_EMP IN     VARCHAR2,
        //IN_LOTNO IN     VARCHAR2,
        //IN_MO IN     VARCHAR2,
        //STRSQL OUT VARCHAR2,
        //RES OUT VARCHAR2
        public class MappingMoElement
        {
            public string database_name { get; set; }
            public string EMP_NO { get; set; }
        }
    }
}