using EnterpriseSystems.Infrastructure.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EnterpriseSystems.Infrastructure.DAO
{
    public class SqlServerDAO
    {
        private const string DatabaseConnectionString = "NULL";

        public CustomerRequestVO GetCustomerRequestByIdentity(int customerRequestIdentity)
        {
            const string selectQueryStatement = "SELECT * FROM CUS_REQ WHERE CUS_REQ_I = @CUS_REQ_I";

            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@CUS_REQ_I", customerRequestIdentity);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }

                var customerRequestByIdentity = BuildCustomerRequests(queryResult).FirstOrDefault();

                return customerRequestByIdentity;
            }
        }

        public IEnumerable<CustomerRequestVO> GetCustomerRequestsByReferenceNumber(string referenceNumber)
        {
            const string selectQueryStatement = "SELECT A.* FROM CUS_REQ A, REQ_ETY_REF_NBR B WHERE "
                                    + "B.ETY_NM = 'CUS_REQ' AND B.ETY_KEY_I = A.CUS_REQ_I AND "
                                    + "B.REF_NBR = @REF_NBR";

            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@REF_NBR", referenceNumber);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }

                var customerRequestByIdentity = BuildCustomerRequests(queryResult);
                return customerRequestByIdentity;
            }
        }

        public IEnumerable<CustomerRequestVO> GetCustomerRequestsByReferenceNumberAndBusinessName(string referenceNumber, string businessName)
        {
            const string selectQueryStatement = "SELECT A.* FROM CUS_REQ A, REQ_ETY_REF_NBR B WHERE "
                        + "A.BUS_UNT_KEY_CH = @BUS_UNT_KEY_CH AND B.ETY_NM = 'CUS_REQ' "
                        + "AND B.ETY_KEY_I = A.CUS_REQ_I AND B.REF_NBR = @REF_NBR";


            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@REF_NBR", referenceNumber);
                    queryCommand.Parameters.AddWithValue("@BUS_UNT_KEY_CH", businessName);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }

                var customerRequestByIdentity = BuildCustomerRequests(queryResult);
                return customerRequestByIdentity;
            }
        }

        private IEnumerable<CustomerRequestVO> BuildCustomerRequests(DataTable dataTable)
        {
            var customerRequests = new List<CustomerRequestVO>();

            foreach (DataRow currentRow in dataTable.Rows)
            {
                var customerRequest = new CustomerRequestVO
                {
                    Identity = (int)currentRow["CUS_REQ_I"],
                    Status = currentRow["PRS_STT"].ToString(),
                    BusinessEntityKey = currentRow["BUS_UNT_ETY_NM"].ToString(),
                    TypeCode = currentRow["REQ_TYP_C"].ToString(),
                    ConsumerClassificationType = currentRow["CNSM_CLS"].ToString(),
                    CreatedDate = (DateTime?)currentRow["CRT_S"],
                    CreatedUserId = currentRow["CRT_UID"].ToString(),
                    CreatedProgramCode = currentRow["CRT_PGM_C"].ToString(),
                    LastUpdatedDate = (DateTime?)currentRow["LST_UPD_S"],
                    LastUpdatedUserId = currentRow["LST_UPD_UID"].ToString(),
                    LastUpdatedProgramCode = currentRow["LST_UPD_PGM_C"].ToString()
                };

                customerRequest.Appointments = GetAppointmentsByCustomerRequest(customerRequest).ToList();
                customerRequest.Comments = GetCommentsByCustomerRequest(customerRequest).ToList();
                customerRequest.ReferenceNumbers = GetReferenceNumbersByCustomerRequest(customerRequest).ToList();
                customerRequest.Stops = GetStopsByCustomerRequest(customerRequest).ToList();
                customerRequests.Add(customerRequest);
            }
            return customerRequests;
        }


        private IEnumerable<AppointmentVO> GetAppointmentsByCustomerRequest(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_SCH WHERE ETY_NM = 'CUS_REQ' AND ETY_KEY_I = @CUS_REQ_I";


            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@CUS_REQ_I", customerRequest.Identity);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }

                var AppointmentsByCustomerRequest = BuildAppointments(queryResult);
                return AppointmentsByCustomerRequest;
            }
        }

        private IEnumerable<AppointmentVO> BuildAppointments(DataTable dataTable)
        {
            var Appoinments = new List<AppointmentVO>();

            foreach (DataRow currentRow in dataTable.Rows)
            {
                var TempAppointment = new AppointmentVO
                {
                    Identity = (int)currentRow["REQ_ETY_SCH_I"],
                    EntityName = currentRow["ETY_NM"].ToString(),
                    EntityIdentity = (int)currentRow["ETY_KEY_I"],
                    SequenceNumber = (int)currentRow["SEQ_NBR"],
                    FunctionType = currentRow["SCH_FUN_TYP"].ToString(),
                    AppointmentBegin = (DateTime?)currentRow["BEG_S"],
                    AppointmentEnd=(DateTime?)currentRow["END_S"],
                    TimezoneDescription=currentRow["TZ_TYP_DSC"].ToString(),

                    Status = currentRow["PRS_STT"].ToString(),                   
                    CreatedDate = (DateTime?)currentRow["CRT_S"],
                    CreatedUserId = currentRow["CRT_UID"].ToString(),
                    CreatedProgramCode = currentRow["CRT_PGM_C"].ToString(),
                    LastUpdatedDate = (DateTime?)currentRow["LST_UPD_S"],
                    LastUpdatedUserId = currentRow["LST_UPD_UID"].ToString(),
                    LastUpdatedProgramCode = currentRow["LST_UPD_PGM_C"].ToString()
                };
                Appoinments.Add(TempAppointment);       
            }

            return Appoinments;
        }


        private IEnumerable<CommentVO> GetCommentsByCustomerRequest(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_CMM WHERE ETY_NM = 'CUS_REQ' AND ETY_KEY_I = @CUS_REQ_I";


            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@CUS_REQ_I", customerRequest.Identity);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }

                var CommentsByCustomerRequest = BuildComments(queryResult);
                return CommentsByCustomerRequest;
            }
        }

        private IEnumerable<CommentVO> BuildComments(DataTable dataTable)
        {
            var Comments = new List<CommentVO>();

            foreach (DataRow currentRow in dataTable.Rows)
            {
                var currentComment = new CommentVO
                {
                   Identity = (int)currentRow["REQ_ETY_CMM_I"],
                   EntityName= currentRow["ETY_NM"].ToString(),
                   EntityIdentity  = (int) currentRow["ETY_KEY_I"],
                   SequenceNumber = (int) currentRow["SEQ_NBR"],
                   CommentText   = currentRow["CMM_TXT"].ToString(),
                   CommentType  = currentRow["CMM_TYP"].ToString(),
                   CreatedDate = (DateTime?)currentRow["CRT_S"],
                   CreatedUserId = currentRow["CRT_UID"].ToString(),
                   CreatedProgramCode = currentRow["CRT_PGM_C"].ToString(),
                   LastUpdatedDate = (DateTime?)currentRow["LST_UPD_S"],
                   LastUpdatedUserId = currentRow["LST_UPD_UID"].ToString(),
                   LastUpdatedProgramCode = currentRow["LST_UPD_PGM_C"].ToString()
                };
                Comments.Add(currentComment);
            }

            return Comments;
        }


        private IEnumerable<ReferenceNumberVO> GetReferenceNumbersByCustomerRequest(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_REF_NBR WHERE ETY_NM = 'CUS_REQ' AND ETY_KEY_I = @CUS_REQ_I";



            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@CUS_REQ_I", customerRequest.Identity);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }

                var ReferenceNumberByCustomerRequest = BuildReferenceNumbers(queryResult);
                return ReferenceNumberByCustomerRequest;
            }
        }

        private IEnumerable<ReferenceNumberVO> BuildReferenceNumbers(DataTable dataTable)
        {
            var References = new List<ReferenceNumberVO>();

            foreach (DataRow currentRow in dataTable.Rows)
            {
                var currentReference = new ReferenceNumberVO
                {
                    Identity = (int)currentRow["REQ_ETY_CMM_I"],
                    EntityName = currentRow["ETY_NM"].ToString(),
                    EntityIdentity =(int) currentRow["ETY_KEY_I"],
                    SoutheasternReferenceNumberType = currentRow["SLU_REF_NBR_TYP"].ToString(),
                    ReferenceNumber=currentRow["REF_NBR"].ToString(),
                    CreatedDate = (DateTime?)currentRow["CRT_S"],
                    CreatedUserId = currentRow["CRT_UID"].ToString(),
                    CreatedProgramCode = currentRow["CRT_PGM_C"].ToString(),
                    LastUpdatedDate = (DateTime?)currentRow["LST_UPD_S"],
                    LastUpdatedUserId = currentRow["LST_UPD_UID"].ToString(),
                    LastUpdatedProgramCode = currentRow["LST_UPD_PGM_C"].ToString()
                };
                References.Add(currentReference);
            }

            return References;
        }


        private IEnumerable<StopVO> GetStopsByCustomerRequest(CustomerRequestVO customerRequest)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_OGN WHERE ETY_NM = 'CUS_REQ' AND ETY_KEY_I = @CUS_REQ_I";

            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@CUS_REQ_I", customerRequest.Identity);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }                
                var StopsByCustomerRequest = BuildStops(queryResult);
                return StopsByCustomerRequest;
            }
        }

        private IEnumerable<StopVO> BuildStops(DataTable dataTable)
        {
            var Stops = new List<StopVO>();

            foreach (DataRow currentRow in dataTable.Rows)
            {
                var currentStop = new StopVO
                {
                    Identity = (int)currentRow["REQ_ETY_OGN_I"],
                    EntityName = currentRow["ETY_NM"].ToString(),
                    EntityIdentity = (int)currentRow["ETY_KEY_I"],
                    StopNumber = currentRow["STP_NBR"].ToString(),
                    RoleType= currentRow["SLU_PTR_RL_TYP_C"].ToString(),
                    OrganizationName  = currentRow["OGN_NM"].ToString(),
                    AddressLine1= currentRow["ADR_LNE_1"].ToString(),
                    AddressLine2= currentRow["ADR_LNE_2"].ToString(),
                    AddressCityName= currentRow["ADR_CTY_NM"].ToString(),
                    AddressStateCode= currentRow["ADR_ST_PROV_C"].ToString(),
                    AddressCountryCode= currentRow["ADR_CRY_C"].ToString(),
                    AddressPostalCode=currentRow["ADR_PST_C_SRG"].ToString(),        
                    CreatedDate = (DateTime?)currentRow["CRT_S"],
                    CreatedUserId = currentRow["CRT_UID"].ToString(),
                    CreatedProgramCode = currentRow["CRT_PGM_C"].ToString(),
                    LastUpdatedDate = (DateTime?)currentRow["LST_UPD_S"],
                    LastUpdatedUserId = currentRow["LST_UPD_UID"].ToString(),
                    LastUpdatedProgramCode = currentRow["LST_UPD_PGM_C"].ToString()
                };
                currentStop.Appointments = GetAppointmentsByStop(currentStop).ToList();
                currentStop.Comments = GetCommentsByStop(currentStop).ToList();
                Stops.Add(currentStop);
            }
            return (Stops);
        }


        private IEnumerable<AppointmentVO> GetAppointmentsByStop(StopVO stop)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_SCH WHERE ETY_NM = 'REQ_ETY_OGN' AND ETY_KEY_I = @REQ_ETY_OGN_I";

            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@REQ_ETY_OGN_I", stop.Identity);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }
                var AppointmentsByStop = BuildAppointments(queryResult);
                return AppointmentsByStop;
            }
        }

        private IEnumerable<CommentVO> GetCommentsByStop(StopVO stop)
        {
            const string selectQueryStatement = "SELECT * FROM REQ_ETY_CMM WHERE ETY_NM = 'REQ_ETY_OGN' AND ETY_KEY_I = @REQ_ETY_OGN_I";


            using (SqlConnection defaultSqlConnection = new SqlConnection(DatabaseConnectionString))
            {
                defaultSqlConnection.Open();
                DataTable queryResult = new DataTable();

                using (SqlCommand queryCommand = new SqlCommand(selectQueryStatement, defaultSqlConnection))
                {
                    queryCommand.Parameters.AddWithValue("@REQ_ETY_OGN_I", stop.Identity);
                    var sqlReader = queryCommand.ExecuteReader();
                    queryResult.Load(sqlReader);
                }
                var CommentsByStop = BuildComments(queryResult);
                return CommentsByStop;
            }
        }
    }
}
