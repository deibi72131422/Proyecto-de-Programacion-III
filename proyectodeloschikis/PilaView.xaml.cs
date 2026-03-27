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
    /// Lógica de interacción para PilaView.xaml
    /// </summary>
    public partial class PilaView : UserControl
    {
        Stack<int> pila = new Stack<int>();
        int capacidad = 10;

        public PilaView()
        {
            InitializeComponent();

            btnPush.Click += Push_Click;
            btnPop.Click += Pop_Click;
            btnLimpiarPila.Click += Limpiar_Click;
            btnEstaVacia.Click += EstaVacia_Click;
            btnEstaLlena.Click += EstaLlena_Click;
            btnCapacidad.Click += Capacidad_Click;
            btnVerCima.Click += VerCima_Click;
        }

        void Mostrar()
        {
            listaPila.Items.Clear();

            foreach (int m in pila)
            {
                listaPila.Items.Add(m);
            }
        }

        // insertar (Push)
        void Push_Click(object sender, RoutedEventArgs e)
        {
                // Validar que no esté vacía la caja de texto
                if (string.IsNullOrWhiteSpace(txtPush.Text))
                {
                    MessageBox.Show("Ingrese un número para agregar a la pila", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validar que sea entero
                if (!validacion.EsEntero(txtPush.Text, out int m)) return;

                // Validar capacidad
                if (pila.Count >= capacidad)
                {
                    MessageBox.Show("La pila está llena. No se puede agregar más elementos.", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                pila.Push(m);
                txtPush.Clear();
                Mostrar();
        }

        // quitar (Pop)
        void Pop_Click(object sender, RoutedEventArgs e)
        {
            if (pila.Count == 0)
            {
                MessageBox.Show("La pila está vacía", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            pila.Pop();
            Mostrar();
        }

        // limpiar
        void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            pila.Clear();
            Mostrar();
        }

        // esta vacia
        void EstaVacia_Click(object sender, RoutedEventArgs e)
        {
            txtEstaVaciaPila.Text = pila.Count == 0 ? "Sí" : "No";
        }

        // esta llena
        void EstaLlena_Click(object sender, RoutedEventArgs e)
        {
            txtEstaLlenaPila.Text = pila.Count == capacidad ? "Sí" : "No";
        }

        // capacidad
        void Capacidad_Click(object sender, RoutedEventArgs e)
        {
            txtCapacidadPila.Text = capacidad.ToString();
        }

        // ver cima
        void VerCima_Click(object sender, RoutedEventArgs e)
        {
            if (pila.Count == 0)
            {
                MessageBox.Show("La pila está vacía", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            txtCimaPila.Text = pila.Peek().ToString();
        }
    }
}
