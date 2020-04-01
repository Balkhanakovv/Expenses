using System;
using System.Collections.Generic;
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
using System.Data.SQLite;
using System.Globalization;

namespace database
{
    /// <summary>
    /// Логика взаимодействия для EditNoteWindow.xaml
    /// </summary>
    public partial class EditNoteWindow : Window
    {
        public event EventHandler edited;
        string path = MainWindow.db_path;

        public EditNoteWindow()
        {
            InitializeComponent();
            try
            {
                nameTb.Text = MainWindow.selectedNote.name;
            }
            catch { }
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double cost = double.Parse(costTb.Text, CultureInfo.InvariantCulture);

                if (nameTb.Text != "")
                {
                    SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                    m_dbConnection.Open();

                    string sqlResponse = "UPDATE expenses SET cost="+ costTb.Text+" WHERE name='"+MainWindow.selectedNote.name+"'";
                    SQLiteCommand command = new SQLiteCommand(sqlResponse, m_dbConnection);
                    command.ExecuteNonQuery();
                    m_dbConnection.Close();

                    edited?.Invoke(this, EventArgs.Empty);
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Введите число!\n(точка, а не запятая)");
            }
        }
    }
}
