using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Consumer :Element, INotifyPropertyChanged
    {
        private bool isActive;

        public bool IsActive { get => isActive; set => isActive = value; }

        public Consumer()
        {

        }
        public Consumer(string name, double maxPower):base(name,maxPower)
        {
            isActive = true;
        }

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;



    }
}