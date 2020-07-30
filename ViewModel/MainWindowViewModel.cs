using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System.Windows.Input;
using Autodesk.Revit.UI.Selection;
using TransferringParameters.MVVM;
using TransferringParameters.ViewModel;
using TransferringParameters.Model;
using TransferringParameters.View;

namespace TransferringParameters.ViewModel
{
    public class MainWindowViewModel :ModelBase
    {


        public string nameCat;
        public string NameCat
        {
            get { return nameCat; }
            set { nameCat = value;
                GenerateCategories(value);
                OnPropertyChanged(); }
        }

        private ObservableCollection<CollectionClass> _listCategoriesCollection;
        private ObservableCollection<CollectionClass> _listCategoriesCollection2;

        public ObservableCollection<CollectionClass> ListCategoriesCollection2
        {
            get => _listCategoriesCollection2;
            set { _listCategoriesCollection2 = value; OnPropertyChanged(); }

        }


        private ObservableCollection<CollectionClass> _ListBindingParameters;
        private ICommand _command;

        public CollectionClass selectedInitialParameter;

        public CollectionClass RemName;


        public CollectionClass SelectedInitialParameter
        {
            get { return selectedInitialParameter; }
            set { selectedInitialParameter = value; OnPropertyChanged(); }
        }

        public CollectionClass selectedDestinationParameter;
        public CollectionClass SelectedDestinationParameter
        {
            get { return selectedDestinationParameter; }
            set { selectedDestinationParameter = value; OnPropertyChanged(); }
        }
        
      public bool isCheckedinsulation;
        public bool IsCheckedinsulation
        {
            get { return isCheckedinsulation; }
            set { isCheckedinsulation = value; OnPropertyChanged(); }
        }

        //Картинка параметра экземпляра
        public string imageSourceGreen;
        public string ImageSourceGreen
        {
            get
            {


#if R2017

             return "pack://application:,,,/TransferringParameters_2017;component/Resources/Green.png";
#elif R2018

               return = "pack://application:,,,/TransferringParameters_2018;component/Resources/Green.png";
#elif R2019

                return "pack://application:,,,/TransferringParameters_2019;component/Resources/Green.png";//2019

#elif R2020

              return "pack://application:,,,/TransferringParameters_2020;component/Resources/Green.png";
#endif

            }
        }

        //Картинка параметра типа
        public string imageSourceRed;
        public string ImageSourceRed
        {
            get
            {


#if R2017

             return "pack://application:,,,/TransferringParameters_2017;component/Resources/Red.png";
#elif R2018

               return = "pack://application:,,,/TransferringParameters_2018;component/Resources/Red.png";
#elif R2019

                return "pack://application:,,,/TransferringParameters_2019;component/Resources/Red.png";//2019

#elif R2020

              return "pack://application:,,,/TransferringParameters_2020;component/Resources/Red.png";
#endif

            }
        }


        public RelayCommand CheckObjCommand { get; private set; }
        public RelayCommand CheckAllCommand { get; private set; }
        private Action _closeAction;
        public RevitModelClass RevitModel { get; set; }



        public MainWindowViewModel(RevitModelClass _RM)
        {
            RevitModel = _RM;

            ListBindingParameters = RevitModel.GenerateInitialParametersList();
            if(ListBindingParameters==null)
            {
                RevitModel.ShowTaskDialog("Error", "Missing of project parameters");


            }
            ListCategoriesCollection = RevitModel.GenerateCategoriesList();

            ListCategoriesCollection2 = new ObservableCollection<CollectionClass>(ListCategoriesCollection);

            SelectedInitialParameter = ListBindingParameters[0]; 
            SelectedDestinationParameter = ListBindingParameters[0];


        }
        public ObservableCollection<CollectionClass> ListCategoriesCollection
        {
            get => _listCategoriesCollection;
            set { _listCategoriesCollection = value; OnPropertyChanged(); }

        }
        public ObservableCollection<CollectionClass> ListBindingParameters
        {
            get => _ListBindingParameters;
            set { _ListBindingParameters = value; OnPropertyChanged(); }

        }

        //Запуск плагина
        public ICommand CommandButton
        {
            get
            {
                if (_command == null)
                    _command = new RelayCommand(o =>
                    {
                        var CheckViews = ListCategoriesCollection2.FirstOrDefault(i => i.IsCheckedCategory);
                        if (CheckViews != null)
                        {
                            RevitModel.ReplaceValueOfParameter(
                                                               selectedInitialParameter.NameInitialParameter,
                                                               selectedDestinationParameter.NameDestinationParameter,
                                                               IsCheckedinsulation,
                                                               CheckViews.CategoryRevit);
                        }

                       
                    });
                return _command;
            }
        }
        public Action CloseAction
        {
            get => _closeAction;
            set
            {
                _closeAction = value;
                OnPropertyChanged();
            }
        }

        //Автопоиск категории
        public void GenerateCategories(string _name)
        {
            try
            {
              
                string texboxText = _name;

                CollectionClass namwB = ListCategoriesCollection2.FirstOrDefault(i => i.IsCheckedCategory);

                if (namwB != null)
                {
                    RemName = namwB;
                }

                ListCategoriesCollection2.Clear();

                if (!String.IsNullOrEmpty(texboxText))
                {
                   
                 
                    foreach (var a in ListCategoriesCollection.Where(i => i.NameCategory.StartsWith(texboxText)))
                    {
                        ListCategoriesCollection2.Add(a);

                    }
                }
                else
                {
                
                    foreach (var a in ListCategoriesCollection)
                    {
                        ListCategoriesCollection2.Add(a);

                    }

                }

                 namwB = ListCategoriesCollection2.FirstOrDefault(i => i.NameCategory == RemName.NameCategory);

                if(namwB!=null)
                {
                    namwB.IsCheckedCategory = true;

                }
            }
            catch
            {


            }
        }
    }

}
