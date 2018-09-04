using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class PriceAndPower
    {
        private double price;
        private double power;
        private DateTime time;

        public double Price
        {
            get
            { return price; }
            set
            { price = value; }
        }
        public double Power
        {
            get
            { return power; }
            set
            { power = value; }
        }
        public DateTime Time

        {
            get
            { return time; }
            set
            { time = value; }
        }
        public PriceAndPower() { }
        public PriceAndPower(double price,double power,DateTime time)
        {
            this.power = power;
            this.price = price;
            this.time = time;
        }


    }
}
