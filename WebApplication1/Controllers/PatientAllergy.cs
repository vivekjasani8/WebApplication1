using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Transactions;
using WebApplication1.models;

namespace WebApplication1.Controllers
{
    public class PatientAllergy : Controller
    {
        private readonly IConfiguration _configuration;
        public PatientAllergy(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string query = @"select * from Patient_GetById(@id);";

            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpGet]
        [Route("patientgetlist")]
        public JsonResult Get()
        {
            string query = @"select * from patient_getlist();";

            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                {
/*                    myCommand.Parameters.AddWithValue("@id", id);
*/                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        [Route("patientinsert")]
        public JsonResult Post(patientAllergy pa)
        {
            string query = $"select * from patient_allergy_create('{pa.PatientId}','{pa.AllergyMasterId}')";

            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                {
                    using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        try
                        {
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);
                            myReader.Close();

                            txscope.Complete();
                            txscope.Dispose();
                        }
                        catch (Exception ex)
                        {
                            txscope.Dispose();
                            return new JsonResult(ex.Message);
                        }
                        finally
                        {
                            mycon.Close();
                        }
                    }
                }
            }
            return new JsonResult(table);
        }
    }
}
