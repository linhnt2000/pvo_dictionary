
using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class SysRegister : BaseModel
    {
        [Key]
        public int sys_register_id { get; set; }
        public string register_name { get; set; }
        public int register_type { get; set; }
        public int sort_order { get; set; }
    }
}
