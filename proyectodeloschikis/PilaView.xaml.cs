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

        // insertar
        void Push_Click(object sender, RoutedEventArgs e)
        {
            if (pila.Count < capacidad)
            {
                int m = int.Parse(txtPush.Text);
                pila.Push(m);
                txtPush.Clear();
                Mostrar();
            }
        }

        // quitar
        void Pop_Click(object sender, RoutedEventArgs e)
        {
            if (pila.Count > 0)
            {
                pila.Pop();
                Mostrar();
            }
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
            txtEstaVaciaPila.Text = pila.Count == 0 ? "Si" : "No";
        }

        // esta llena
        void EstaLlena_Click(object sender, RoutedEventArgs e)
        {
            txtEstaLlenaPila.Text = pila.Count == capacidad ? "Si" : "No";
        }

        // capacidad
        void Capacidad_Click(object sender, RoutedEventArgs e)
        {
            txtCapacidadPila.Text = capacidad.ToString();
        }

        // ver cima
        void VerCima_Click(object sender, RoutedEventArgs e)
        {
            if (pila.Count > 0)
                txtCimaPila.Text = pila.Peek().ToString();
        }
    }
}
