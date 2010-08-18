using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using mpost.SilverlightMultiFileUpload.Contracts;
using mpost.SilverlightMultiFileUpload.Utils.Constants;

namespace mpost.SLMFU.MEF.Plugins.Thumbnails
{   
    [Export(typeof(IVisualizeFileRow))]
    public partial class ThumbnailFileRowControl : UserControl, IVisualizeFileRow
    {
        public IUserFile UserFile
        {
            get
            {
                if (this.DataContext != null)
                    return ((IUserFile)this.DataContext);
                else
                    return null;

            }
            set
            {
                this.DataContext = value;
            }
        }

        public ThumbnailFileRowControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(FileRowControl_Loaded);

        }


        private void FileRowControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Subscribe to PropertyChanged of the UserFile
            UserFile.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(FileRowControl_PropertyChanged);

            VisualStateManager.GoToState(this, UserFile.State.ToString(), true);
        }

        private void FileRowControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                VisualStateManager.GoToState(this, UserFile.State.ToString(), true);

                //Show grey text when the upload is finished
                if (this.UserFile.State == Enums.FileStates.Finished)
                {
                    GreyOutText();
                    ShowValidIcon();
                }

                //Show error message when the upload failed:
                if (this.UserFile.State == Enums.FileStates.Error)
                {
                    ErrorMsgTextBlock.Visibility = Visibility.Visible;

                    if (!string.IsNullOrEmpty(this.UserFile.ErrorMessage))
                        ErrorMsgTextBlock.Text = this.UserFile.ErrorMessage;
                }
            }
            else if (e.PropertyName == "Percentage")
            {
                // if the percentage is decreasing, don't use an animation
                if (UserFile.Percentage < PercentageProgress.Value)
                    PercentageProgress.Value = UserFile.Percentage;
                else
                {
                    sbProgressFrame.Value = UserFile.Percentage;
                    sbProgress.Begin();
                }
            }

        }

        private void ShowValidIcon()
        {
            PercentageProgress.Visibility = Visibility.Collapsed;
            ValidUploadIcon.Visibility = Visibility.Visible;
        }

        private void GreyOutText()
        {
            SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);

            FileNameTextBlock.Foreground = grayBrush;
            StateTextBlock.Foreground = grayBrush;
            FileSizeTextBlock.Foreground = grayBrush;

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //this.UserFile.CancelUpload();

            this.UserFile.State = Enums.FileStates.Deleted;

            this.Visibility = Visibility.Collapsed;

        }


    }
}
