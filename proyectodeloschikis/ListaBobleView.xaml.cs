using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace proyectodeloschikis
{
    public partial class ListaBobleView : UserControl
    {
        // lista doblemente enlazada
        LinkedList<int> lista = new LinkedList<int>();

        public ListaBobleView()
        {
            InitializeComponent();
        }

        // mostrar elementos en la lista visual
        void Mostrar()
        {
            listaDatos.ItemsSource = null;
            listaDatos.ItemsSource = lista;
        }

        // 🔹 agregar al inicio
        void AgregarInicio_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsEntero(txtValor.Text, out int m))
                {
                    txtValor.Clear();
                    return;
                }

                lista.AddFirst(m);
                txtValor.Clear();
                Mostrar();
            });
        }

        // 🔹 agregar al final
        void AgregarFinal_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsEntero(txtValor.Text, out int m))
                {
                    txtValor.Clear();
                    return;
                }

                lista.AddLast(m);
                txtValor.Clear();
                Mostrar();
            });
        }

        // 🔹 eliminar inicio
        void EliminarInicio_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count == 0)
            {
                MessageBox.Show("La lista está vacía", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            lista.RemoveFirst();
            Mostrar();
        }

        // 🔹 eliminar final
        void EliminarFinal_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count == 0)
            {
                MessageBox.Show("La lista está vacía", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            lista.RemoveLast();
            Mostrar();
        }

        // 🔹 buscar elemento
        void Buscar_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (lista.Count == 0)
                {
                    MessageBox.Show("La lista está vacía", "Advertencia",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!validacion.EsEntero(txtBuscar.Text, out int m))
                {
                    txtBuscar.Clear();
                    return;
                }

                if (lista.Contains(m))
                    MessageBox.Show("Elemento encontrado", "Resultado",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Elemento no encontrado", "Resultado",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                txtBuscar.Clear();
            });
        }

        // 🔹 verificar si la lista está vacía
        void EstaVacia_Click(object sender, RoutedEventArgs e)
        {
            txtEstaVacia.Text = lista.Count == 0 ? "Sí" : "No";
        }

        // 🔹 ordenar ascendente
        void Ordenar_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count == 0)
            {
                MessageBox.Show("La lista está vacía", "Advertencia",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<int> temp = lista.ToList();
            temp.Sort();

            lista.Clear();

            foreach (int m in temp)
                lista.AddLast(m);

            Mostrar();
        }

        // 🔹 limpiar lista
        void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            lista.Clear();
            Mostrar();
        }
    }
}