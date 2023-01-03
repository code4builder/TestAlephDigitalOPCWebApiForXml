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

        //public Node(string nodeId, string nodeClass, string browseName, string displayName, string description)
        //{
        //    NodeId= nodeId;
        //    NodeClass= nodeClass;
        //    BrowseName= browseName;
        //    DisplayName= displayName;
        //    Description= description;
        //}

        //public Node()
        //{

        //}
    }
}
