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
    public class ReceiptConfigController : ApiController
    {
        [System.Web.Http.Route("QDataConfig")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> QDataConfig(string databaseName, string IN_FUNC, string IN_SUBFUNC, string IN_DATA, string IN_EMPNO)
        {
            try
            {
                string checkExistsProcedure = "SFIS1.SP_QDATA_CONFIG";
                string procedureResult = "";
                List<Dictionary<string, object>> cursorData = new List<Dictionary<string, object>>();
                var connectionString = new GetString().Get()[databaseName];
                using (var connection = new OracleConnection(connectionString))
                {
                    using (var command = new OracleCommand(checkExistsProcedure, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("IN_FUNC", OracleDbType.Varchar2).Value = IN_FUNC;
                        command.Parameters.Add("IN_SUBFUNC", OracleDbType.Varchar2).Value = IN_SUBFUNC;
                        command.Parameters.Add("IN_DATA", OracleDbType.Varchar2).Value = IN_DATA;
                        command.Parameters.Add("IN_EMPNO", OracleDbType.Varchar2).Value = IN_EMPNO;
                        command.Parameters.Add("OUT_DT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        command.Parameters.Add("RES", OracleDbType.Varchar2, 1000).Direction = ParameterDirection.Output;
                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                        procedureResult = command.Parameters["RES"].Value.ToString();
                        if(IN_DATA == "null" || IN_SUBFUNC == "SHOWDATA")
                        {
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
                }
                if (procedureResult == "OK")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok",  data = cursorData });
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
