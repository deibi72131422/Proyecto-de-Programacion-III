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

        private bool HayDatos()
        {
            if (datos.Count == 0)
            {
                MessageBox.Show("no hay datos cargados");
                return false;
            }
            return true;
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

        private void btnCargar_Click(object sender, RoutedEventArgs e)
        {
            if (!validacion.EsEntero(txtCantidad.Text, out int cantidad))
            {
                txtCantidad.Clear();
                return;
            }

            Random rnd = new Random();
            datos.Clear();

            for (int a = 0; a < cantidad; a++)
                datos.Add(rnd.Next(1, 100));

            MostrarOriginal();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (!validacion.EsEntero(txtNumero.Text, out int numero))
            {
                txtNumero.Clear();
                return;
            }

            datos.Add(numero);
            txtNumero.Clear();
            MostrarOriginal();
        }

        // BURBUJA
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

        // SELECCION
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

        // INSERCION
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

        // QUICK SORT
        private void btnQuick_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);
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

        // MERGE
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

            List<int> izq = MergeSort(lista.GetRange(0, medio));
            List<int> der = MergeSort(lista.GetRange(medio, lista.Count - medio));

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

        // RADIX
        private void btnRadix_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            int max = lista[0];
            foreach (int x in lista)
                if (x > max) max = x;

            for (int exp = 1; max / exp > 0; exp *= 10)
                CountingSort(lista, exp);

            if (!EsAscendente())
                lista.Reverse();

            MostrarOrdenado(lista);
        }

        private void CountingSort(List<int> lista, int exp)
        {
            int n = lista.Count;
            int[] salida = new int[n];
            int[] conteo = new int[10];

            for (int a = 0; a < n; a++)
                conteo[(lista[a] / exp) % 10]++;

            for (int a = 1; a < 10; a++)
                conteo[a] += conteo[a - 1];

            for (int a = n - 1; a >= 0; a--)
            {
                salida[conteo[(lista[a] / exp) % 10] - 1] = lista[a];
                conteo[(lista[a] / exp) % 10]--;
            }

            for (int a = 0; a < n; a++)
                lista[a] = salida[a];
        }

        // BUCKET
        private void btnBucket_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            int max = lista[0];
            foreach (int x in lista)
                if (x > max) max = x;

            List<int>[] buckets = new List<int>[10];

            for (int a = 0; a < 10; a++)
                buckets[a] = new List<int>();

            foreach (int x in lista)
            {
                int index = x * 10 / (max + 1);
                buckets[index].Add(x);
            }

            lista.Clear();

            foreach (List<int> bucket in buckets)
            {
                for (int a = 1; a < bucket.Count; a++)
                {
                    int key = bucket[a];
                    int m = a - 1;

                    while (m >= 0 && bucket[m] > key)
                    {
                        bucket[m + 1] = bucket[m];
                        m--;
                    }

                    bucket[m + 1] = key;
                }

                foreach (int x in bucket)
                    lista.Add(x);
            }

            if (!EsAscendente())
                lista.Reverse();

            MostrarOrdenado(lista);
        }

        // SHELL
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

        // HEAP
        private void btnHeap_Click(object sender, RoutedEventArgs e)
        {
            if (!HayDatos()) return;

            List<int> lista = new List<int>(datos);

            int n = lista.Count;

            for (int a = n / 2 - 1; a >= 0; a--)
                Heapify(lista, n, a);

            for (int a = n - 1; a >= 0; a--)
            {
                int temp = lista[0];
                lista[0] = lista[a];
                lista[a] = temp;

                Heapify(lista, a, 0);
            }

            if (!EsAscendente())
                lista.Reverse();

            MostrarOrdenado(lista);
        }

        private void Heapify(List<int> lista, int n, int i)
        {
            int mayor = i;
            int izq = 2 * i + 1;
            int der = 2 * i + 2;

            if (izq < n && lista[izq] > lista[mayor])
                mayor = izq;

            if (der < n && lista[der] > lista[mayor])
                mayor = der;

            if (mayor != i)
            {
                int temp = lista[i];
                lista[i] = lista[mayor];
                lista[mayor] = temp;

                Heapify(lista, n, mayor);
            }
        }
        // BUSQUEDA SECUENCIAL
        private void btnSecuencial_Click(object sender, RoutedEventArgs e)
        {
            if (!validacion.EsEntero(txtBuscar.Text, out int valor))
            {
                txtBuscar.Clear();
                return;
            }

            int posicion = -1;

            for (int i = 0; i < datos.Count; i++)
            {
                if (datos[i] == valor)
                {
                    posicion = i;
                    break;
                }
            }

            if (posicion == -1)
                txtResultado.Text = "No encontrado";
            else
                txtResultado.Text = "Encontrado en posición: " + posicion;
        }


        // BUSQUEDA BINARIA
        private void btnBinaria_Click(object sender, RoutedEventArgs e)
        {
            if (!validacion.EsEntero(txtBuscar.Text, out int valor))
            {
                txtBuscar.Clear();
                return;
            }

            List<int> lista = new List<int>(datos);

            // ordenar primero
            lista.Sort();

            int inicio = 0;
            int fin = lista.Count - 1;
            int pos = -1;

            while (inicio <= fin)
            {
                int medio = (inicio + fin) / 2;

                if (lista[medio] == valor)
                {
                    pos = medio;
                    break;
                }

                if (lista[medio] < valor)
                    inicio = medio + 1;
                else
                    fin = medio - 1;
            }

            if (pos == -1)
                txtResultado.Text = "No encontrado";
            else
                txtResultado.Text = "Encontrado en posición: " + pos;
        }

        private void btnLimpiarOrdenado_Click(object sender, RoutedEventArgs e)
        {
            listaOrdenado.Items.Clear();
        }

        private void btnLimpiarTodo_Click(object sender, RoutedEventArgs e)
        {
            datos.Clear();
            listaOriginal.Items.Clear();
            listaOrdenado.Items.Clear();
            txtResultado.Text = "";
        }
    }
}