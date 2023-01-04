using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;
using TestAlephDigitalOPCWebApi;
using TestAlephDigitalOPCWebApi.Models;

namespace TestAlephDigitalOPCWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ApplicationController : ControllerBase
    {
        [HttpGet("item/{id}")]
        // GET: Returns the full "node" in the native xml format. Application/item/{id}
        public ActionResult GetNodeInXmlById(int id)
        {
            var xElement = Utilities.GetXElementById(id);

            var xDocument = Utilities.CreateNewXDocumentWithNode(xElement);

            string xmlString = xDocument.ToString();

            return xElement == null ? NotFound() : Content(xmlString, "text/xml", System.Text.Encoding.UTF8);
        }

        [HttpGet("query")]
        // GET: Returns the full "node" in the native xml format. Application/item/{id}
        public ActionResult<string> GetAllNodeInJson()
        {
            Utilities.PrepareXmlMinInfo();

            string jsonString = Utilities.XmlToJson("Data/allNodesForJson.xml");

            return jsonString;
        }

        [HttpGet("test")]
        // Starting page
        public ActionResult TestApi()
        {
            return Ok("Web server is launched at " + DateTime.Now);
        }
    }
}
