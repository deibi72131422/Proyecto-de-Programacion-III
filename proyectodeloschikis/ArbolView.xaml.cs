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
    /// Lógica de interacción para ArbolView.xaml
    /// </summary>
    public partial class ArbolView : UserControl
    {
        public ArbolView()
        {
            InitializeComponent();
        }
        // chavales esto me dio chatsito para probar el arbol, ns si es correcto, ustedebn ven, pero funciona medio raro, estoy usando un canvas para dibujar el arbol, y cada vez que inserto un nodo, borro todo el canvas y lo vuelvo a dibujar, no se si es la mejor forma, pero es la que se me ocurrio, si tienen alguna sugerencia para mejorar esto, soy todo oidos
        public class Nodo
        {
            public int Valor;
            public Nodo Izq;
            public Nodo Der;

            public Nodo(int valor)
            {
                Valor = valor;
                Izq = null;
                Der = null;
            }
        }
        Nodo raiz = null;

        Nodo Insertar(Nodo nodo, int valor)
        {
            if (nodo == null)
                return new Nodo(valor);

            if (valor < nodo.Valor)
                nodo.Izq = Insertar(nodo.Izq, valor);
            else
                nodo.Der = Insertar(nodo.Der, valor);

            return nodo;
        }
        private void Insertar_Click(object sender, RoutedEventArgs e)
        {
            int valor = int.Parse(txtValor.Text);

            raiz = Insertar(raiz, valor);

            DibujarArbol();
        }
        void DibujarArbol()
        {
            canvasArbol.Children.Clear();

            DibujarNodo(raiz, 400, 20, 150);
        }
        void DibujarNodo(Nodo nodo, double x, double y, double offset)
        {
            if (nodo == null) return;

            // 🔵 CÍRCULO
            Ellipse circulo = new Ellipse
            {
                Width = 35,
                Height = 35,
                Fill = Brushes.MediumPurple
            };

            Canvas.SetLeft(circulo, x);
            Canvas.SetTop(circulo, y);

            canvasArbol.Children.Add(circulo);

            // 🔤 TEXTO
            TextBlock texto = new TextBlock
            {
                Text = nodo.Valor.ToString(),
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };

            Canvas.SetLeft(texto, x + 10);
            Canvas.SetTop(texto, y + 5);

            canvasArbol.Children.Add(texto);

            // 🔻 IZQUIERDA
            if (nodo.Izq != null)
            {
                Line linea = new Line
                {
                    X1 = x + 15,
                    Y1 = y + 35,
                    X2 = x - offset + 15,
                    Y2 = y + 80,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };

                canvasArbol.Children.Add(linea);

                DibujarNodo(nodo.Izq, x - offset, y + 80, offset / 2);
            }

            // 🔺 DERECHA
            if (nodo.Der != null)
            {
                Line linea = new Line
                {
                    X1 = x + 15,
                    Y1 = y + 35,
                    X2 = x + offset + 15,
                    Y2 = y + 80,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                };

                canvasArbol.Children.Add(linea);

                DibujarNodo(nodo.Der, x + offset, y + 80, offset / 2);
            }
        }
    }
}
