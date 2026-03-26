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
    public class Arista
    {
        public Vertice D_Destino { get; set; }
        public int D_Peso { get; set; } = 1;
        public Arista(Vertice d) { D_Destino = d; }
    }
    public class Vertice
    {
        public string Nombre { get; set; }
        public Point D_Pos { get; set; }
        public List<Arista> D_AristasSalientes { get; set; } = new List<Arista>();
        public Vertice(string n, Point p) { Nombre = n; D_Pos = p; }
    }
    public partial class GrafosView : UserControl
    {
        private List<Vertice> D_L = new List<Vertice>();
        private int D_ID = 1;
        private const double D_Radio = 15;
        public GrafosView()
        {
            InitializeComponent();
            canvasGrafo.MouseLeftButtonDown += CanvasGrafo_MouseLeftButtonDown;
        }
        private void CanvasGrafo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point pD = e.GetPosition(canvasGrafo);
            // Agrega un nuevo nodo usando el ID actual como nombre
            D_L.Add(new Vertice(D_ID.ToString(), pD));
            D_ID++;
            D_RedibujarTodo();
        }
        private void NuevoVertice_Click(object sender, RoutedEventArgs e)
        {
            // Busca los nodos por el texto ingresado en los cuadros de texto
            var vO = D_L.FirstOrDefault(x => x.Nombre == txtOrigen.Text);
            var vD = D_L.FirstOrDefault(x => x.Nombre == txtDestino.Text);

            if (vO != null && vD != null && vO != vD)
            {
                // Verifica si la conexion ya existe antes de agregarla
                if (!vO.D_AristasSalientes.Any(a => a.D_Destino == vD))
                {
                    vO.D_AristasSalientes.Add(new Arista(vD));
                    D_RedibujarTodo();
                }
            }
            D_LimpiarTextBox();
        }
        private void AgregarLazo_Click(object sender, RoutedEventArgs e)
        {
            var vD = D_L.FirstOrDefault(x => x.Nombre == txtOrigen.Text);
            // Agrega una arista que apunta al mismo nodo origen
            if (vD != null && !vD.D_AristasSalientes.Any(a => a.D_Destino == vD))
            {
                vD.D_AristasSalientes.Add(new Arista(vD));
                D_RedibujarTodo();
            }
            D_LimpiarTextBox();
        }

        private void VaciarGrafo_Click(object sender, RoutedEventArgs e)
        {
            D_L.Clear();
            D_ID = 1;
            txtVertices.Text = "0";
            txtAristas.Text = "0";
            D_LimpiarTextBox();
            D_RedibujarTodo();
        }
        private void BFS_Click(object sender, RoutedEventArgs e)
        {
            var vI = D_L.FirstOrDefault(x => x.Nombre == txtOrigen.Text);
            if (vI == null)
            {
                MessageBox.Show("Por favor, escribe el número del nodo de origen en la primera caja.");
                return;
            }

            var visD = new List<Vertice>();
            var colD = new Queue<Vertice>();

            // Inicia la exploracion por niveles usando una cola
            colD.Enqueue(vI);
            visD.Add(vI);

            while (colD.Count > 0)
            {
                var aD = colD.Dequeue();
                foreach (var rD in aD.D_AristasSalientes)
                {
                    if (!visD.Contains(rD.D_Destino))
                    {
                        visD.Add(rD.D_Destino);
                        colD.Enqueue(rD.D_Destino);
                    }
                }
            }
            MessageBox.Show("Recorrido BFS: " + string.Join(" -> ", visD.Select(x => x.Nombre)));
            D_LimpiarTextBox();
        }

        private void DFS_Click(object sender, RoutedEventArgs e)
        {
            var vI = D_L.FirstOrDefault(x => x.Nombre == txtOrigen.Text);
            if (vI == null)
            {
                MessageBox.Show("Por favor, escribe el número del nodo de origen en la primera caja.");
                return;
            }

            var visD = new List<Vertice>();
            D_Recurse(vI, visD);
            MessageBox.Show("Recorrido DFS: " + string.Join(" -> ", visD.Select(x => x.Nombre)));
            D_LimpiarTextBox();
        }

        private void D_Recurse(Vertice v, List<Vertice> vis)
        {
            // Metodo recursivo para profundizar en cada rama del grafo
            vis.Add(v);
            foreach (var a in v.D_AristasSalientes)
            {
                if (!vis.Contains(a.D_Destino)) D_Recurse(a.D_Destino, vis);
            }
        }

        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            var vO = D_L.FirstOrDefault(x => x.Nombre == txtOrigen.Text);
            var vD = D_L.FirstOrDefault(x => x.Nombre == txtDestino.Text);

            if (vO == null || vD == null)
            {
                MessageBox.Show("Para Dijkstra debes poner el origen en la primera caja y el destino en la segunda.");
                return;
            }

            var distD = new Dictionary<Vertice, int>();
            var prevD = new Dictionary<Vertice, Vertice>();
            var qD = new List<Vertice>(D_L);

            // Inicializa distancias con un valor alto (infinito)
            foreach (var v in D_L) { distD[v] = 1000000; prevD[v] = null; }
            distD[vO] = 0;

            while (qD.Count > 0)
            {
                // Selecciona el nodo con la distancia minima acumulada
                var uD = qD.OrderBy(x => distD[x]).First();
                qD.Remove(uD);

                if (uD == vD) break;

                foreach (var aD in uD.D_AristasSalientes)
                {
                    int alt = distD[uD] + aD.D_Peso;
                    // Actualiza la distancia si se encuentra un camino mas corto
                    if (alt < distD[aD.D_Destino])
                    {
                        distD[aD.D_Destino] = alt;
                        prevD[aD.D_Destino] = uD;
                    }
                }
            }

            if (distD[vD] == 1000000) MessageBox.Show("No existe un camino entre " + vO.Nombre + " y " + vD.Nombre);
            else
            {
                // Reconstruye la ruta desde el destino hacia atras usando los previos
                var rD = new List<string>();
                var curr = vD;
                while (curr != null) { rD.Add(curr.Nombre); curr = prevD[curr]; }
                rD.Reverse();
                MessageBox.Show("Camino más corto (Dijkstra): " + string.Join(" -> ", rD) + "\nCosto: " + distD[vD]);
            }
            D_LimpiarTextBox();
        }

        private void MatrizAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            int n = D_L.Count;
            if (n == 0) 
                return;

            int[,] m = new int[n, n];
            // Llena la matriz colocando 1 donde existe una arista entre nodos
            for (int i = 0; i < n; i++)
                foreach (var a in D_L[i].D_AristasSalientes) m[i, D_L.IndexOf(a.D_Destino)] = 1;

            string s = "Matriz de Adyacencia:\n\n\t";
            for (int j = 0; j < n; j++) 
                    s += "V" + D_L[j].Nombre + "\t";
                    s += "\n\n";

            for (int i = 0; i < n; i++)
            {
                s += "V" + D_L[i].Nombre + "\t";
                for (int j = 0; j < n; j++) s += m[i, j] + "\t";
                s += "\n\n";
            }

            MessageBox.Show(s, "Representación del Grafo");
        }

        private void ListaAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            if (D_L.Count == 0) return;
            string s = "Lista de Adyacencia:\n";
            // Muestra cada nodo seguido de sus nodos vecinos conectados
            foreach (var v in D_L) s += v.Nombre + ": " + string.Join(", ", v.D_AristasSalientes.Select(a => a.D_Destino.Nombre)) + "\n";
            MessageBox.Show(s);
        }

        private void D_LimpiarTextBox()
        {
            txtOrigen.Clear();
            txtDestino.Clear();
        }

        private void D_RedibujarTodo()
        {
            canvasGrafo.Children.Clear();
            // Primero dibuja todas las aristas para que queden detras de los nodos
            foreach (var vD in D_L)
                foreach (var aD in vD.D_AristasSalientes)
                {
                    if (vD == aD.D_Destino) D_DibujarLazo(vD);
                    else D_DibujarLineaFlecha(vD.D_Pos, aD.D_Destino.D_Pos);
                }

            // Dibuja los circulos morados con el ID del nodo
            foreach (var vD in D_L)
            {
                Ellipse eD = new Ellipse {
                    Width = 30, 
                    Height = 30, 
                    Fill = Brushes.MediumPurple, Stroke = Brushes.White, StrokeThickness = 1 };
                Canvas.SetLeft(eD, vD.D_Pos.X - D_Radio); Canvas.SetTop(eD, vD.D_Pos.Y - D_Radio);
                canvasGrafo.Children.Add(eD);

                TextBlock tD = new TextBlock {
                    Text = vD.Nombre,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 12 };
                Canvas.SetLeft(tD, vD.D_Pos.X - 6); Canvas.SetTop(tD, vD.D_Pos.Y - 8);
                canvasGrafo.Children.Add(tD);
            }
            txtVertices.Text = D_L.Count.ToString();
            txtAristas.Text = D_L.Sum(x => x.D_AristasSalientes.Count).ToString();
        }

        private void D_DibujarLineaFlecha(Point pO, Point pD)
        {
            canvasGrafo.Children.Add(new Line { 
                X1 = pO.X,
                Y1 = pO.Y, X2 = pD.X,
                Y2 = pD.Y, Stroke = Brushes.White, StrokeThickness = 2 });

            // Calcula el angulo para posicionar la punta de la flecha
            double aD = Math.Atan2(
                pD.Y - pO.Y,
                pD.X - pO.X);
            Point pI = new Point(pD.X - D_Radio * Math.Cos(aD), pD.Y - D_Radio * Math.Sin(aD));

            Point p1 = new Point(pI.X - 10 * Math.Cos(aD + Math.PI / 6), pI.Y - 10 * Math.Sin(aD + Math.PI / 6));
            Point p2 = new Point(pI.X - 10 * Math.Cos(aD - Math.PI / 6), pI.Y - 10 * Math.Sin(aD - Math.PI / 6));

            canvasGrafo.Children.Add(new Polygon { Fill = Brushes.White, Points = new PointCollection { pI, p1, p2 } });
        }
        private void D_DibujarLazo(Vertice vD)
        {
            // Crea una curva Bezier para representar un lazo (arista al mismo nodo)
            Point pI = new Point(vD.D_Pos.X - 8, vD.D_Pos.Y - 13);
            Point pF = new Point(vD.D_Pos.X + 8, vD.D_Pos.Y - 13);
            Point pC1 = new Point(vD.D_Pos.X - 25, vD.D_Pos.Y - 45);
            Point pC2 = new Point(vD.D_Pos.X + 25, vD.D_Pos.Y - 45);

            PathGeometry gD = new PathGeometry();
            PathFigure fD = new PathFigure { StartPoint = pI };
            fD.Segments.Add(new BezierSegment { Point1 = pC1, Point2 = pC2, Point3 = pF });
            gD.Figures.Add(fD);

            canvasGrafo.Children.Add(new Path { Data = gD, Stroke = Brushes.White, StrokeThickness = 2 });

            // Dibuja la flecha del lazo en el punto final de la curva
            Point pPunta = new Point(pF.X - 2, pF.Y - 2);
            Point p1 = new Point(pPunta.X - 5, pPunta.Y - 10);
            Point p2 = new Point(pPunta.X + 10, pPunta.Y - 2);
            canvasGrafo.Children.Add(new Polygon { Fill = Brushes.White, Points = new PointCollection { pPunta, p1, p2 } });
        }
    }
}