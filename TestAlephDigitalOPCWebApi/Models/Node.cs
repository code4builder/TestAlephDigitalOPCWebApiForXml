using System.Xml.Serialization;

namespace TestAlephDigitalOPCWebApi.Models
{
    public class Node
    {
        [XmlElement("NodeId")]
        public string? NodeId { get; set; }

        [XmlElement("NodeClass")]
        public string? NodeClass { get; set; }

        [XmlElement("BrowseName")]
        public string? BrowseName { get; set; }
        
        [XmlElement("DisplayName")]
        public string? DisplayName { get; set;}
        
        [XmlElement("Description")]
        public string? Description { get; set; }
    }
}
