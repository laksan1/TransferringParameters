using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using TransferringParameters.ViewModel;
using TransferringParameters.Model;
using System.Linq;
using TransferringParameters.MVVM;
using TransferringParameters.View;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Mechanical;

namespace TransferringParameters.Model
{
    public class RevitModelClass
    {

        private UIApplication _uiApplication;
        private Document _document;
        private readonly UIDocument _uiDocument;
        public int i = 0;


        public void ReplaceValueOfParameter(
                                            // List<CollectionClass> allviews, 
                                            string _initialParameter,
                                            string _destinationParameter,
                                            bool _IsCheckedInsulation,
                                            BuiltInCategory _selectedCategory)//добавить имя листа
        {
          

            var collector = Get_collector_by_category(_document, _selectedCategory); //Взять из аргумента

            if (collector == null || collector.Count() == 0)
            {
                TaskDialog.Show("Error", "You do not have ducts and pipes");
            }

            using (var tr = new Transaction(_document, "Set parameter"))
            {
               
                tr.Start();
              
                    foreach (Element el in collector)
                    {

                        if (_IsCheckedInsulation)
                        {

                            if (el is Duct duct)

                            {

                                IsElemenDuct(duct, _initialParameter, _destinationParameter);

                            }

                            if (el is Pipe pipe)

                            {

                                IsElemenPipe(pipe, _initialParameter, _destinationParameter);

                            }

                        }

                        else
                        {


                            Parameter parameterInitial = el.LookupParameter(_initialParameter);
                            Parameter parameterDestination = el.LookupParameter(_destinationParameter);
                        /// По типу
                       
                            var typeId = el.GetTypeId();
                            Element elementByType = _document.GetElement(typeId);
                            Parameter parameterDestinationType = elementByType.LookupParameter(_destinationParameter);

                        if(parameterDestination==null)
                        {

                            parameterDestination = parameterDestinationType;
                        }


                            if (parameterInitial != null && parameterDestination != null 
                                                         && !parameterDestination.IsReadOnly)
                            {
                                    string valueParametr = GetParameterValue(parameterInitial);
                                            parameterDestination.Set(valueParametr);
                            i++;

                            }
                        }

                    }

                    tr.Commit();
                }

            ShowTaskDialog("Information", $"Parameter value changed for {i} elements", i);

        }


        public string GetParameterValue(Parameter parameter)
        {
            string s;
            switch (parameter.StorageType)
            {
                case StorageType.Double:

                    s = parameter.AsDouble().ToString();
                    break;

                case StorageType.Integer:
                    s = parameter.AsInteger().ToString();
                    break;

                case StorageType.String:
                    s = parameter.AsString();
                    break;

                case StorageType.ElementId:
                    s = parameter.AsElementId().IntegerValue.ToString();
                    break;

                case StorageType.None:
                    s = "";
                    break;

                default:
                    s = "0.0";
                    break;
            }
            return s;
        }

        public void ShowTaskDialog(string _title, string _containt, int _i)
        {
            TaskDialog mainDialog = new TaskDialog(_title);
            mainDialog.MainInstruction = _containt;
            //mainDialog.MainContent = _containt;
            mainDialog.Show();

        }

        public void IsElemenDuct(Duct _duct, string _initialParameter, string _destinationParameter)
        {

                Parameter parameterDuct = _duct.LookupParameter(_initialParameter);

                Element elemntInsulationDuct = GetInsulationDuct(_duct, _document);
                if (elemntInsulationDuct != null)
                {
                    Parameter parameterInsulation = elemntInsulationDuct.LookupParameter(_destinationParameter);

                var typeId = elemntInsulationDuct.GetTypeId();
                Element elementByType = _document.GetElement(typeId);
                Parameter parameterDestinationType = elementByType.LookupParameter(_destinationParameter);

                parameterInsulation = (parameterInsulation == null) ? parameterDestinationType : null;
              
                if (parameterDuct != null && parameterInsulation != null && !parameterInsulation.IsReadOnly)
                    {
                        string valueparameterDuct = GetParameterValue(parameterDuct);
                        parameterInsulation.Set(valueparameterDuct);
                        i++;

                    }
                }
        }

        public void IsElemenPipe(Pipe _pipe, string _initialParameter, string _destinationParameter)
        {
                Parameter parameterPipe = _pipe.LookupParameter(_initialParameter);

                Element elemntInsulationPipe = GetInsulationPipe(_pipe, _document);
                if (elemntInsulationPipe != null)
                {
                    Parameter parameterInsulation = elemntInsulationPipe.LookupParameter(_destinationParameter);

                    var typeId = elemntInsulationPipe.GetTypeId();
                    Element elementByType = _document.GetElement(typeId);
                    Parameter parameterDestinationType = elementByType.LookupParameter(_destinationParameter);

                parameterInsulation = (parameterInsulation == null) ? parameterDestinationType : parameterInsulation;

                if (parameterPipe != null && parameterInsulation != null && !parameterInsulation.IsReadOnly)
                    {
                        string valueparameterPipe = GetParameterValue(parameterPipe);
                        parameterInsulation.Set(valueparameterPipe);
                        i++;

                    }
                }

        }


        public Element GetInsulationDuct(Duct _duct, Document _doc)
        {

            var MepSystem = _duct.MEPSystem;

            MechanicalSystem mechanicalSystem = MepSystem as MechanicalSystem;

            if (mechanicalSystem != null)
            {
                var iterator = mechanicalSystem.DuctNetwork.ForwardIterator();
                while (iterator.MoveNext())
                {
                    var insulationid = iterator.Current;
                    if (insulationid is DuctInsulation ductInsulation)
                    {
                        if (ductInsulation.HostElementId == _duct.Id)
                        {
                            Element elDuctInsulation = _doc.GetElement(ductInsulation.Id);

                            return elDuctInsulation;

                        }
                    }
                }

            }

            return null;

        }

        public Element GetInsulationPipe(Pipe _pipe, Document _doc)
        {


            var MepSystem = _pipe.MEPSystem;

            PipingSystem mechanicalSystem = MepSystem as PipingSystem;

            if (mechanicalSystem != null)
            {
                var iterator = mechanicalSystem.PipingNetwork.ForwardIterator();
                while (iterator.MoveNext())
                {
                    var insulationid = iterator.Current;
                    if (insulationid is PipeInsulation pipeInsulation)
                    {
                        if (pipeInsulation.HostElementId == _pipe.Id)
                        {
                            Element elDuctInsulation = _doc.GetElement(pipeInsulation.Id);

                            return elDuctInsulation;

                        }
                    }
                }

            }

            return null;

        }
        //Коллектор pipe and duct
        public IList<Element> Get_collector_by_category(Document _doc, BuiltInCategory _builtInCategorySelected)
        {
            List<Element> elemntsList = new List<Element>();

            BuiltInCategory[] bics = new BuiltInCategory[] {
                _builtInCategorySelected

            };

            IList<ElementFilter> catfs = new List<ElementFilter>(bics.Count());
            foreach (BuiltInCategory bic in bics)
            {

                catfs.Add(new ElementCategoryFilter(bic, false));
            }

            ElementFilter filter = new LogicalOrFilter(catfs);

            elemntsList = new FilteredElementCollector(_doc).WhereElementIsNotElementType().WhereElementIsViewIndependent().WherePasses(filter).ToList();
            if(elemntsList.Count==0)
            {

            
                    TaskDialog mainDialog = new TaskDialog("Error, " + _doc.Application.Username);
                    mainDialog.MainInstruction = "0 elements";
                    mainDialog.MainContent =
                    "Elements of the selected category are left in the project.";
                


            }


            return elemntsList;



        }

        public RevitModelClass(UIApplication uiapp)
        {
            _uiApplication = uiapp;
            _uiDocument = _uiApplication.ActiveUIDocument;
            _document = _uiDocument.Document;

        }
        
        public  ObservableCollection<CollectionClass> GenerateCategoriesList()
        {
            ObservableCollection<CollectionClass> CategoriesNames = new ObservableCollection<CollectionClass>();
       

            Categories categories = _document.Settings.Categories;

            if (categories != null)
            {
           
                 foreach (Category s in categories)
                 {

                    CategoriesNames.Add(new CollectionClass()
                    {
                        NameCategory = s.Name,
                        CategoryRevit = GetBuiltInCategory(s)
                    });
                 }
            }

           
            
            return CategoriesNames;

        }

    
            public static BuiltInCategory GetBuiltInCategory(Category category)
            {
                if (System.Enum.IsDefined(typeof(BuiltInCategory),
                                              category.Id.IntegerValue))
                {
                    var builtInCategory = (BuiltInCategory)category.Id.IntegerValue;
                    return builtInCategory;
                }

                return BuiltInCategory.INVALID;
            }
        


        public ObservableCollection<CollectionClass> GenerateInitialParametersList()
        {

            Element projectInfoElement = new FilteredElementCollector(_document).
                OfCategory(BuiltInCategory.OST_ProjectInformation).FirstElement();

            ObservableCollection<CollectionClass> InitialParameters = new ObservableCollection<CollectionClass>();

            BindingMap map = _document.ParameterBindings;

            bool checkShared;

            DefinitionBindingMapIterator it
              = map.ForwardIterator();
            it.Reset();


            while (it.MoveNext())
            {

                var definition = (InternalDefinition)it.Key;
                var sharedParameterElement = _document.GetElement(
                  definition.Id) as SharedParameterElement;

                checkShared = (sharedParameterElement != null) ? true : false;

                InitialParameters.Add(new CollectionClass()
                {
                    NameInitialParameter = it.Key.Name,
                    Binding = it.Current as ElementBinding,
                    Definition = it.Key,
                    IsShared = checkShared, // Исправить, некоторые неправильные
                    NameDestinationParameter = it.Key.Name

                });

            }

            return InitialParameters;

        }
    } 
}

