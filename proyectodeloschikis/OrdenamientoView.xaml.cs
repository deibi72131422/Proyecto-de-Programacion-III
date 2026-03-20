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
    /// Lógica de interacción para OrdenamientoView.xaml
    /// </summary>
    public partial class OrdenamientoView : UserControl
    {
        List<int> datos = new List<int>();

        public OrdenamientoView()
        {
            InitializeComponent();
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(txtNumero.Text, out num))
            {
                datos.Add(num);
                listaDatos.ItemsSource = null;
                listaDatos.ItemsSource = datos;
                txtNumero.Clear();
            }
        }

        private void Ordenar_Click(object sender, RoutedEventArgs e)
        {
            datos = datos.OrderBy(x => x).ToList();

            listaDatos.ItemsSource = null;
            listaDatos.ItemsSource = datos;
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            int num;
            if (int.TryParse(txtNumero.Text, out num))
            {
                int index = datos.IndexOf(num);

                if (index >= 0)
                    txtResultado.Text = "Encontrado en posición: " + index;
                else
                    txtResultado.Text = "No encontrado";
            }
        }
    }
}
