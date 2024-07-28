using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace COSIG
{
    public class APIObject
    {
        public string type {  get; set; }
        public object data { get; set; }

        public APIObject(object data)
        {
            this.data = data;
            type = data.GetType().ToString();
        }

        public bool IsType(Type Type)
        {
            return type == Type.ToString();
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }



    }
}
