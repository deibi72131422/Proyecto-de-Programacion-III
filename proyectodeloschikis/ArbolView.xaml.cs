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
    // esta clase es para crear cada bolita del arbol
    public class Nodo
    {
        public string Valor { get; set; }
        public Nodo Izquierdo { get; set; }
        public Nodo Derecho { get; set; }

        public Nodo(string v)
        {
            Valor = v;
        }
    }
    public partial class ArbolView : UserControl
    {
        private Nodo raiz = null;
        private enum DataMode { Enteros, Caracteres, Alfa }
        private DataMode? modoSeleccionado = null; // modo fijado al primer insertar

        public ArbolView()
        {
            InitializeComponent();
        }

        private int Comparar(string v1, string v2)
        {
            // usar el modo fijado si existe, sino la selección actual del radio
            bool usarEnteros = (modoSeleccionado == DataMode.Enteros) || (modoSeleccionado == null && rbEnteros.IsChecked == true);
            if (usarEnteros)
            {
                if (int.TryParse(v1, out int n1) && int.TryParse(v2, out int n2))
                    return n1.CompareTo(n2);
            }
            return string.Compare(v1, v2, StringComparison.Ordinal);
        }

        //  INSERTAR
        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.NoVacio(txtValor.Text)) return;

                string valorRaw = txtValor.Text;

                // Si no hay modo fijado, fijarlo según el radio seleccionado ahora
                if (modoSeleccionado == null)
                {
                    if (rbEnteros.IsChecked == true) modoSeleccionado = DataMode.Enteros;
                    else if (rbCaracteres.IsChecked == true) modoSeleccionado = DataMode.Caracteres;
                    else modoSeleccionado = DataMode.Alfa;

                    // deshabilitar cambios de modo mientras el árbol tenga datos
                    rbEnteros.IsEnabled = false;
                    rbCaracteres.IsEnabled = false;
                    rbAlfaNumerico.IsEnabled = false;
                }

                if (!ValidarEntradaArbol(valorRaw, out string valor))
                {
                    txtValor.Clear();
                    return;
                }

                //  VALIDACIÓN DE DUPLICADOS
                if (Encontrar(raiz, valor) != null)
                {
                    MessageBox.Show("Ese valor ya existe", "Aviso",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    txtValor.Clear();
                    return;
                }
                //  insertar si todo está bien
                raiz = InsertarRec(raiz, valor);

                txtValor.Clear();
                DibujarArbol();
            });
        }

        private Nodo InsertarRec(Nodo n, string v)
        {

            if (n == null) return new Nodo(v);

            int c = Comparar(v, n.Valor);
            if (c < 0) n.Izquierdo = InsertarRec(n.Izquierdo, v);
            else if (c > 0) n.Derecho = InsertarRec(n.Derecho, v);

            return n;
        }

        //  ELIMINAR
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.NoVacio(txtValor.Text)) return;

                if (raiz == null)
                {
                    txtValor.Clear();
                    return;
                }

                string valorRaw = txtValor.Text;

                if (!ValidarEntradaArbol(valorRaw, out string valor))
                {
                    txtValor.Clear();
                    return;
                }

                raiz = EliminarRec(raiz, valor);
                txtValor.Clear();
                DibujarArbol();
            });
        }

        private Nodo EliminarRec(Nodo n, string v)
        {
            if (n == null) return null;

            int c = Comparar(v, n.Valor);
            if (c < 0) n.Izquierdo = EliminarRec(n.Izquierdo, v);
            else if (c > 0) n.Derecho = EliminarRec(n.Derecho, v);
            else
            {
                if (n.Izquierdo == null) return n.Derecho;
                if (n.Derecho == null) return n.Izquierdo;

                Nodo m = n.Derecho;
                while (m.Izquierdo != null) m = m.Izquierdo;

                n.Valor = m.Valor;
                n.Derecho = EliminarRec(n.Derecho, m.Valor);
            }
            return n;
        }

        //  BALANCEAR
        private void Balancear_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) return;

            List<string> l = new List<string>();
            PasarALista(raiz, l);
            raiz = CrearBalanceado(l, 0, l.Count - 1);

            DibujarArbol();
            txtResultado.Text = "Arbol equilibrado correctamente";
        }

        private void PasarALista(Nodo n, List<string> l)
        {
            if (n == null) return;
            PasarALista(n.Izquierdo, l);
            l.Add(n.Valor);
            PasarALista(n.Derecho, l);
        }

        private Nodo CrearBalanceado(List<string> l, int ini, int fin)
        {
            if (ini > fin) return null;

            int m = (ini + fin) / 2;
            Nodo n = new Nodo(l[m]);

            n.Izquierdo = CrearBalanceado(l, ini, m - 1);
            n.Derecho = CrearBalanceado(l, m + 1, fin);

            return n;
        }

        //  BUSCAR
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.NoVacio(txtValor.Text)) return;

                if (raiz == null)
                {
                    txtResultado.Text = "El árbol está vacío";
                    txtValor.Clear();
                    return;
                }

                string valorRaw = txtValor.Text;
                if (!ValidarEntradaArbol(valorRaw, out string valor))
                {
                    txtValor.Clear();
                    return;
                }

                txtResultado.Text = (Encontrar(raiz, valor) != null)
                    ? "Si existe"
                    : "No existe";

                txtValor.Clear();
            });
        }

        private Nodo Encontrar(Nodo n, string v)
        {
            if (n == null || n.Valor == v) return n;

            int c = Comparar(v, n.Valor);
            return c < 0 ? Encontrar(n.Izquierdo, v) : Encontrar(n.Derecho, v);
        }

        //  NIVEL
        private void Nivel_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.NoVacio(txtValor.Text)) return;

                if (raiz == null)
                {
                    txtResultado.Text = "El árbol está vacío";
                    txtValor.Clear();
                    return;
                }

                string valorRaw = txtValor.Text;
                if (!ValidarEntradaArbol(valorRaw, out string valor))
                {
                    txtValor.Clear();
                    return;
                }

                int niv = GetNiv(raiz, valor, 1);
                txtResultado.Text = niv != -1 ? "Nivel: " + niv : "No encontrado";

                txtValor.Clear();
            });
        }

        private int GetNiv(Nodo n, string v, int niv)
        {
            if (n == null) return -1;

            int c = Comparar(v, n.Valor);
            if (c == 0) return niv;

            return c < 0
                ? GetNiv(n.Izquierdo, v, niv + 1)
                : GetNiv(n.Derecho, v, niv + 1);
        }

        //  ALTURA
        private void Altura_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = "Altura: " + GetAlt(raiz);
        }

        private int GetAlt(Nodo n)
        {
            if (n == null) return 0;
            return 1 + Math.Max(GetAlt(n.Izquierdo), GetAlt(n.Derecho));
        }

        // MINIMO
        private void Minimo_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) return;

            Nodo aux = raiz;
            while (aux.Izquierdo != null)
                aux = aux.Izquierdo;

            txtResultado.Text = "Minimo: " + aux.Valor;
        }

        //  MAXIMO
        private void Maximo_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) return;

            Nodo aux = raiz;
            while (aux.Derecho != null)
                aux = aux.Derecho;

            txtResultado.Text = "Maximo: " + aux.Valor;
        }

        // RECORRIDOS
        private void InOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            PasarALista(raiz, l);
            txtResultado.Text = "InOrden: " + string.Join(", ", l);
        }

        private void PreOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            Pre(raiz, l);
            txtResultado.Text = "PreOrden: " + string.Join(", ", l);
        }

        private void Pre(Nodo n, List<string> l)
        {
            if (n == null) return;
            l.Add(n.Valor);
            Pre(n.Izquierdo, l);
            Pre(n.Derecho, l);
        }

        private void PostOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            Post(raiz, l);
            txtResultado.Text = "PostOrden: " + string.Join(", ", l);
        }

        private void Post(Nodo n, List<string> l)
        {
            if (n == null) return;
            Post(n.Izquierdo, l);
            Post(n.Derecho, l);
            l.Add(n.Valor);
        }

        // REINICIAR
        private void Reiniciar_Click(object sender, RoutedEventArgs e)
        {
            raiz = null;
            canvasArbol.Children.Clear();
            txtResultado.Clear();
            txtValor.Clear();
            // permitir cambiar modo nuevamente
            modoSeleccionado = null;
            rbEnteros.IsEnabled = true;
            rbCaracteres.IsEnabled = true;
            rbAlfaNumerico.IsEnabled = true;
        }

        // Valida la entrada para el árbol según el modo seleccionado (enteros/caracteres/alfa)
        private bool ValidarEntradaArbol(string entrada, out string salida)
        {
            salida = entrada?.Trim() ?? "";

            if (modoSeleccionado == DataMode.Enteros)
            {
                if (!validacion.EsEntero(salida, out int n)) return false;
                salida = n.ToString();
                return true;
            }

            if (modoSeleccionado == DataMode.Caracteres)
            {
                if (!validacion.EsCaracter(salida, out char c)) return false;
                salida = c.ToString();
                return true;
            }

            // Alfa: alfanumérico, usar utilitario
            if (!validacion.EsAlfaNumerico(salida, out string v, 50)) return false;
            salida = v;
            return true;
        }
        private void DibujarArbol()
        {
            canvasArbol.Children.Clear();

            if (raiz != null)
            {
                DibujarNodo(raiz, 400, 40, 160);
            }
        }
        private void DibujarNodo(Nodo n, double x, double y, double s)
        {
            if (n == null) return;

            if (n.Izquierdo != null)
            {
                Line l = new Line
                {
                    X1 = x,
                    Y1 = y,
                    X2 = x - s,
                    Y2 = y + 60,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };
                canvasArbol.Children.Add(l);
                DibujarNodo(n.Izquierdo, x - s, y + 60, s / 2);
            }

            if (n.Derecho != null)
            {
                Line l = new Line
                {
                    X1 = x,
                    Y1 = y,
                    X2 = x + s,
                    Y2 = y + 60,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };
                canvasArbol.Children.Add(l);
                DibujarNodo(n.Derecho, x + s, y + 60, s / 2);
            }

            Ellipse el = new Ellipse
            {
                Width = 32,
                Height = 32,
                Fill = Brushes.MediumPurple,
                Stroke = Brushes.White,
                StrokeThickness = 1.5
            };

            Canvas.SetLeft(el, x - 16);
            Canvas.SetTop(el, y - 16);
            canvasArbol.Children.Add(el);

            TextBlock t = new TextBlock
            {
                Text = n.Valor,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                FontSize = 13
            };

            Canvas.SetLeft(t, x - 9);
            Canvas.SetTop(t, y - 9);
            canvasArbol.Children.Add(t);
        }
    }
}