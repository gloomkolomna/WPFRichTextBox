using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFRichTextBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RtfEditOnClick(object sender, RoutedEventArgs e)
        {
            var rtfDialog = new RtfView();
            rtfDialog.SetText(Text);
            rtfDialog.ShowDialog();
            if (rtfDialog.IsOk)
            {
                LoadRtfFromString(rtfDialog.Text);
                Text = rtfDialog.Text;
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string),
            typeof(MainWindow),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                TextPropertyChangedCallback));

        private static void TextPropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var newValue = (string)dependencyPropertyChangedEventArgs.NewValue;
            var rtfView = (MainWindow)dependencyObject;
            rtfView.LoadRtfFromString(newValue);
        }

        private void LoadRtfFromString(string rtfString)
        {
            var range = new TextRange(RtbEditor.Document.ContentStart, RtbEditor.Document.ContentEnd);
            try
            {
                using (var stream = new MemoryStream())
                {
                    using (var rtfStreamWriter = new StreamWriter(stream))
                    {
                        rtfStreamWriter.Write(rtfString);
                        rtfStreamWriter.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        range.Load(stream, DataFormats.Rtf);
                    }
                }
            }
            catch (Exception e)
            {
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(e.Message)));
            }
        }
    }
}
