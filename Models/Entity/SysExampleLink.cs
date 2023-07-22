
using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class SysExampleLink : BaseModel
    {
        [Key]
        public int sys_example_link_id { get; set; }
        public string example_link_name { get; set; }
        public int example_link_type { get; set; }
        public int sort_order { get; set; }
    }
}
