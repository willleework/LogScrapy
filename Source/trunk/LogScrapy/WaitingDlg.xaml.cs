using System;
using System.Windows;

namespace LogScrapy
{
    public partial class WaitingDlg
    {

        private readonly string m_strThePromptText;
        
        public WaitingDlg(string strThePromptText=null)
        {
            m_strThePromptText = strThePromptText;
            InitializeComponent();

        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(m_strThePromptText))
            {
                tbPrompt.Text = m_strThePromptText;
            }
        }
    }
}
