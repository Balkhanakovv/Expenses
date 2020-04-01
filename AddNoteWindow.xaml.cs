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
    /// Логика взаимодействия для AddNoteWindow.xaml
    /// </summary>
    public partial class AddNoteWindow : Window
    {
        public event EventHandler added;
        string path = MainWindow.db_path;

        public AddNoteWindow()
        {
            InitializeComponent();
        }

        private void AddBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double cost = double.Parse(costTb.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Введите число!\n(точка, а не запятая)");
            }

            try
            {
                if (nameTb.Text != "")
                {
                    SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                    m_dbConnection.Open();

                    string sqlResponse = "INSERT INTO expenses VALUES('" + nameTb.Text + "', " + costTb.Text + ")";
                    SQLiteCommand command = new SQLiteCommand(sqlResponse, m_dbConnection);
                    command.ExecuteNonQuery();
                    m_dbConnection.Close();

                    added?.Invoke(this, EventArgs.Empty);
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Введите уникальное название.");
            }
        }
    }
}
