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
using System.Windows.Shapes;

namespace WPFRichTextBox
{
    /// <summary>
    /// Interaction logic for RtfView.xaml
    /// </summary>
    public partial class RtfView : Window
    {
        public RtfView()
        {
            InitializeComponent();
        }

        private void SaveRtfOnClick(object sender, RoutedEventArgs e)
        {
            string rtfFromRtb;
            var range = new TextRange(rtfEditor.Document.ContentStart, rtfEditor.Document.ContentEnd);

            using (var ms = new MemoryStream())
            {
                range.Save(ms, DataFormats.Rtf);
                ms.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(ms))
                {
                    rtfFromRtb = sr.ReadToEnd();
                }
            }

            Text = rtfFromRtb;
            IsOk = true;
            Close();
        }

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            var byteArray = Encoding.UTF8.GetBytes(text);
            using (var stream = new MemoryStream(byteArray))
            {
                var range = new TextRange(rtfEditor.Document.ContentStart, rtfEditor.Document.ContentEnd);
                range.Load(stream, System.Windows.DataFormats.Rtf);
            }

            rtfEditor.CaretPosition = rtfEditor.Document.ContentEnd;
            rtfEditor.ScrollToEnd();
        }

        public string Text { get; set; }

        public bool IsOk { get; set; }
    }
}
