using System.Xml.Serialization;

namespace TestAlephDigitalOPCWebApi.Models
{
    public class Node
    {
        public string? NodeId { get; set; }

        public string? NodeClass { get; set; }

        public string? BrowseName { get; set; }

        public string? DisplayName { get; set;}

        public string? Description { get; set; }
    }
}
