namespace WebApplication1.models
{
    public class patient
    {
        public int patient_id { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
        public string? sex_type { get; set; }
        public string? chart_no { get; set; }
        public DateTime? dob { get; set; }
      
    }
    public class Getpatient
    { 
        public int? patient_id { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? sex_type { get; set; }
        public DateTime? dob { get; set; }
        public string? sorttype { get; set; }
        public int? page_no { get; set; }
        public int? page_size { get; set; }
    }
}
