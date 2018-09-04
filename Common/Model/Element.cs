using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
  
    public class Element
    {
        private string name;
        private double power;
        public string Name { get => name; set => name = value; }
        public double Power { get => power; set => power = value; }

        public Element()
        {

        }
        public Element(string name,double power)
        {
            this.name = name;
            this.power = power;
        }
    }
}
