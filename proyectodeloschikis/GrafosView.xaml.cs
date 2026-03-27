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
using System.Data;

namespace proyectodeloschikis
{
    // Clase para las conexiones de los nodos
    public class Arista
    {
        public Vertice vDestino { get; set; }
        public int pPeso { get; set; } = 1;
        public Arista(Vertice d) { vDestino = d; }
    }

    // Clase para los circulos o nodos
    public class Vertice
    {
        public string nNombre { get; set; }
        public Point pPosicion { get; set; }
        public List<Arista> lAristas { get; set; } = new List<Arista>();
        public Vertice(string n, Point p) { nNombre = n; pPosicion = p; }
    }

    public partial class GrafosView : UserControl
    {
        // Variables para arrastrar nodos con el mouse
        private Vertice selectedVertice = null;
        private bool isDragging = false;
        private Point lastMousePos;

        // Variables globales del grafo
        private List<Vertice> listaVertices = new List<Vertice>();
        private int contadorId = 1;
        private const double radioNodo = 15;

        public GrafosView()
        {
            InitializeComponent();
        }

        // Evento para crear un nuevo nodo en el dibujo
        private void NuevoVertice_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                // Si está vacío usamos el contador, si no validar y normalizar
                string nombre;
                if (string.IsNullOrWhiteSpace(D_txtNuevoNodo.Text))
                {
                    nombre = contadorId.ToString();
                }
                else
                {
                    if (!validacion.EsAlfaNumerico(D_txtNuevoNodo.Text, out nombre, 20))
                    {
                        D_txtNuevoNodo.Clear();
                        return;
                    }
                }

                // Revisa que el nombre del nodo no este repetido (insensible a mayúsculas)
                if (listaVertices.Any(v => string.Equals(v.nNombre?.Trim(), nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($" Nombre duplicado ('{nombre}'). No se permiten nodos con el mismo nombre.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    D_txtNuevoNodo.Clear();
                    return;
                }

                listaVertices.Add(new Vertice(nombre, new Point(0, 0)));
                if (string.IsNullOrWhiteSpace(D_txtNuevoNodo.Text)) contadorId++;

                // Ordena y dibuja de nuevo
                AcomodarNodosEnCirculo();
                DibujarGrafoCompleto();

                D_txtNuevoNodo.Clear();
            });
        }

        // Evento para conectar dos nodos existentes
        private void NuevaArista_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsTextoValido(D_txtO.Text, out string origen) || !validacion.EsTextoValido(D_txtD.Text, out string destino))
                {
                    LimpiarEntradasTexto();
                    return;
                }

                var vO = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), origen, StringComparison.OrdinalIgnoreCase));
                var vD = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), destino, StringComparison.OrdinalIgnoreCase));

                if (vO == null)
                {
                    MessageBox.Show($" Nodo origen '{origen}' no encontrado. Revisa el nombre ingresado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LimpiarEntradasTexto();
                    return;
                }
                if (vD == null)
                {
                    MessageBox.Show($" Nodo destino '{destino}' no encontrado. Revisa el nombre ingresado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LimpiarEntradasTexto();
                    return;
                }
                if (vO == vD)
                {
                    MessageBox.Show($" Origen y destino son el mismo nodo ('{origen}'). No se crea la arista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LimpiarEntradasTexto();
                    return;
                }

                if (!vO.lAristas.Any(a => a.vDestino == vD))
                {
                    vO.lAristas.Add(new Arista(vD));
                    DibujarGrafoCompleto();
                }

                LimpiarEntradasTexto();
            });
        }

        // Evento para borrar un nodo especifico
        private void EliminarNodo_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsTextoValido(D_txtNuevoNodo.Text, out string nombre))
                {
                    D_txtNuevoNodo.Clear();
                    return;
                }

                var vE = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), nombre, StringComparison.OrdinalIgnoreCase));
                if (vE == null)
                {
                    MessageBox.Show($" Nodo '{nombre}' no encontrado. Asegúrate de que el nombre sea correcto.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    D_txtNuevoNodo.Clear();
                    return;
                }

                // Borra el nodo y tambien las flechas que apuntaban a el
                listaVertices.Remove(vE);
                foreach (var vT in listaVertices) vT.lAristas.RemoveAll(a => a.vDestino == vE);

                AcomodarNodosEnCirculo();
                DibujarGrafoCompleto();

                D_txtNuevoNodo.Clear();
            });
        }

        // Evento para crear un lazo 
        private void AgregarLazo_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsTextoValido(D_txtO.Text, out string origen)) { LimpiarEntradasTexto(); return; }

                var vN = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), origen, StringComparison.OrdinalIgnoreCase));
                if (vN == null)
                {
                    MessageBox.Show($" Nodo '{origen}' no encontrado. No se puede agregar un lazo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LimpiarEntradasTexto();
                    return;
                }

                if (!vN.lAristas.Any(a => a.vDestino == vN))
                {
                    vN.lAristas.Add(new Arista(vN));
                    DibujarGrafoCompleto();
                }

                LimpiarEntradasTexto();
            });
        }

        // Limpia toda la pantalla y resetea datos
        private void VaciarGrafo_Click(object sender, RoutedEventArgs e)
        {
            listaVertices.Clear();
            contadorId = 1;
            D_txtV.Text = "0";
            D_txtA.Text = "0";
            canvasGrafo.Children.Clear();
        }

        // Recorrido BFS usando una cola
        private void BFS_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsTextoValido(D_txtO.Text, out string origen)) return;
                var vI = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), origen, StringComparison.OrdinalIgnoreCase));
                if (vI == null) { MessageBox.Show($" Nodo origen '{origen}' no encontrado. No se puede realizar BFS.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            var vis = new List<Vertice>();
            var col = new Queue<Vertice>();
            col.Enqueue(vI);
            vis.Add(vI);

            while (col.Count > 0)
            {
                var vA = col.Dequeue();
                foreach (var a in vA.lAristas)
                {
                    if (!vis.Contains(a.vDestino)) { vis.Add(a.vDestino); col.Enqueue(a.vDestino); }
                }
            }
            MessageBox.Show("Ruta BFS: " + string.Join(" -> ", vis.Select(v => v.nNombre)));
            });
        }

        // Algoritmo Dijkstra para buscar costos
        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsTextoValido(D_txtO.Text, out string origen) || !validacion.EsTextoValido(D_txtD.Text, out string destino)) return;
                var vO = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), origen, StringComparison.OrdinalIgnoreCase));
                var vD = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), destino, StringComparison.OrdinalIgnoreCase));
                if (vO == null || vD == null)
                {
                    MessageBox.Show($" Origen ('{origen}') o destino ('{destino}') no encontrados. Verifica los nombres.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

            var dist = new Dictionary<Vertice, int>();
            var lP = new List<Vertice>(listaVertices);

            // Inicializa distancias en un numero muy alto
            foreach (var v in listaVertices) dist[v] = 99999;
            dist[vO] = 0;

            while (lP.Count > 0)
            {
                var vA = lP.OrderBy(v => dist[v]).First();
                lP.Remove(vA);
                if (vA == vD) break;

                foreach (var a in vA.lAristas)
                {
                    int nD = dist[vA] + a.pPeso;
                    if (nD < dist[a.vDestino]) dist[a.vDestino] = nD;
                }
            }
            MessageBox.Show("Costo total Dijkstra: " + dist[vD]);
            });
        }

        // Mostrar matriz de adyacencia en una ventana
        private void MatrizAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (listaVertices.Count == 0)
                {
                    MessageBox.Show("El grafo está vacío. No hay nodos para mostrar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                int n = listaVertices.Count;
                var dt = new DataTable();
                dt.Columns.Add("Nodo", typeof(string));
                // columnas con nombres de nodos
                foreach (var v in listaVertices) dt.Columns.Add(v.nNombre, typeof(int));

                for (int i = 0; i < n; i++)
                {
                    var row = dt.NewRow();
                    row["Nodo"] = listaVertices[i].nNombre;
                    for (int j = 0; j < n; j++)
                    {
                        // encuentra si existe arista de i a j
                        int val = 0;
                        var a = listaVertices[i].lAristas.FirstOrDefault(ar => string.Equals(ar.vDestino.nNombre, listaVertices[j].nNombre, StringComparison.OrdinalIgnoreCase));
                        if (a != null) val = a.pPeso;
                        row[listaVertices[j].nNombre] = val;
                    }
                    dt.Rows.Add(row);
                }

                var win = new MatrizAdyacenciaWindow();
                win.matrizGrid.ItemsSource = dt.DefaultView;
                win.Show();
            });
        }

        // Mostrar lista de adyacencia en una ventana
        private void ListaAdyacencia_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (listaVertices.Count == 0)
                {
                    MessageBox.Show("El grafo está vacío. No hay nodos para mostrar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var datos = new List<(string Nodo, string Vecinos)>();
                foreach (var v in listaVertices)
                {
                    string vecinos = v.lAristas.Count == 0 ? string.Empty : string.Join(", ", v.lAristas.Select(a => a.vDestino.nNombre));
                    datos.Add((v.nNombre, vecinos));
                }

                // Preferir la ventana que tiene el DataGrid si existe
                var win = new ListaAdyacenciaWindow();
                // Formatea cada entrada como Nodo
                win.Cargar(datos.Select(d => string.IsNullOrEmpty(d.Vecinos) ? d.Nodo : $"{d.Nodo}: {d.Vecinos}").ToList());
                win.Show();
            });
        }

        // Metodo para que los nodos no queden uno encima de otro
        private void AcomodarNodosEnCirculo()
        {
            if (listaVertices.Count == 0) return;
            double width = canvasGrafo.ActualWidth > 0 ? canvasGrafo.ActualWidth : (double.IsNaN(canvasGrafo.Width) || canvasGrafo.Width == 0 ? 800 : canvasGrafo.Width);
            double height = canvasGrafo.ActualHeight > 0 ? canvasGrafo.ActualHeight : (double.IsNaN(canvasGrafo.Height) || canvasGrafo.Height == 0 ? 600 : canvasGrafo.Height);
            double cX = width / 2;
            double cY = height / 2;
            double rC = 180; // Radio del circulo imaginario
            double aP = 2 * Math.PI / listaVertices.Count;

            for (int i = 0; i < listaVertices.Count; i++)
            {
                double x = cX + rC * Math.Cos(i * aP);
                double y = cY + rC * Math.Sin(i * aP);
                listaVertices[i].pPosicion = new Point(x, y);
            }
        }

        // Dibuja todo el contenido del canvas
        private void DibujarGrafoCompleto()
        {
            canvasGrafo.Children.Clear();

            // Primero dibuja todas las aristas
            foreach (var v in listaVertices)
            {
                foreach (var a in v.lAristas)
                {
                    if (v == a.vDestino) DibujarAristaLazo(v);
                    else DibujarLineaConFlecha(v.pPosicion, a.vDestino.pPosicion);
                }
            }

            // dibuja los  (nodos) encima
            foreach (var v in listaVertices)
            {
                Ellipse eN = new Ellipse { 
                    Width = 30,
                    Height = 30,
                    Fill = Brushes.MediumPurple,
                    Stroke = Brushes.White,
                    StrokeThickness = 1 };
                Canvas.SetLeft(eN, v.pPosicion.X - radioNodo);
                Canvas.SetTop(eN, v.pPosicion.Y - radioNodo);
                canvasGrafo.Children.Add(eN);

                // Nombre del nodo

                TextBlock tN = new TextBlock { 
                    Text = v.nNombre,
                    Foreground = Brushes.White, 
                    FontWeight = FontWeights.Bold };
                Canvas.SetLeft(tN, v.pPosicion.X - 6);
                Canvas.SetTop(tN, v.pPosicion.Y - 8);
                canvasGrafo.Children.Add(tN);
            }

            // Actualiza los contadores de la interfaz
            D_txtV.Text = listaVertices.Count.ToString();
            D_txtA.Text = listaVertices.Sum(v => v.lAristas.Count).ToString();
        }

        // Crea la linea y la punta de la flecha
        private void DibujarLineaConFlecha(Point pI, Point pF)
        {
            canvasGrafo.Children.Add(new Line { 
                X1 = pI.X,
                Y1 = pI.Y, 
                X2 = pF.X, 
                Y2 = pF.Y, Stroke = Brushes.White, StrokeThickness = 2 });

            // Calculo para poner la flecha al final de la linea
            double ang = Math.Atan2(pF.Y - pI.Y, pF.X - pI.X);
            Point pP = new Point(pF.X - radioNodo * Math.Cos(ang), pF.Y - radioNodo * Math.Sin(ang));
            Point p1 = new Point(pP.X - 10 * Math.Cos(ang + Math.PI / 6), pP.Y - 10 * Math.Sin(ang + Math.PI / 6));
            Point p2 = new Point(pP.X - 10 * Math.Cos(ang - Math.PI / 6), pP.Y - 10 * Math.Sin(ang - Math.PI / 6));

            canvasGrafo.Children.Add(new Polygon { Fill = Brushes.White, Points = new PointCollection { pP, p1, p2 } });
        }

        // Crea la curva para el lazo
        private void DibujarAristaLazo(Vertice v)
        {
            Point pI = new Point(v.pPosicion.X - 8, v.pPosicion.Y - 13);
            Point pF = new Point(v.pPosicion.X + 8, v.pPosicion.Y - 13);

            PathFigure pf = new PathFigure { StartPoint = pI };
            pf.Segments.Add(new BezierSegment
            {
                Point1 = new Point(v.pPosicion.X - 25, v.pPosicion.Y - 45),
                Point2 = new Point(v.pPosicion.X + 25, v.pPosicion.Y - 45),
                Point3 = pF
            });

            canvasGrafo.Children.Add(new Path { Data = new PathGeometry { Figures = { pf } }, Stroke = Brushes.White, StrokeThickness = 2 });

            // Flecha del lazo
            canvasGrafo.Children.Add(new Polygon { Fill = Brushes.White, Points = new PointCollection { pF, new Point(pF.X - 5, pF.Y - 10), new Point(pF.X + 10, pF.Y - 2) } });
        }

        private void LimpiarEntradasTexto() { D_txtO.Clear(); D_txtD.Clear(); D_txtNuevoNodo.Clear(); }

        // Funcion DFS

        private void DFS_Click(object sender, RoutedEventArgs e)
        {
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.EsTextoValido(D_txtO.Text, out string origen)) return;
                var start = listaVertices.FirstOrDefault(v => string.Equals(v.nNombre?.Trim(), origen, StringComparison.OrdinalIgnoreCase));
                if (start == null) { MessageBox.Show($"Nodo origen '{origen}' no encontrado.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

                var visited = new HashSet<Vertice>();
                var orden = new List<Vertice>();

                void Recur(Vertice v)
                {
                    visited.Add(v);
                    orden.Add(v);
                    foreach (var a in v.lAristas)
                    {
                        if (!visited.Contains(a.vDestino)) Recur(a.vDestino);
                    }
                }

                Recur(start);
                MessageBox.Show("Ruta DFS: " + string.Join(" -> ", orden.Select(v => v.nNombre)));
            });
        }

        // Eventos de mouse para seleccionar y arrastrar nodos
        private void CanvasGrafo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // selección por cercanía al centro del nodo
            var p = e.GetPosition(canvasGrafo);
            selectedVertice = listaVertices.FirstOrDefault(v => Math.Sqrt(Math.Pow(v.pPosicion.X - p.X, 2) + Math.Pow(v.pPosicion.Y - p.Y, 2)) <= radioNodo * 1.5);
            if (selectedVertice != null)
            {
                isDragging = true;
                lastMousePos = p;
                canvasGrafo.CaptureMouse();
            }
        }

        private void CanvasGrafo_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || selectedVertice == null) return;
            var p = e.GetPosition(canvasGrafo);
            // mueve directamente al puntero
            selectedVertice.pPosicion = p;
            DibujarGrafoCompleto();
            lastMousePos = p;
        }

        private void CanvasGrafo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isDragging) return;
            isDragging = false;
            selectedVertice = null;
            canvasGrafo.ReleaseMouseCapture();
        }
    }
}