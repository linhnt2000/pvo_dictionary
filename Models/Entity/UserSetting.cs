using System.ComponentModel.DataAnnotations;

namespace thu6_pvo_dictionary.Models.Entity
{
    public class UserSetting : BaseModel
    {
        [Key]
        public int user_setting_id { get; set; }
        public int user_id { get; set; }
        public string setting_key { get; set; }
        public string setting_value { get; set; }

    }
}
