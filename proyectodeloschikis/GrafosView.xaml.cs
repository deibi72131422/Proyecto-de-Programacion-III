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
    /// Lógica de interacción para GrafosView.xaml
    /// </summary>
    public partial class GrafosView : UserControl
    {
        public GrafosView()
        {
            InitializeComponent();
        }
        private void ListaAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            ListaAdyacenciaWindow ventana = new ListaAdyacenciaWindow();
            ventana.Show();
        }
        private void MatrizAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            MatrizAdyacenciaWindow ventana = new MatrizAdyacenciaWindow();
            ventana.Show();
        }
    }
}
