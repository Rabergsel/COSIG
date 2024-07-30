using System.Text.Json;

namespace COSIG
{
    public class APIObject
    {
        public string type { get; set; }
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
