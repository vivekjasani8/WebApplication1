using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using System.Data;
using System.Transactions;
using WebApplication1.models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        public bool check_null(string? attribute)
        {
            if(attribute == null || attribute == "") { return false; }
            return true;
        }
        
        /// <summary>
        /// this get api is created to get single patient data through patient id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> it return the patient data and its allergys</returns>
        [HttpGet("{id}")]
        public patientinfo Get(int id)
        {
            patientinfo patient = new patientinfo();
            List<patientallergyinfo> p = new List<patientallergyinfo>();
            string _query = @"select * from patientget(patient_id=>" + id+ ");";
            Console.WriteLine(_query);
            string query = @"select * from patient_allergy_get(patient_id=>" + id + " );";

            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(_query, mycon))
                {
                    NpgsqlDataReader myReader = myCommand.ExecuteReader();
                    while(myReader.Read())
                    {
                        patient.patientid = Convert.ToInt32(myReader.GetValue(0).ToString());
                        patient.firstname = myReader.GetValue(1).ToString();
                        patient.lastname = myReader.GetValue(2).ToString();
                    }
                    table.Load(myReader);
                    myReader.Close();
                    
                }
                using(NpgsqlCommand mycommand = new NpgsqlCommand(query,mycon))
                {
                    NpgsqlDataReader dr = mycommand.ExecuteReader();
                    while(dr.Read())
                    {
                        patientallergyinfo up = new patientallergyinfo();
                        up.PatientAllergyId = Convert.ToInt32(dr.GetValue(0).ToString());
                        up.AllergyId = Convert.ToInt32(dr[1].ToString());
                        up.Note = dr[2].ToString();
                        p.Add(up);
                    }
                }
                patient.allergys = p.ToList();
                mycon.Close();
            }
            return patient;
        }
        /// <summary>
        /// this get api is used to get patient data with sorttype and pagination
        /// </summary>
        /// <param name="gl"></param>
        /// <returns> patients data and its allergy</returns>
        [HttpGet]
        [Route("getlist")]
        public List<patientinfo> getpatientallergy(getlist gl)
        {
            List<patientinfo> patientallergy = new List<patientinfo>();
            int flag = 0;
            string q = @"select * from patientget(";
            if (check_null(gl.sorttype))
            {
                q += @"sorttype=>'"+ gl.sorttype +"'";
                flag = 1;
            }
            if (gl.page_size != null)
            {
                if (flag == 1)
                { q+= @","; }
                q += @"page_size=>'"+ gl.page_size +"'";
                flag = 1;
            }
            if (gl.page_no != null)
            {
                if (flag == 1)
                { q += @","; }
                q += @"page_no=>'"+ gl.page_no +"'";
                flag = 1;
            }
            q += @")";
            Console.WriteLine(q);
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(q, mycon))
                {
                    NpgsqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        patientinfo pa = new patientinfo();
                        pa.patientid = Convert.ToInt32(r.GetValue(0).ToString());
                        pa.firstname = r.GetValue(1).ToString();
                        pa.lastname = r.GetValue(2).ToString();

                        patientallergy.Add(pa);
                    }
                    r.Close();
                }
                foreach( var i in patientallergy)
                {
                    List<patientallergyinfo> p = new List<patientallergyinfo>();
                    string query = @"select * from patient_allergy_get(patient_id=>" + i.patientid + " );";
                    using (NpgsqlCommand cmd1 = new NpgsqlCommand(query, mycon))
                    {
                        NpgsqlDataReader dr = cmd1.ExecuteReader();
                        while (dr.Read())
                        {
                            patientallergyinfo up = new patientallergyinfo();
                            up.PatientAllergyId = Convert.ToInt32(dr.GetValue(0).ToString());
                            up.AllergyId = Convert.ToInt32(dr[1].ToString());
                            up.Note = dr[2].ToString();
                            p.Add(up);
                        }
                        i.allergys = p.ToList();
                        dr.Close();
                    }
                }
                
            }
            return patientallergy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("patientinsert")]
        public patientallergy Post(patientallergy p)
        {
            patientallergy ans= new patientallergy();
            string q = @"select * from patient_insert(fname=>'" + p.firstname + "',lname=>'" + p.lastname + "',mname=>'" + p.middlename + "',sextype=>'" + p.sextype + "'";
            q += @",_dob=>'" + p.dob + "')";
            int i;
            Console.WriteLine(q);
            //string q1 = @"select * from ";
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            DataTable table = new DataTable();
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {
                        mycon.Open();
                        using (NpgsqlCommand myCommand = new NpgsqlCommand(q, mycon))
                        {
                            NpgsqlDataReader myReader = myCommand.ExecuteReader();
                            table.Load(myReader);
                            i = (int)table.Rows[0][0];
                            myReader.Close();
                        }
                        foreach (var a in p.created)
                        {
                            string q1 = @"select * from patient_allergy_create(patient_id=>" + i + ",allergy=>'" + a.Allergy + "'";
                            if(check_null(a.Note))
                            {
                                q1+= ",note=>'" + a.Note + "'";
                            }
                            q1 += @")";
                            Console.WriteLine(q1);
                            using (NpgsqlCommand cmd = new NpgsqlCommand(q1, mycon))
                            {
                                NpgsqlDataReader myReader = cmd.ExecuteReader();
                                table.Load(myReader);
                                myReader.Close();
                            }
                        }
                        txscope.Complete();
                        txscope.Dispose();
                        ans.patientid = i;
                        ans.firstname = p.firstname;
                        ans.lastname= p.lastname;
                        ans.sextype= p.sextype;
                        ans.dob= p.dob;
                        ans.middlename= p.middlename;
                        ans.created= p.created;
                    }
                    catch (Exception)
                    {
                        txscope.Dispose();
                        //return new JsonResult(ex.Message);
                    }
                    finally { mycon.Close(); }
                }
            }
            return ans;
        }
        
        [HttpPut]
        [Route("patientupdate")]
        public JsonResult put(patientallergy p)
        {
            string query = @"select * from patient_update(patientid=>"+p.patientid+ ",fname=>'"+p.firstname+"',lname=>'"+p.lastname+"',mname=>'"+p.middlename+"'";
            /*if(check_null(p.transaction))
            {
                query+= @",lasttransaction=>'" + p.transaction+"'";
            }*/
            query += @")";
            Console.WriteLine(query);
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {
                        mycon.Open();
                        using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                        {
                            NpgsqlDataReader myReader = myCommand.ExecuteReader();
                            myReader.Close();
                        }
                        foreach (var a in p.created)
                        {
                            string q1 = @"select * from patient_allergy_create(patient_id=>" + p.patientid + ",allergy=>'" + a.Allergy + "'";
                            if (check_null(a.Note))
                            {
                                q1 += ",note=>'" + a.Note + "'";
                            }
                            q1 += @")";
                            Console.WriteLine(q1);
                            using (NpgsqlCommand myCommand = new NpgsqlCommand(q1, mycon))
                            {
                                NpgsqlDataReader r=myCommand.ExecuteReader();
                                r.Close();
                            }
                        }
                        foreach (var a in p.updated)
                        {
                            string q1 = @"select * from patient_allergy_update(patientallergy_id=>" + a.PatientAllergyId + ",patient_id=>" + p.patientid + ",allergy=>'" + a.Allergy + "'";
                            if (check_null(a.Note))
                            {
                                q1 += ",note=>'" + a.Note + "'";
                            }
                            if (check_null(a.transaction))
                            {
                                q1 += ",lasttransaction=>'" + a.transaction + "'";
                            }
                            q1 += @")";
                            Console.WriteLine(q1);
                            using (NpgsqlCommand myCommand = new NpgsqlCommand(q1, mycon))
                            {

                                myCommand.ExecuteNonQuery();
                            }
                        }
                        foreach (var a in p.deleted)
                        {
                            string q1 = @"select * from patient_allergy_delete(patientallergy_id=>" + a.PatientAllergyId + ")";
                            Console.WriteLine(q1);
                            using (NpgsqlCommand myCommand = new NpgsqlCommand(q1, mycon))
                            {
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        txscope.Complete();
                        txscope.Dispose();
                        }
                    catch (Exception ex)
                    {
                            txscope.Dispose();
                            return new JsonResult(ex.Message);
                    }
                }
            }
            return new JsonResult(p);

        }

        [HttpDelete]
        [Route("patientdelete")]
        public delete Delete(delete p)
        {
            string q = @"select * from _delete_patient_allergy(_patient_id=>" + p.patientid + ")";
            Console.WriteLine(q);
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(q, mycon))
                {
                    using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        try
                        {
                            myCommand.ExecuteNonQuery();
                            txscope.Complete();
                            txscope.Dispose();
                        }
                        catch (Exception )
                        {
                            txscope.Dispose();
                        }
                        finally
                        {
                            mycon.Close();
                        }
                    }
                }
            }
            return p;
        }

        [HttpPatch]
        [Route("patientpatch")]
        public JsonResult Patch(patientallergy p)
        {
            string query = @"select * from patient_patch(patientid=>"+p.patientid+"";
            if(check_null(p.firstname))
            {
                query += @",fname=>'" + p.firstname + "'";
            }
            if (check_null(p.middlename))
            {
                query += @",lname=>'" + p.lastname + "'";
            }
            if (check_null(p.lastname))
            {
                query += @",mname=>'" + p.middlename + "'";
            }
            if (check_null(p.sextype))
            {
                query += @",sextype=>'" + p.sextype + "'";
            }
            if(check_null(p.dob))
            {
                query += @",dob=>'" + p.dob + "'";
            }
            query += @")";
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {
                        mycon.Open();
                        using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                        {
                            NpgsqlDataReader myReader = myCommand.ExecuteReader();
                            myReader.Close();
                        }
                        foreach (var a in p.created)
                        {
                            string q1 = @"select * from patient_allergy_create(patient_id=>" + p.patientid + ",allergy=>'" + a.Allergy + "'";
                            if (check_null(a.Note))
                            {
                                q1 += ",note=>'" + a.Note + "'";
                            }
                            q1 += @")";
                            Console.WriteLine(q1);
                            using (NpgsqlCommand myCommand = new NpgsqlCommand(q1, mycon))
                            {
                                NpgsqlDataReader r = myCommand.ExecuteReader();
                                r.Close();
                            }
                        }
                        foreach (var a in p.updated)
                        {
                            string q1 = @"select * from patient_allergy_update(patientallergy_id=>" + a.PatientAllergyId + ",patient_id=>" + a.PatientId + ",allergy=>'" + a.Allergy + "'";
                            if (check_null(a.Note))
                            {
                                q1 += ",note=>'" + a.Note + "'";
                            }
                            if (check_null(a.transaction))
                            {
                                q1 += ",lasttransaction=>'" + a.transaction + "'";
                            }
                            q1 += @")";
                            Console.WriteLine(q1);
                            using (NpgsqlCommand myCommand = new NpgsqlCommand(q1, mycon))
                            {

                                myCommand.ExecuteNonQuery();
                            }
                        }
                        foreach (var a in p.deleted)
                        {
                            string q1 = @"select * from patient_allergy_delete(patientallergy_id=>" + a.PatientAllergyId + ")";
                            Console.WriteLine(q1);
                            using (NpgsqlCommand myCommand = new NpgsqlCommand(q1, mycon))
                            {
                                myCommand.ExecuteNonQuery();
                            }
                        }
                        txscope.Complete();
                        txscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        txscope.Dispose();
                        return new JsonResult(ex.Message);
                    }
                }
            }
            return new JsonResult(p);
        }
        /// <summary>
        /// this post method used to insesrt patient allergy data into patient allergy table
        /// </summary>
        /// <param name="pa"></param>
        /// <returns>it return patientallergy id of created patient</returns>
        /*[HttpPost]
        [Route("patientinsert")]
        public JsonResult Post(patientallergyinsert pa)
        {
            *//*            Console.WriteLine(pa.Patient_Id);
            *//*
            string query = $"select * from patient_allergy_create('{pa.PatientId}'";
            if(check_null(pa.Allergy))
            {
                query+= $",'{pa.Allergy}'";
            }
            if(check_null(pa.Note))
            {
                query+=$",'{pa.Note}'";
            }
            query+=$")";
            Console.WriteLine(query);
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
        }*/
        /*/// <summary>
        /// this put method is used to update a patient allergy into the patientallergy table
        /// </summary>
        /// <param name="pa"></param>
        /// <returns>it return updated patientallergyid </returns>
        [HttpPut]
        [Route("patientupdate")]
        public int Put(patientupdate pa)
        {
            string query = @"select * from patient_allergy_update(patientallergy_id=>'" + pa.PatientAllergyId + "'";
            if (pa.PatientId != null)
            {
                query += @",patient_id=>'" + pa.PatientId + "'";
            }
            if (check_null(pa.Allergy))
            {
                query += @",allergy=>'" + pa.Allergy + "'";
            }

            if (check_null(pa.Note))
            {
                query += @",note=>'" + pa.Note + "'";
            }
            if (check_null(pa.transaction))
            {
                query += @",lasttransaction=>'" + pa.transaction + "'";
            }
            query += @")";
            Console.WriteLine(query);
            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                {
                    using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        try
                        {
                            myCommand.ExecuteNonQuery();

                            txscope.Complete();
                            txscope.Dispose();
                        }
                        catch (Exception)
                        {
                            txscope.Dispose();
                        }
                        finally
                        {
                            mycon.Close();
                        }
                    }
                }
            }
            return pa.PatientAllergyId;
        }*/
        /*[HttpDelete]
        [Route("patientdelete")]
        public bool Delete(patientdelete p)
        {
            string query = $"select * from patient_allergy_delete('{p.PatientAllergyId}')";
            if (p.PatientAllergyId == 0)
            {
                return false;
            }
            Console.WriteLine(query);
            DataTable table = new DataTable();
            string? sqlDataSource = _configuration.GetConnectionString("WebApiDatabase");

            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, mycon))
                {
                    using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        try
                        {
                            myCommand.ExecuteNonQuery();

                            txscope.Complete();
                            txscope.Dispose();
                        }
                        catch (Exception)
                        {
                            txscope.Dispose();
                        }
                        finally { mycon.Close(); }
                    }
                }
            }
            return true;
        }*/
        /*/// <summary>
        /// this method is used to patch the patent data into the patientallergy
        /// </summary>
        /// <param name="pa"></param>
        /// <returns>it return the patientallergyid</returns>
        [HttpPatch]
        [Route("patientpatch")]
        public JsonResult Patch(patientpatch pa)
        {
            string query = @"select * from patient_allergy_patch(patientallergy_id=>'"+pa.PatientAllergyId+"'";
            if (pa.PatientId != null)
            {
                query += @",patient_id=>'"+pa.PatientId+"'";
            }
            if (check_null(pa.Allergy))
            {
                query += @",allergy=>'"+pa.Allergy+"'";
            }
            if (check_null(pa.Note))
            {
                query += @",note=>'"+ pa.Note+"'";
            }
            query += @")";
            Console.WriteLine(query);
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
                            //myCommand.ExecuteNonQuery();

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
        }*/
    }
}
