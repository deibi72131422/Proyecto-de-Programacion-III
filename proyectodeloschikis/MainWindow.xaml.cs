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

namespace proyectodeloschikis
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Ordenamiento_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new OrdenamientoView();
        }

        private void Listas_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ListaEnlazadaView();
        }
        private void LidstasDoblementeEnlazadas_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ListaBobleView();
        }
        private void Colas_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ColaView();
        }
        private void Pilas_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PilaView();
        }

        private void Calculadora_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CalculadoraView();
        }
        private void BtnLineales_Checked(object sender, RoutedEventArgs e)
        {
            panelLineales.Visibility = Visibility.Visible;
        }

        private void BtnLineales_Unchecked(object sender, RoutedEventArgs e)
        {
            panelLineales.Visibility = Visibility.Collapsed;
        }

        private void BtnNoLineales_Checked(object sender, RoutedEventArgs e)
        {
            panelNoLineales.Visibility = Visibility.Visible;
        }

        private void BtnNoLineales_Unchecked(object sender, RoutedEventArgs e)
        {
            panelNoLineales.Visibility = Visibility.Collapsed;
        }
    }


}
