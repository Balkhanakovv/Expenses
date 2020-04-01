using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database
{
    public class dbNote
    {
        public string name { get; set; } 
        public double cost { get; set; }

        public dbNote() { }

        public dbNote(string name, double cost)
        {
            this.name = name;
            this.cost = cost;
        }
    }
}
