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
            listaDatos.Items.Clear();

            foreach (int m in lista)
                listaDatos.Items.Add(m);
        }

        // agregar al inicio
        void AgregarInicio_Click(object sender, RoutedEventArgs e)
        {
            int m = int.Parse(txtValor.Text);
            lista.AddFirst(m);
            txtValor.Clear();
            Mostrar();
        }

        // agregar al final
        void AgregarFinal_Click(object sender, RoutedEventArgs e)
        {
            int m = int.Parse(txtValor.Text);
            lista.AddLast(m);
            txtValor.Clear();
            Mostrar();
        }

        // eliminar inicio
        void EliminarInicio_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count > 0)
                lista.RemoveFirst();

            Mostrar();
        }

        // eliminar final
        void EliminarFinal_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count > 0)
                lista.RemoveLast();

            Mostrar();
        }

        // buscar elemento
        void Buscar_Click(object sender, RoutedEventArgs e)
        {
            int m = int.Parse(txtBuscar.Text);

            if (lista.Contains(m))
                MessageBox.Show("Elemento encontrado");
            else
                MessageBox.Show("Elemento no encontrado");
            txtBuscar.Clear();
        }

        // verificar si la lista está vacía
        void EstaVacia_Click(object sender, RoutedEventArgs e)
        {
            txtEstaVacia.Text = lista.Count == 0 ? "Si" : "No";
        }

        // ordenar ascendente
        void Ordenar_Click(object sender, RoutedEventArgs e)
        {
            List<int> temp = lista.ToList();
            temp.Sort();

            lista.Clear();

            foreach (int m in temp)
                lista.AddLast(m);

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