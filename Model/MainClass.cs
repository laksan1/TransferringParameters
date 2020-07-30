using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using TransferringParameters.ViewModel;
using TransferringParameters.View;

namespace TransferringParameters.Model
{
    [Transaction(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
   public class TransferringParameters :  IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var vm = new MainWindowViewModel(new RevitModelClass(commandData.Application));
            var MainWindow = new MainWindow { DataContext = vm };
            try
            {
                if (MainWindow != null)
                {
                    MainWindow.ShowDialog();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exeption", ex.Message);
                return Result.Failed;
            }
        }
   }
}

