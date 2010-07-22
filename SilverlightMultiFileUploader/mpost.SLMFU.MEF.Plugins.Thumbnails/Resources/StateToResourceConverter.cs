using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Resources;
using System.Windows.Data;

namespace mpost.SLMFU.MEF.Plugins.Thumbnails.Resources
{
    public class StateToResourceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string output = string.Empty;

            if (value != null)
            {
                //Get the value from the resource file
                ResourceManager rm = new ResourceManager(typeof(mpost.SLMFU.MEF.Plugins.Thumbnails.UserMessages));

                output = rm.GetString(value.GetType().Name.ToString() + value.ToString());
            }

            return output;
        }

        //We only use one-way binding, so we don't implement this.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
