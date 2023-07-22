﻿
using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class SysTone : BaseModel
    {
        [Key]
        public int sys_tone_id { get; set; }
        public string tone_name { get; set; }
        public int tone_type { get; set; }
        public int sort_order { get; set; }
    }
}
