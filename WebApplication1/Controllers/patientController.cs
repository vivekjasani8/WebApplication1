using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Linq.Expressions;
using System.Transactions;
using WebApplication1.models;
using CoreApiResponse;
//namespace ExceptionHandling.Services
namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {      
            string query = @"select * from get_patient_byid(@id);";

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
        [HttpPost]
        [Route("patientget")]
        public JsonResult Post(Getpatient gp)
        {
            int flag = 0;
            string query = $"select * from patientget(";
            if (gp.patient_id != null)
            {
                query += $"patient_id=>'{gp.patient_id}'";
                flag = 1 ; 
            }
            if (gp.firstname != null)
            {
                if (flag == 1) {

                    query += $",";
                }
                query += $"fname=>'{gp.firstname}'";
                flag = 1 ;
            }
            if (gp.lastname != null && gp.lastname != "")
            {
                if (flag == 1)
                {
                    query += $",";
                }
                query += $"lname=>'{gp.lastname}'";
                flag = 1;
            }
            if (gp.sex_type != null && gp.sex_type != "")
            {
                if (flag == 1)
                {
                    query += $",";
                }
                query += $"sextype=>'{gp.sex_type}'";
                flag = 1;
            }
            if (gp.dob != null)
            {
                if (flag == 1)
                {
                    query += $",";
                }
                query += $"p_dob=>'{gp.dob}'";
                flag = 1;
            }
            if(gp.sorttype != null && gp.sorttype!="")
            {
                if (flag == 1) { query += $","; }
                query += $"sorttype=>'{gp.sorttype}'";
                flag = 1;
            }
            if (gp.page_no != null )
            {
                if (flag == 1) { query += $","; }
                query += $"page_no=>'{gp.page_no}'";
                flag = 1;
            }
            if(gp.page_size != null )
            {
                if (flag == 1) { query += $","; }
                query += $"page_size=>'{gp.page_size}'";
            }
            query += $")";
         
            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }

        //to insert new data for new patien in data base
        [HttpPost]
        [Route("patientinsert")]
        public JsonResult Post(patient pat )
        {
            /*if( pat.lastname == null || pat.firstname== null || pat.middlename ==null || pat.sex_type == null || pat.dob == null )
            {
                return BadRequest();
            }*/
            string query = $"select * from patientcreate(fname=>'{pat.firstname}',lname=>'{pat.lastname}',mname=>'{pat.middlename}',sextype=>'{pat.sex_type}',dob=>'{pat.dob}') ";

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


        //to soft delete patient data from patient data
        [HttpDelete]
        [Route("patientdelete")]
        public JsonResult Delete(patient p)
        {
            string query = $"select * from patientdelete('{p.patient_id}')";

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
                        finally { mycon.Close(); }
                    }
                }
            }
            return new JsonResult("deleted successfully");
        }


        //to update patient data in a patient data
        [HttpPut]
        [Route("patientupdate")]
        public JsonResult Put(patient p)
        {
            string query = $"select * from patientupdate('{p.patient_id}','{p.firstname}'," +
                $"'{p.lastname}','{p.middlename}','{p.sex_type}','{p.dob}')";

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
                            //mycon.Close();
                            
                            txscope.Complete();
                            txscope.Dispose();
                        }
                        catch(Exception ex)
                        {
                            txscope.Dispose();
                            return new JsonResult(ex.Message);
                        }
                        finally { mycon.Close(); }
                    }
                }
            }
            return new JsonResult("updated successfully");
        }
    }
}
