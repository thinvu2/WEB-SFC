﻿using System;
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
using System.Web;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MappingMoController : ApiController
    {
        [System.Web.Http.Route("GetMappingMo")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetMappingMo(string databaseName, string empNo, string inActionType, string lotNo, string moNumber, string valueSearch)
        {
            try
            {
                string checkExistsProcedure = "SFIS1.SP_LOT_LINK_MO";
                string procedureResult = "";
                string strSqlResult = "";
                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = new OracleCommand(checkExistsProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("IN_ACTION_TYPE", OracleDbType.Varchar2).Value = inActionType;
                        command.Parameters.Add("IN_EMP", OracleDbType.Varchar2).Value = empNo;
                        command.Parameters.Add("IN_LOTNO", OracleDbType.Varchar2).Value = HttpUtility.UrlDecode(lotNo); ;
                        command.Parameters.Add("IN_MO", OracleDbType.Varchar2).Value = moNumber;
                        command.Parameters.Add("IN_DATA", OracleDbType.Varchar2).Value = valueSearch;
                        command.Parameters.Add("STRSQL", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        strSqlResult = command.Parameters["STRSQL"].Value?.ToString();
                        procedureResult = command.Parameters["RES"].Value.ToString();
                    }
                }
                if (procedureResult == "OK")
                {
                    DataTable dtCheck = DBConnect.GetData(strSqlResult, databaseName);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
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
    }
}