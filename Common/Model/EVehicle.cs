using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class EVehicle:Element, INotifyPropertyChanged
    {
        private double remainingCapacity;

        public event PropertyChangedEventHandler PropertyChanged;

        public double RemainingCapacity
        {
            get
            {
                return this.remainingCapacity;
            }
            set
            {
                this.remainingCapacity = value;
                OnPropertyChanged("RemainingCapacity");
            }
        }
        public EVehicle(string name,double power):base(name,power)
        {
            this.remainingCapacity = power;
        }
        public EVehicle()
        {

        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
