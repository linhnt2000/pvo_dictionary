﻿using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class Mode : BaseModel
    {
        [Key]
        public int mode_id { get; set; }
        public int sys_mode_id { get; set; }
        public int user_id { get; set; }
        public string mode_name { get; set; }
        public int mode_type { get; set; }
        public int sort_order { get; set; }

    }
}
