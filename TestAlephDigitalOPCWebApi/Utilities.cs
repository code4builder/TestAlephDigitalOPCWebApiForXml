using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using TestAlephDigitalOPCWebApi.Models;

namespace TestAlephDigitalOPCWebApi
{
    public static class Utilities
    {
        /// <summary>
        /// Retrieve XElement by NodeId
        /// </summary>
        /// <param name="nodeId">NodeId from OPC Standard Xml file</param>
        /// <returns>XElement</returns>
        public static XElement GetXElementById(int nodeId)
        {
            {
                XElement xmlElement = XElement.Load("Data/Opc.Ua.Di.NodeSet2.xml");

                IEnumerable<XElement> nodes = xmlElement.Elements();

                string completeNodeId = "ns=1;i=" + nodeId.ToString();

                var xElement = nodes.FirstOrDefault(x => x.Attribute("NodeId")?.Value == completeNodeId.ToString());

                return xElement;
            }
        }

        /// <summary>
        /// Creates an XDocument with only one node
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns>XDocument</returns>
        public static XDocument CreateNewXDocumentWithNode(XElement xElement)
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("UANodeSet", xElement)
                );

            doc.Save("Data/xmlNode.xml");

            return doc;
        }

        /// <summary>
        /// Converts Xml to Json
        /// </summary>
        /// <returns>string in json format</returns>
        public static string XmlToJson(string path)
        {
            XDocument xmlDoc = XDocument.Load(path);

            var elements = xmlDoc.Root.Elements();

            var jsonDoc = JsonConvert.SerializeXNode(xmlDoc.Root, Newtonsoft.Json.Formatting.Indented, true);

            var newJsonDocWithoutRoot = JsonConvert.SerializeObject(jsonDoc, Newtonsoft.Json.Formatting.Indented);

            return jsonDoc;
        }

        /// <summary>
        ///Method for removing all unnecessary stuff (models, comments, aliases, namespaces)
        ///and lefts only nodes: UADataType, UAObject, UAVariable and UA Method
        /// </summary>
        public static void PrepareXmlMinInfo()
        {
            XElement xmlElement = XElement.Load("Data/Opc.Ua.Di.NodeSet2.xml");

            List<XElement> nodes = xmlElement.Elements().ToList();

            nodes.RemoveAt(0);
            nodes.RemoveAt(0);
            nodes.RemoveAt(0);

            foreach (var element in nodes.Descendants().ToList())
            {
                if (element.Name.LocalName != "DisplayName" && element.Name.LocalName != "Description")
                {
                    element.Remove();
                }

                //Removing namespaces
                element.Name = element.Name.LocalName;
            }

            foreach (var node in nodes)
            {
                var attributeBrowseName = node.Attributes().FirstOrDefault(x => x.Name == "BrowseName");
                if (attributeBrowseName != null)
                    node.AddFirst(new XElement(attributeBrowseName.Name.LocalName, attributeBrowseName.Value));

                node.AddFirst(new XElement("NodeClass", node.Name.LocalName));

                var attributeNodeId = node.Attributes().FirstOrDefault(x => x.Name == "NodeId"); 
                if (attributeNodeId != null)
                    node.AddFirst(new XElement(attributeNodeId.Name.LocalName, attributeNodeId.Value));

                //Removing namespaces
                node.Name = "Node";

                node.RemoveAttributes();
            }

            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("UANodeSet", nodes)
                );

            doc.Save("Data/allNodesForJson.xml");
        }

        //public static List<Node> XmlToList(string path)
        //{
        //    //XDocument xmlElement = XDocument.Load("Data/allNodesForJson.xml");

        //    //var root = xmlElement.Root;

        //    List<Node> nodesList = new List<Node>();

        //    XElement xmlElement = XElement.Load("Data/allNodesForJson.xml");

        //    List<XElement> nodesXElement = xmlElement.Elements().ToList();

        //    foreach (var xElement in nodesXElement)
        //    {
        //        nodesXElement.Add(new Node { xElement.});
        //    }
        //}

        public static List<Node> DeserializeFromXML()
        {
            //XElement xmlElement = XElement.Load("Data/allNodesForJson.xml");

            //List<XElement> nodes = xmlElement.Elements().ToList();

            //XNamespace xNamespace = "http://www.w3.org/2001/XMLSchema-instance";

            //XDocument docWithNamespaces = new XDocument(
            //    new XDeclaration("1.0", "utf-8", "yes"),
            //    new XElement(xNamespace + "UANodeSet", 
            //    nodes)
            //    );
            //docWithNamespaces.Save("Data/allNodesWithNamespacesForJson.xml");


            string path = Path.Combine(Environment.CurrentDirectory, "allNodesWithNamespacesForJson.xml");

            XmlSerializer deserializer = new XmlSerializer(typeof(List<Node>));

            List<Node> nodes = new List<Node>();

            using (FileStream stream = File.Open("Data/allNodesWithNamespacesForJson.xml", FileMode.Open))
            {
                nodes = (List<Node>)deserializer.Deserialize(stream);
            }

            return nodes;
        }
    }
}
