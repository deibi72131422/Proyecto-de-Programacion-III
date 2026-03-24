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
    public class Nodo
    {
        public string Valor { get; set; }
        public Nodo Izquierdo { get; set; }
        public Nodo Derecho { get; set; }
        public Nodo(string v) { Valor = v; }
    }

    public partial class ArbolView : UserControl
    {
        private Nodo raiz = null;

        public ArbolView()
        {
            InitializeComponent();
        }
        // Comparacion de los datos

        private int Comparar(string valorNuevo, string valorNodo)
        {
            if (rbEnteros.IsChecked == true)
            {
                int n1 = int.Parse(valorNuevo);
                int n2 = int.Parse(valorNodo);
                return n1.CompareTo(n2);
            }
            else
            {
                return string.Compare(valorNuevo, valorNodo);
            }
        }

        // Botones 
   

        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;

            try
            {
                raiz = InsertarRecursivo(raiz, txtValor.Text);
                txtValor.Clear();
                txtResultado.Text = "Nodo insertado.";
                DibujarArbol();
            }
            catch
            {
                txtResultado.Text = "Error: Tipo de dato incorrecto.";
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;
            bool existe = BuscarNodo(raiz, txtValor.Text);
            txtResultado.Text = existe ? "Encontrado." : "No encontrado.";
        }

        private void Reiniciar_Click(object sender, RoutedEventArgs e)
        {
            raiz = null;
            txtResultado.Clear();
            txtValor.Clear();
            DibujarArbol();
        }

        // Parte de los Recorridos

        private void InOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> lista = new List<string>();
            RecorridoInOrden(raiz, lista);
            txtResultado.Text = "In-Orden: " + string.Join(" - ", lista);
        }

        private void RecorridoInOrden(Nodo nodo, List<string> lista)
        {
            if (nodo == null) return;
            RecorridoInOrden(nodo.Izquierdo, lista);
            lista.Add(nodo.Valor);
            RecorridoInOrden(nodo.Derecho, lista);
        }

        private void PreOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> lista = new List<string>();
            RecorridoPreOrden(raiz, lista);
            txtResultado.Text = "Pre-Orden: " + string.Join(" - ", lista);
        }

        private void RecorridoPreOrden(Nodo nodo, List<string> lista)
        {
            if (nodo == null) return;
            lista.Add(nodo.Valor);
            RecorridoPreOrden(nodo.Izquierdo, lista);
            RecorridoPreOrden(nodo.Derecho, lista);
        }

        private void PostOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> lista = new List<string>();
            RecorridoPostOrden(raiz, lista);
            txtResultado.Text = "Post-Orden: " + string.Join(" - ", lista);
        }

        private void RecorridoPostOrden(Nodo nodo, List<string> lista)
        {
            if (nodo == null) return;
            RecorridoPostOrden(nodo.Izquierdo, lista);
            RecorridoPostOrden(nodo.Derecho, lista);
            lista.Add(nodo.Valor);
        }
        // Logica recursiva del Arbol

        private Nodo InsertarRecursivo(Nodo actual, string valor)
        {
            if (actual == null) return new Nodo(valor);

            int comparacion = Comparar(valor, actual.Valor);

            if (comparacion < 0)
            {
                actual.Izquierdo = InsertarRecursivo(actual.Izquierdo, valor);
            }
            else if (comparacion > 0)
            {
                actual.Derecho = InsertarRecursivo(actual.Derecho, valor);
            }

            return actual;
        }

        private bool BuscarNodo(Nodo n, string v)
        {
            if (n == null) return false;
            if (n.Valor == v) return true;

            int comparacion = Comparar(v, n.Valor);

            if (comparacion < 0)
                return BuscarNodo(n.Izquierdo, v);
            else
                return BuscarNodo(n.Derecho, v);
        }

        // Parte con CANVA
        private void DibujarArbol()
        {
            canvasArbol.Children.Clear();
            if (raiz != null)
                DibujarNodoRecursivo(raiz, 1000, 50, 250);
        }

        private void DibujarNodoRecursivo(Nodo n, double x, double y, double off)
        {
            if (n == null) return;

            // Líneas
            if (n.Izquierdo != null)
            {
                Line l = new Line { X1 = x, Y1 = y, X2 = x - off, Y2 = y + 70, Stroke = Brushes.White, StrokeThickness = 2 };
                canvasArbol.Children.Add(l);
                DibujarNodoRecursivo(n.Izquierdo, x - off, y + 70, off / 2);
            }
            if (n.Derecho != null)
            {
                Line l = new Line { X1 = x, Y1 = y, X2 = x + off, Y2 = y + 70, Stroke = Brushes.White, StrokeThickness = 2 };
                canvasArbol.Children.Add(l);
                DibujarNodoRecursivo(n.Derecho, x + off, y + 70, off / 2);
            }

            // Círculo
            Ellipse circle = new Ellipse { Width = 34, Height = 34, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C084FC")), Stroke = Brushes.White };
            Canvas.SetLeft(circle, x - 17);
            Canvas.SetTop(circle, y - 17);
            canvasArbol.Children.Add(circle);

            // Valor
            TextBlock txt = new TextBlock { Text = n.Valor, Foreground = Brushes.Black, FontWeight = FontWeights.Bold };
            Canvas.SetLeft(txt, x - 7);
            Canvas.SetTop(txt, y - 9);
            canvasArbol.Children.Add(txt);
        }

        // Acciones de los Botones restantes
        private void Altura_Click(object sender, RoutedEventArgs e) => txtResultado.Text = "Altura: " + GetAltura(raiz);
        private int GetAltura(Nodo n) => n == null ? 0 : 1 + Math.Max(GetAltura(n.Izquierdo), GetAltura(n.Derecho));

        private void Eliminar_Click(object sender, RoutedEventArgs e) { }
        private void Nivel_Click(object sender, RoutedEventArgs e) { }
        private void Minimo_Click(object sender, RoutedEventArgs e) { }
        private void Maximo_Click(object sender, RoutedEventArgs e) { }
    }
}