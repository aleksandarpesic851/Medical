using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class UserModel
    {
        [Key]
        public int user_id { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
        public string user_name { get; set; }
        public string user_role { get; set; }
        public string user_phone { get; set; }
        public string user_dob { get; set; }
        public string user_address { get; set; }
    }
}
