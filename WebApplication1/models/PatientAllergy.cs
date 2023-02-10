namespace WebApplication1.models
{
    public class patientAllergy
    {
        public int PatientAllergyId { get; set; }
        public int PatientId { get; set; }
        public int AllergyMasterId { get; set;}
    }
    public class AllergyMaster
    {
        public int AllergyMasterId { get; set; }
        public int? code { get; set; }
        public string? Note { get; set; }
    }
    /*public class Patient
    {
        public int PatientId { get; set; }
    }*/
}
