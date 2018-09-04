using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Battery : Element, INotifyPropertyChanged
    {
      
        private double capacity;
        private bool isCharging;
        private bool isDisCharging;
     
        private double remainingCapacity;

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
        public bool IsDisCharging
        {
            get
            {
                return this.isDisCharging;
            }
            set
            {
                this.isDisCharging = value;
                this.OnPropertyChanged("IsDisCharging");
            }
        }
        public double RemainingCapacity
        {
            get
            {
                return this.remainingCapacity;
            }
            set
            {
                this.remainingCapacity = value;
                this.OnPropertyChanged("RemainingCapacity");
            }
        }
        public Battery()
        {

        }
        public Battery(string name, double power, double capacity) : base(name, power)
        {
            this.capacity = capacity;
            this.isCharging = false;
            this.isDisCharging = false;
            this.remainingCapacity = capacity;
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

