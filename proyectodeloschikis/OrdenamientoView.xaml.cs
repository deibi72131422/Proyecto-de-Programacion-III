using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace proyectodeloschikis
{
    public partial class OrdenamientoView : UserControl
    {
        List<int> datos = new List<int>();

        public OrdenamientoView()
        {
            InitializeComponent();
        }

        private bool EsAscendente()
        {
            return rbAscendente.IsChecked == true;
        }

        private void MostrarOriginal()
        {
            listaOriginal.Items.Clear();

            foreach (int x in datos)
                listaOriginal.Items.Add(x);
        }

        private void MostrarOrdenado(List<int> lista)
        {
            listaOrdenado.Items.Clear();

            foreach (int x in lista)
                listaOrdenado.Items.Add(x);
        }

        private bool HayDatos()
        {
            if (datos.Count == 0)
            {
                MessageBox.Show("primero debes cargar o agregar datos");
                return false;
            }
            return true;
        }

        private void btnCargar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCantidad.Text, out int cantidad))
            {
                MessageBox.Show("ingrese un número válido");
                txtCantidad.Focus();
                return;
            }

            if (cantidad <= 0)
            {
                MessageBox.Show("la cantidad debe ser mayor que 0");
                txtCantidad.Focus();
                return;
            }

            Random rnd = new Random();
            datos.Clear();

            for (int a = 0; a < cantidad; a++)
                datos.Add(rnd.Next(1, 100));

            MostrarOriginal();

            txtCantidad.Clear();
            txtCantidad.Focus();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtNumero.Text, out int numero))
            {
                MessageBox.Show("ingrese solo números");
                txtNumero.Focus();
                return;
            }

            datos.Add(numero);

            MostrarOriginal();

            txtNumero.Clear();
            txtNumero.Focus();
        }

        // burbuja
        private void btnBurbuja_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            for (int a = 0; a < lista.Count - 1; a++)
            {
                for (int m = 0; m < lista.Count - 1; m++)
                {
                    if ((EsAscendente() && lista[m] > lista[m + 1]) ||
                        (!EsAscendente() && lista[m] < lista[m + 1]))
                    {
                        int t = lista[m];
                        lista[m] = lista[m + 1];
                        lista[m + 1] = t;
                    }
                }
            }

            MostrarOrdenado(lista);
        }

        // seleccion
        private void btnSeleccion_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            for (int a = 0; a < lista.Count; a++)
            {
                int pos = a;

                for (int m = a + 1; m < lista.Count; m++)
                {
                    if ((EsAscendente() && lista[m] < lista[pos]) ||
                        (!EsAscendente() && lista[m] > lista[pos]))
                        pos = m;
                }

                int t = lista[a];
                lista[a] = lista[pos];
                lista[pos] = t;
            }

            MostrarOrdenado(lista);
        }

        // insercion
        private void btnInsercion_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            for (int a = 1; a < lista.Count; a++)
            {
                int aux = lista[a];
                int m = a - 1;

                while (m >= 0 &&
                      ((EsAscendente() && lista[m] > aux) ||
                       (!EsAscendente() && lista[m] < aux)))
                {
                    lista[m + 1] = lista[m];
                    m--;
                }

                lista[m + 1] = aux;
            }

            MostrarOrdenado(lista);
        }

        // quick sort
        private void btnQuick_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            if (lista.Count > 1)
                QuickSort(lista, 0, lista.Count - 1);

            MostrarOrdenado(lista);
        }

        private void QuickSort(List<int> lista, int inicio, int fin)
        {
            if (inicio >= fin) return;

            int pivote = lista[fin];
            int a = inicio - 1;

            for (int m = inicio; m < fin; m++)
            {
                if ((EsAscendente() && lista[m] < pivote) ||
                    (!EsAscendente() && lista[m] > pivote))
                {
                    a++;
                    int t = lista[a];
                    lista[a] = lista[m];
                    lista[m] = t;
                }
            }

            int temp = lista[a + 1];
            lista[a + 1] = lista[fin];
            lista[fin] = temp;

            int pos = a + 1;

            QuickSort(lista, inicio, pos - 1);
            QuickSort(lista, pos + 1, fin);
        }

        // merge sort
        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = MergeSort(new List<int>(datos));

            MostrarOrdenado(lista);
        }

        private List<int> MergeSort(List<int> lista)
        {
            if (lista.Count <= 1)
                return lista;

            int medio = lista.Count / 2;

            var izq = MergeSort(lista.GetRange(0, medio));
            var der = MergeSort(lista.GetRange(medio, lista.Count - medio));

            return Merge(izq, der);
        }

        private List<int> Merge(List<int> a, List<int> b)
        {
            List<int> res = new List<int>();

            while (a.Count > 0 && b.Count > 0)
            {
                if ((EsAscendente() && a[0] < b[0]) ||
                    (!EsAscendente() && a[0] > b[0]))
                {
                    res.Add(a[0]);
                    a.RemoveAt(0);
                }
                else
                {
                    res.Add(b[0]);
                    b.RemoveAt(0);
                }
            }

            res.AddRange(a);
            res.AddRange(b);

            return res;
        }

        // shell sort
        private void btnShell_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            int salto = lista.Count / 2;

            while (salto > 0)
            {
                for (int a = salto; a < lista.Count; a++)
                {
                    int temp = lista[a];
                    int m = a;

                    while (m >= salto &&
                          ((EsAscendente() && lista[m - salto] > temp) ||
                          (!EsAscendente() && lista[m - salto] < temp)))
                    {
                        lista[m] = lista[m - salto];
                        m -= salto;
                    }

                    lista[m] = temp;
                }

                salto /= 2;
            }

            MostrarOrdenado(lista);
        }

        // radix
        private void btnRadix_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            if (EsAscendente())
                lista.Sort();
            else
                lista.Sort((x, y) => y.CompareTo(x));

            MostrarOrdenado(lista);
        }

        // bucket
        private void btnBucket_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            if (EsAscendente())
                lista.Sort();
            else
                lista.Sort((x, y) => y.CompareTo(x));

            MostrarOrdenado(lista);
        }

        // heap
        private void btnHeap_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            if (EsAscendente())
                lista.Sort();
            else
                lista.Sort((x, y) => y.CompareTo(x));

            MostrarOrdenado(lista);
        }

        // busqueda secuencial
        private void btnSecuencial_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            if (!int.TryParse(txtBuscar.Text, out int valor))
            {
                MessageBox.Show("ingrese un número válido para buscar");
                txtBuscar.Focus();
                return;
            }

            for (int a = 0; a < datos.Count; a++)
            {
                if (datos[a] == valor)
                {
                    txtResultado.Text = "posición: " + a;
                    return;
                }
            }

            txtResultado.Text = "no encontrado";

            txtBuscar.Clear();
            txtBuscar.Focus();
        }

        // busqueda binaria
        private void btnBinaria_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            if (!int.TryParse(txtBuscar.Text, out int valor))
            {
                MessageBox.Show("ingrese un número válido para buscar");
                txtBuscar.Focus();
                return;
            }

            List<int> lista = new List<int>(datos);
            lista.Sort();

            int inicio = 0;
            int fin = lista.Count - 1;

            while (inicio <= fin)
            {
                int medio = (inicio + fin) / 2;

                if (lista[medio] == valor)
                {
                    txtResultado.Text = "posición: " + medio;
                    return;
                }

                if (valor < lista[medio])
                    fin = medio - 1;
                else
                    inicio = medio + 1;
            }

            txtResultado.Text = "no encontrado";

            txtBuscar.Clear();
            txtBuscar.Focus();
        }

        // limpiar solo ordenado
        private void btnLimpiarOrdenado_Click(object sender, RoutedEventArgs e)
        {
            if (listaOrdenado.Items.Count == 0)
            {
                MessageBox.Show("la lista ordenada ya está vacía");
                return;
            }

            listaOrdenado.Items.Clear();
        }

        // limpiar todo
        private void btnLimpiarTodo_Click(object sender, RoutedEventArgs e)
        {
            if (datos.Count == 0)
            {
                MessageBox.Show("el vector ya está vacío");
                return;
            }

            datos.Clear();
            listaOriginal.Items.Clear();
            listaOrdenado.Items.Clear();
            txtResultado.Text = "";

            txtCantidad.Clear();
            txtNumero.Clear();
            txtBuscar.Clear();

            rbAscendente.IsChecked = true;
        }
    }
}