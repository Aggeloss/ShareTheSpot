using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTheSpot.Models
{
    public class Token
    {
        public int Id { set; get; }
        public string access_token { set; get; }
        public string error_description { set; get; }
        public DateTime expire_date { set; get; }
        public int expire_in { set; get; }

        public Token() { }
        
        public string ToString()
        {
            return "access_token: " + this.access_token + " || error_description: " + this.error_description + " || expire_date: " + this.expire_date
                    + " || expire_in: " + this.expire_in;
        }
    }
}
