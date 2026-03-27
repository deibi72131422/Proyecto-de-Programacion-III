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
            validacion.EjecutarSeguro(() =>
            {
                // Validar que no esté vacío y recuperar valor limpio
                if (!validacion.EsTextoValido(txtInsertar.Text, out string valor)) return;

                // Validar alfanumérico y longitud máxima para evitar entradas problemáticas
                if (!validacion.EsAlfaNumerico(valor, out string alfaValor, 50)) return;

                // Validar que no esté llena la cola
                if (cola.Count >= max)
                {
                    MessageBox.Show("La cola está llena. No se puede agregar más elementos.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                cola.Enqueue(alfaValor);
                listaDatos.Items.Add(alfaValor);        // Mantenemos el comportamiento original
                txtInsertar.Clear();
            });
        }

        // quitar
        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            if (cola.Count == 0)
            {
                MessageBox.Show("La cola está vacía", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            cola.Dequeue();
            listaDatos.Items.RemoveAt(0);   // Mantenemos el comportamiento original
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
            txtEstaVacia.Text = cola.Count == 0 ? "Sí" : "No";
        }

        // esta llena
        private void btnLlena_Click(object sender, RoutedEventArgs e)
        {
            txtEstaLlena.Text = cola.Count == max ? "Sí" : "No";
        }

        // tamaño maximo
        private void btnTamano_Click(object sender, RoutedEventArgs e)
        {
            txtTamanoMaximo.Text = max.ToString();
        }

        // cima (frente de la cola)
        private void btnCima_Click(object sender, RoutedEventArgs e)
        {
            if (cola.Count == 0)
            {
                MessageBox.Show("La cola está vacía", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            txtCima.Text = cola.Peek();
        }
    }
}
