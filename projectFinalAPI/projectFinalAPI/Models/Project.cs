using System.ComponentModel;
using projectFinalAPI.Utils;
using Newtonsoft.Json;


namespace projectFinalAPI.Models
{
    [JsonConverter(typeof(ProjectJsonConverter))]

    public class Project
    {
        public int ID; 
        public DateTime OpenDate { get; set; } 
        public DateTime CloseDate { get; set; }
        public bool IsActive { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public int ClientID { get; set; }

        public override string ToString()
        {
            return $"ShortName: {ShortName} <> Active: {IsActive} <> Dates[Open: {OpenDate} Closed: {CloseDate}]";
        }

    }
}


