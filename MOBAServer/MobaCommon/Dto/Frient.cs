using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobaCommon.Dto
{
    public class Frient
    {
        public int Id;
        public string Name;
        public bool IsOnline;

        public Frient()
        {

        }

        public Frient(int id,string name, bool online)
        {
            this.Id = id;
            this.Name = name;
            this.IsOnline = online;
        }
    }
}
