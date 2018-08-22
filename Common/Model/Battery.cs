using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Battery: Element, INotifyPropertyChanged
    {
       
        private double capacity;
        private bool isCharging;

        public event PropertyChangedEventHandler PropertyChanged;
        public double Capacity
        {
            get
            {
                return this.capacity;
            }
            set
            {
                this.capacity = value;
                this.OnPropertyChanged("Capacity");
            }
        }
        public bool IsCharging
        {
            get
            {
                return this.isCharging;
            }
            set
            {
                this.isCharging = value;
                this.OnPropertyChanged("IsCharging");
            }
        }
        public Battery()
        {

        }
        public Battery(string name, double power,double capacity):base(name,power)
        {
            this.capacity = capacity;
            isCharging = false;
        }

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}

