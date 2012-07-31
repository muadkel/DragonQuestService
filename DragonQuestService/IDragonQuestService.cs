using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DragonQuestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDragonQuestService" in both code and config file together.
    [ServiceContract]
    public interface IDragonQuestService
    {
        //For the iOS client use JSON as the standard so the Naming convection will append XML if an additional feed type is required



        //Get a player object by ID
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "player/{id}")]
        Model.Player GetPlayer(string id);
        //XML Version
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, UriTemplate = "playerxml/{id}")]
        Model.Player GetPlayerXML(string id);


    }
}
