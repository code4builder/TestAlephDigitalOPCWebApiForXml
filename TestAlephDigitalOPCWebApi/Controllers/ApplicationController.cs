using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public IActionResult GetNodeInXmlById(int id)
        {
            var xElement = Utilities.GetXElementById(id);

            var xDocument = Utilities.CreateNewXDocumentWithNode(xElement);

            string xmlString = xDocument.ToString();

            return xElement == null ? NotFound() : Content(xmlString, "text/xml", System.Text.Encoding.UTF8);
        }

        [HttpGet("query")]
        // GET: Returns the full "node" in the native xml format. Application/item/{id}.
        // Also it allows to return a filtered list by the query params: NodeClass and BrowseName
        public IActionResult GetAllNodeInJson([FromQuery] string? nodeClass, string? browseName)
        {
            Utilities.PrepareXmlMinInfo();

            if (nodeClass != null)
            {
                var listNodesByNodeClass = Utilities.FilterByNodeClass(nodeClass);
                var jsonListNodesByNodeClass = JsonConvert.SerializeObject(listNodesByNodeClass, Newtonsoft.Json.Formatting.Indented);
               return Ok(jsonListNodesByNodeClass);
            }
            else if (browseName != null)
            {
                var listNodesByNodeClass = Utilities.FilterByBrowseName(browseName);
                var jsonListNodesByNodeClass = JsonConvert.SerializeObject(listNodesByNodeClass, Newtonsoft.Json.Formatting.Indented);
                return Ok(jsonListNodesByNodeClass);
            }
            else
            {
                return Ok(Utilities.XmlToJson("Data/allNodesForJson.xml"));
            }
        }

        [HttpGet("test")]
        // Starting page
        public ActionResult TestApi()
        {
            return Ok("Web server is launched at " + DateTime.Now);
        }
    }
}
