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

        //  Comparacion
        private int Comparar(string v1, string v2)
        {
            if (rbEnteros.IsChecked == true)
            {
                if (int.TryParse(v1, out int n1) && int.TryParse(v2, out int n2))
                    return n1.CompareTo(n2);
                return 0;
            }
            return string.Compare(v1, v2);
        }

        // Insertar 
        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) 
                return;
            raiz = InsertarRecursivo(raiz, txtValor.Text);
            txtValor.Clear();
            DibujarArbol();
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;
            raiz = EliminarRecursivo(raiz, txtValor.Text);
            txtValor.Clear();
            DibujarArbol();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text)) return;

            bool encontrado = BuscarRecursivo(raiz, txtValor.Text);
            txtResultado.Text = encontrado ? "El Valor no pudo ser encontrado." : "No existe en el árbol";
            txtValor.Clear();
        }

        private void Altura_Click(object sender, RoutedEventArgs e)
        {
            txtResultado.Text = "Altura: " + GetAltura(raiz);
        }

        private void Minimo_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null) 
                return;
            txtResultado.Text = "Mínimo: " + BuscarMinimo(raiz).Valor;
        }

        private void Maximo_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null)
                return;
            Nodo aux = raiz;
            while (aux.Derecho != null) aux = aux.Derecho;
            txtResultado.Text = "Máximo: " + aux.Valor;
        }

        private void Nivel_Click(object sender, RoutedEventArgs e)
        {
            if (raiz == null)
                return;
            List<string> lista = new List<string>();
            Queue<Nodo> cola = new Queue<Nodo>();
            cola.Enqueue(raiz);
            while (cola.Count > 0)
            {
                Nodo actual = cola.Dequeue();
                lista.Add(actual.Valor);
                if (actual.Izquierdo != null) cola.Enqueue(actual.Izquierdo);
                if (actual.Derecho != null) cola.Enqueue(actual.Derecho);
            }
            txtResultado.Text = "Por Niveles: " + string.Join(" - ", lista);
        }

        private void InOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            RecIn(raiz, l);
            txtResultado.Text = "InOrden: " + string.Join(", ", l);
        }

        private void PreOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            RecPre(raiz, l);
            txtResultado.Text = "PreOrden: " + string.Join(", ", l);
        }

        private void PostOrden_Click(object sender, RoutedEventArgs e)
        {
            List<string> l = new List<string>();
            RecPost(raiz, l);
            txtResultado.Text = "PostOrden: " + string.Join(", ", l);
        }

        private void Reiniciar_Click(object sender, RoutedEventArgs e)
        {
            raiz = null;
            txtResultado.Clear();
            DibujarArbol();
        }

        // Parte Recursiva
        private Nodo InsertarRecursivo(Nodo actual, string v)
        {
            if (actual == null)
                return new Nodo(v);
            int comp = Comparar(v, actual.Valor);
            if (comp < 0) 
                actual.Izquierdo = InsertarRecursivo(actual.Izquierdo, v);
            else if (comp > 0)
                actual.Derecho = InsertarRecursivo(actual.Derecho, v);
            return actual;
        }

        private Nodo EliminarRecursivo(Nodo actual, string v)
        {
            if (actual == null) 
                return null;
            int comp = Comparar(v, actual.Valor);
            if (comp < 0)
                actual.Izquierdo = EliminarRecursivo(actual.Izquierdo, v);
            else if (comp > 0)
                actual.Derecho = EliminarRecursivo(actual.Derecho, v);
            else
            {
                if (actual.Izquierdo == null) return actual.Derecho;
                if (actual.Derecho == null) return actual.Izquierdo;
                Nodo min = BuscarMinimo(actual.Derecho);
                actual.Valor = min.Valor;
                actual.Derecho = EliminarRecursivo(actual.Derecho, min.Valor);
            }
            return actual;
        }

        private bool BuscarRecursivo(Nodo n, string v)
        {
            if (n == null) return false;
            if (n.Valor == v) return true;
            return Comparar(v, n.Valor) < 0 ? BuscarRecursivo(n.Izquierdo, v) : BuscarRecursivo(n.Derecho, v);
            txtValor.Clear();
        }

        private Nodo BuscarMinimo(Nodo n)
        {
            while (n.Izquierdo != null) n = n.Izquierdo;
            return n;
        }

        private int GetAltura(Nodo n) => n == null ? 0 : 1 + Math.Max(GetAltura(n.Izquierdo), GetAltura(n.Derecho));

        //  recorrido

        private void RecIn(Nodo n, List<string> l)
        {
            if (n != null)
            {
                RecIn(n.Izquierdo, l);
                l.Add(n.Valor);
                RecIn(n.Derecho, l);
            }
        }

        private void RecPre(Nodo n, List<string> l)
        {
            if (n != null)
            {
                l.Add(n.Valor);
                RecPre(n.Izquierdo, l);
                RecPre(n.Derecho, l);
            }
        }
       
        private void RecPost(Nodo n, List<string> l)
        {
            if (n != null)
            {
                RecPost(n.Izquierdo, l);
                RecPost(n.Derecho, l);
                l.Add(n.Valor);
            }
        }

        // CANVA
        private void DibujarArbol()
        {
            canvasArbol.Children.Clear();
            if (raiz != null) DibujarNodo(raiz, 400, 40, 180);
        }

        private void DibujarNodo(Nodo n, double x, double y, double off)
        {
            if (n == null) 
                return;
            if (n.Izquierdo != null)
            {
                canvasArbol.Children.Add(new Line { X1 = x, Y1 = y, X2 = x - off, Y2 = y + 60, Stroke = Brushes.White, StrokeThickness = 1.5 });
                DibujarNodo(n.Izquierdo, x - off, y + 60, off / 2);
            }
            if (n.Derecho != null)
            {
                canvasArbol.Children.Add(new Line { X1 = x, Y1 = y, X2 = x + off, Y2 = y + 60, Stroke = Brushes.White, StrokeThickness = 1.5 });
                DibujarNodo(n.Derecho, x + off, y + 60, off / 2);
            }
            Ellipse ell = new Ellipse { Width = 30, Height = 30, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C084FC")), Stroke = Brushes.White };
            Canvas.SetLeft(ell, x - 15); Canvas.SetTop(ell, y - 15);
            canvasArbol.Children.Add(ell);
            TextBlock t = new TextBlock { Text = n.Valor, Foreground = Brushes.Black, FontWeight = FontWeights.Bold, FontSize = 11 };
            Canvas.SetLeft(t, x - 6); Canvas.SetTop(t, y - 8);
            canvasArbol.Children.Add(t);
        }
    }
}