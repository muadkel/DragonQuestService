using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DragonQuestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DragonQuestService" in code, svc and config file together.
    public class DragonQuestService : IDragonQuestService
    {
        public Model.Player GetPlayerXML(string id)
        {
            return GetPlayer(id);
        }

        public Model.Player GetPlayer(string id)
        {
            Model.Player rtnPlayer = new Model.Player();

            rtnPlayer.Id = 1;
            rtnPlayer.Name = "Newb";


            return rtnPlayer;
        }
    }
}
