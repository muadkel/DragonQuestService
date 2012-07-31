using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DragonQuestService.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public Player()
        {
            Id = 0;
            Name = "N/A";
        }

    }
}