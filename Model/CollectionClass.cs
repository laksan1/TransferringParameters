using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace TransferringParameters.Model
{
   public  class CollectionClass: INotifyPropertyChanged
    {
        public string NameInitialParameter { get; set; } //Имя параметра проекта

        public string NameDestinationParameter { get; set; } //Имя параметра проекта

        public ElementBinding Binding { get; set; } //Связь параметра проекта

        public bool IsShared { get; set; } //Является общим

        public Definition Definition { get; set; } //Определение

        

        public BuiltInCategory CategoryRevit { get; set; } //Определение


        public string NameCategory { get; set; } //Имя вида

        public bool isCheckedCategory;

        public bool IsCheckedCategory
        {
            get { return isCheckedCategory; }
            set { isCheckedCategory = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      


    }
}
