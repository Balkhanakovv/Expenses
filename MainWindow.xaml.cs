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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;

namespace database
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string db_path = "";
        public static dbNote selectedNote;

        public MainWindow()
        {
            InitializeComponent();
            resultLb.Content = "";
        }

        private void opendbBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".db";
                dlg.Filter = "sqlite database (.db)|*.db";

                dlg.ShowDialog();
                db_path = dlg.FileName;

                showDB();
            }
            catch { }
        }

        private void createdbBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".db";
                dlg.Filter = "sqlite database (.db)|*.db";

                dlg.ShowDialog();
                db_path = dlg.FileName;

                SQLiteConnection.CreateFile(db_path);

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + db_path + ";Version=3;");

                m_dbConnection.Open();
                string sqlResponse = "CREATE TABLE expenses (name TEXT NOT NULL UNIQUE, cost REAL NOT NULL)";
                SQLiteCommand command = new SQLiteCommand(sqlResponse, m_dbConnection);
                command.ExecuteNonQuery();
                m_dbConnection.Close();

                showDB();
            }
            catch { }
        }

        private void addNoteBt_Click(object sender, RoutedEventArgs e)
        {
            if (db_path == "")
            {
                MessageBox.Show("Создайте новую базу данных или\nоткройте уже существующую.");                
            }
            else
            {
                AddNoteWindow addWin = new AddNoteWindow();
                addWin.added += refresh_dbnote;

                if (addWin.ShowDialog() == true) { }

                addWin.added -= refresh_dbnote;
            }
        }

        private void editNoteBt_Click(object sender, RoutedEventArgs e)
        {
            selectedNote = (dbNote)tableDG.SelectedItem;


            if (db_path == "")
            {
                MessageBox.Show("Создайте новую базу данных или\nоткройте уже существующую.");
            }
            else
            {
                EditNoteWindow editWin = new EditNoteWindow();
                editWin.edited += refresh_dbnote;

                if (editWin.ShowDialog() == true) { }

                editWin.edited -= refresh_dbnote;
            }
        }

        private void deleteNoteBt_Click(object sender, RoutedEventArgs e)
        {
            if (db_path == "")
            {
                MessageBox.Show("Создайте новую базу данных или\nоткройте уже существующую.");
            }
            else
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = " + db_path + "; Version = 3; ");
                m_dbConnection.Open();

                var note = (dbNote)tableDG.SelectedItem;

                string sqlRequest = "DELETE FROM expenses WHERE name='" + note.name + "'";
                SQLiteCommand command = new SQLiteCommand(sqlRequest, m_dbConnection);
                command.ExecuteNonQuery();
                m_dbConnection.Close();

                showDB();
            }
        }

        private void refresh_dbnote(object sender, EventArgs e)
        {
            tableDG.Items.Clear();

            showDB();
        }

        private void showDB()
        {
            double result = 0; 

            tableDG.Items.Clear();

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + db_path + ";Version=3;");
            m_dbConnection.Open();
            string sqlRequest = "SELECT * FROM expenses";
            SQLiteCommand command = new SQLiteCommand(sqlRequest, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dbNote note = new dbNote { name = reader["name"].ToString(), cost = double.Parse(reader["cost"].ToString()) };
                tableDG.Items.Add(note);
                result += note.cost;
                resultLb.Content = result.ToString();
                if (tableDG.Items.Count == 0)
                    resultLb.Content = "";
            }

            m_dbConnection.Close();


        }

        private void AboutBt_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программа для подсчета расходов.\n\nhttps://github.com/Balkhanakovv/ExpensesApp\n\nВсе иконки взяты с сайта freepik.com");
        }
    }
}
