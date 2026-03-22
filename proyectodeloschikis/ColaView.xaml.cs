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
    /// Lógica de interacción para ColaView.xaml
    /// </summary>
    public partial class ColaView : UserControl
    {
        Queue<string> cola = new Queue<string>();
        int max = 10;

        public ColaView()
        {
            InitializeComponent();
        }

        // insertar
        private void btnInsertar_Click(object sender, RoutedEventArgs e)
        {
            if (cola.Count < max)
            {
                cola.Enqueue(txtInsertar.Text);
                listaDatos.Items.Add(txtInsertar.Text);
                txtInsertar.Clear();
            }
        }

        // quitar
        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            if (cola.Count > 0)
            {
                cola.Dequeue();
                listaDatos.Items.RemoveAt(0);
            }
        }

        // limpiar
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            cola.Clear();
            listaDatos.Items.Clear();
        }

        // esta vacia
        private void btnVacia_Click(object sender, RoutedEventArgs e)
        {
            if (cola.Count == 0)
                txtEstaVacia.Text = "Si";
            else
                txtEstaVacia.Text = "No";
        }

        // esta llena
        private void btnLlena_Click(object sender, RoutedEventArgs e)
        {
            if (cola.Count == max)
                txtEstaLlena.Text = "Si";
            else
                txtEstaLlena.Text = "No";
        }

        // tamaño maximo
        private void btnTamano_Click(object sender, RoutedEventArgs e)
        {
            txtTamanoMaximo.Text = max.ToString();
        }

        // cima
        private void btnCima_Click(object sender, RoutedEventArgs e)
        {
            if (cola.Count > 0)
                txtCima.Text = cola.Peek();
        }
    }
}
