using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.models
{
    public class patientallergy
    {
        public int? patientid { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
        public string? sextype { get; set; }
        public string? dob { get; set; }
        //public string? transaction { get; set; }
        public patientallergyinsert[]? created { get; set; }
        public patientupdate[]? updated { get; set; }
        public List<patientdelete>? deleted { get; set; }
    }
    public class patientallergyinsert
    {
        public int? PatientId { get; set; }
        public string? Allergy { get; set;}
        public string? Note { get; set; }
    }
    public class patientupdate
    {
        public int? PatientAllergyId { get; set; }
        public int? PatientId { get; set; }
        public string? Allergy { get; set; }
        public string? Note { get; set; }
        public string? transaction { get;set; }
    }
    public class patientpatch
    {
        public int PatientAllergyId { get; set; }
        public int? PatientId { get; set; }
        public string? Allergy { get; set; }
        public string? Note { get; set; }
    }
    public class patientdelete
    {
        [DefaultValue(0)]
        public int? PatientAllergyId { get; set; }
    }
    public class delete
    {
        public int? patientid { get; set; }
    }
    public class getlist
    {
        
        public string? sorttype { get; set; }
        public int? page_size { get; set; }
        public int? page_no { get; set; }
    }
    public class patientinfo
    {
        public int? patientid { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public List<patientallergyinfo>? allergys { get; set; }
    }
    public class patientallergyinfo
    {
        public int? PatientAllergyId { get; set; }
        public int? AllergyId { get; set; }
        public string? Note { get; set;}
    }
    public class function
    { 
        public bool check_null { get; set; }
    }
 
}
