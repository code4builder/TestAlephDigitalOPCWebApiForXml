using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using TestAlephDigitalOPCWebApi.Models;

namespace TestAlephDigitalOPCWebApi
{
    public static class Utilities
    {
        #region Methods ApplicationControleer for requests Application/item/{id} for full node in native xml format

        /// <summary>
        /// Retrieve XElement by NodeId
        /// </summary>
        /// <param name="nodeId">NodeId from OPC Standard Xml file</param>
        /// <returns>XElement</returns>
        public async static Task<XElement> GetXElementByIdAsync(int nodeId)
        {
            await Task.Delay(0);

            XElement xmlElement = XElement.Load("Data/Opc.Ua.Di.NodeSet2.xml");

            IEnumerable<XElement> nodes = xmlElement.Elements();

            string completeNodeId = "ns=1;i=" + nodeId.ToString();

            var xElement = nodes.FirstOrDefault(x => x.Attribute("NodeId")?.Value == completeNodeId.ToString());

            return xElement;
        }

        /// <summary>
        /// Creates an XDocument with only one node
        /// </summary>
        /// <param name="xElement"></param>
        /// <returns>XDocument</returns>
        public static XDocument CreateNewXDocumentWithNode(XElement xElement) //Make it Async
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("UANodeSet", xElement) //UANodeSet -should be const field
                );

            //Uncomment if you need to save the document
            //doc.Save(Constants.PathOutputNodeXml);

            return doc;
        }
        #endregion

        #region Method Application controller for requests Application/query for retrieve all "node" with the minimum set of info in json format 

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
        ///and lefts only nodes UADataType, UAObject, UAVariable and UA Method with minimal requested information
        /// </summary>
        public static void PrepareXmlMinInfo()
        {
            XElement xmlElement = XElement.Load(Constants.PathInputXml);

            List<XElement> nodes = xmlElement.Elements().ToList();

            //Removing Namespaces Uris, Models and Aliases
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
                new XElement("ArrayOfNode", nodes)
                );

            doc.Save(Constants.PathOutputAllNodesForJson);


            XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";

            XDocument docForDeserialization = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("ArrayOfNode", new XAttribute(XNamespace.Xmlns + "xsi", xsiNs), nodes)
                );

            docForDeserialization.Save(Constants.PathAllNodesForJsonForDeserialization);

            var nodesFromXml = DeserializeFromXML();


        }
        #endregion

        #region Methods for filtering the list by the query parameters: by NodeClass and by BrowseName
        public static List<Node> FilterByNodeClass(string nodeClassValue)
        {
            var nodesFromXml = DeserializeFromXML();
            return nodesFromXml.Where(node => node.NodeClass == nodeClassValue).ToList();
        }


        /// <summary>
        /// Filter by property BrowseName
        /// </summary>
        /// <param name="browseNameValue"></param>
        /// <returns></returns>
        public static List<Node> FilterByBrowseName(string browseNameValue)
        {
            var nodesFromXml = DeserializeFromXML();
            return nodesFromXml.Where(node => node.BrowseName == browseNameValue).ToList();
        }

        private static List<Node> DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Node>));
            TextReader textReader = new StreamReader(Constants.PathAllNodesForJsonForDeserialization);
            List<Node> nodes = new List<Node>();
            nodes = (List<Node>)deserializer.Deserialize(textReader);
            textReader.Close();

            return nodes;
        }
        #endregion
    }
}
