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
    /// Lógica de interacción para ListaEnlazadaView.xaml
    /// </summary>
    public partial class ListaEnlazadaView : UserControl
    {
        // lista enlazada
        LinkedList<int> lista = new LinkedList<int>();

        public ListaEnlazadaView()
        {
            // iniciar interfaz
            InitializeComponent();
        }

        // mostrar elementos en pantalla
        void Mostrar()
        {
            listaDatos.Items.Clear();

            foreach (int m in lista)
            {
                listaDatos.Items.Add(m);
            }
        }

        // agregar al inicio
        void AgregarInicio_Click(object sender, RoutedEventArgs e)
        {
            int m = int.Parse(txtValor.Text);
            lista.AddFirst(m);
            Mostrar();

            txtValor.Clear(); 
        }
        

        // agregar al final
        void AgregarFinal_Click(object sender, RoutedEventArgs e)
        {
            int m = int.Parse(txtValor.Text);
            lista.AddLast(m);
            Mostrar();
            txtValor.Clear();
        }

        // eliminar primer elemento
        void EliminarInicio_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count > 0)
                lista.RemoveFirst();

            Mostrar();
        }

        // eliminar último elemento
        void EliminarFinal_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count > 0)
                lista.RemoveLast();

            Mostrar();
        }

        // buscar elemento
        void Buscar_Click(object sender, RoutedEventArgs e)
        {
            int m = int.Parse(txtbucarelemtolista.Text);

            if (lista.Contains(m))
                MessageBox.Show("Elemento encontrado");
            else
                MessageBox.Show("Elemento no encontrado");
            txtbucarelemtolista.Clear();
        }

        // verificar si está vacía
        void EstaVacia_Click(object sender, RoutedEventArgs e)
        {
            txtVacia.Text = lista.Count == 0 ? "Si" : "No";
        }

        // sumar elementos
        void Suma_Click(object sender, RoutedEventArgs e)
        {
            int suma = lista.Sum();
            txtSuma.Text = suma.ToString();
        }

        // calcular media
        void Media_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count > 0)
            {
                double media = lista.Average();
                txtMedia.Text = media.ToString();
            }
        }

        // encontrar mayor
        void Mayor_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count > 0)
            {
                int mayor = lista.Max();
                txtMayor.Text = mayor.ToString();
            }
        }

        // ordenar descendente
        void Ordenar_Click(object sender, RoutedEventArgs e)
        {
            List<int> m = lista.ToList();
            m.Sort();
            m.Reverse();

            lista.Clear();

            foreach (int a in m)
                lista.AddLast(a);

            Mostrar();
        }

        // limpiar lista
        void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            lista.Clear();
            Mostrar();
        }
    }
}
