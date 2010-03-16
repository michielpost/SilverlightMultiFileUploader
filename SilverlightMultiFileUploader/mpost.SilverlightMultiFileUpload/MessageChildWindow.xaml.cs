using System.Windows;
using System.Windows.Controls;

namespace mpost.SilverlightMultiFileUpload
{
    public partial class MessageChildWindow : ChildWindow
    {
        public string Message { get; set; }

        public MessageChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }      
    }
}

