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
    public class SolarPanel:Element, INotifyPropertyChanged
    {
        
        private double curentPower;

        public double CurentPower
        {
            get
            {
                return this.curentPower;
            }
            set
            {
                this.curentPower = value;
                this.OnPropertyChanged("CurentPower");
            }
        }
        public SolarPanel()
        {

        }
        public SolarPanel(string name,double maxPower):base(name,maxPower)
        {
            this.curentPower = 0;
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
