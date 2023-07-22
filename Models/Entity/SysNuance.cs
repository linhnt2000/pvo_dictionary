
using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class SysNuance : BaseModel
    {
        [Key]
        public int sys_nuance_id { get; set; }
        public string nuance_name { get; set; }
        public int nuance_type { get; set; }
        public int sort_order { get; set; }
    }
}
