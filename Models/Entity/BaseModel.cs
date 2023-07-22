using System;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class BaseModel
    {
        public DateTime created_date { get; set; } = DateTime.Now;
        public DateTime updated_date { get; set; }
    }
}
