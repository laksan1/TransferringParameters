using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TransferringParameters.View;

namespace TransferringParameters.Model
{
   public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Если выбран шаблон сменить картинку
            if ((bool)value)
            {

#if R2017

                string packUri = "pack://application:,,,/TransferringParameters_2017;component/Resources/Green.png";
#elif R2018

                string packUri = "pack://application:,,,/TransferringParameters_2018;component/Resources/Green.png";
#elif R2019

                string packUri = "pack://application:,,,/TransferringParameters_2019;component/Resources/Green.png";//2019

#elif R2020

                string packUri = "pack://application:,,,/TransferringParameters_2020;component/Resources/Green.png";
#endif

                BitmapImage Image1 = new BitmapImage(new Uri(packUri, UriKind.Absolute));

                return Image1.UriSource;


            }
            //Если не выбран шаблон
            else
            {


#if R2017

                     string packUri = "pack://application:,,,/TransferringParameters_2017;component/Resources/Red.png";
#elif R2018

              
                     string packUri = "pack://application:,,,/TransferringParameters_2018;component/Resources/Red.png";
#elif R2019


                string packUri = "pack://application:,,,/TransferringParameters_2019;component/Resources/Red.png";//2019

#elif R2020

              
                     string packUri = "pack://application:,,,/TransferringParameters_2020;component/Resources/Red.png";
#endif

                BitmapImage Image1 = new BitmapImage(new Uri(packUri, UriKind.Absolute));
                return Image1.UriSource;

            }

        }

        //Ничего не делает
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : Binding.DoNothing;
        }


    }
}
