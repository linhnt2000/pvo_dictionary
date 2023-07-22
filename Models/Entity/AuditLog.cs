using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class AuditLog : BaseModel
    {
        [Key]
        public int audit_log_id { get; set; }
        public int user_id { get; set; }
        public string screen_info { get; set; }
        public int action_type { get; set; }
        public string reference { get; set; }
        public string description { get; set; }
    }
}
