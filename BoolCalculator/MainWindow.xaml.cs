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

namespace BoolCalculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Insert(object sender, RoutedEventArgs e)
        {
            int selection = expression.SelectionStart;
            expression.Text = expression.Text.Insert(selection, (string)(sender as Button).Content);
            expression.SelectionStart = selection + 1;
        }
        private void Calculate(object sender, RoutedEventArgs e)
        {
            Calculation calc = new Calculation(expression.Text, truthTable);
            calc.Calc(1);
        }
        private void Keyboard(object sender, RoutedEventArgs e)
        {
            if (panel.Visibility == Visibility.Hidden)
            {
                panel.Visibility = Visibility.Visible;
                return;
            }
            if (panel.Visibility == Visibility.Visible)
            {
                panel.Visibility = Visibility.Hidden;
                return;
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            expression.Text = string.Empty;
            truthTable.Columns.Clear();
            truthTable.Items.Clear();
        }
    }
}
