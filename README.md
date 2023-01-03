# TestAlephDigitalOPCWebApi

This Web Api provides a REST API interface to query the attached XML (Data\Opc.Ua.Di.NodeSet2.xml).

The XML is made by “node” elements which are identified by “NodeId” field.

There are several type of “node”:
- UADataType
- UAObject
- UAVariable
- UAMethod

All these kinds of node share common information because they derive from UANode (see XML schema definition referenced by the shared XML file for more details).

The API shall expose the following endpoints:

### /item/{node-id} </br>
Returns the full “node” in the native format i.e. “application/xml”

### /query</br>
The output will be all “node” with the minimum set of information in json format (application/json):
 - NodeId
 - NodeClass (Variable, DataType, Object, Method, …)
 - BrowseName
 - DisplayName
 - Description</br></br>
It allows to return a filtered list of the above output by the following query params :
 - NodeClass
 - BrowseName
