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
    public partial class CalculadoraView : UserControl
    {
        private Stack<string> pila = new Stack<string>();
        private List<string> operadores = new List<string> { "*", "/", "+", "-" };

        public CalculadoraView()
        {
            InitializeComponent();
        }

        // --- MÉTODOS DE INTERFAZ (EVENTOS) ---

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string valor = btn.Content.ToString();

            if (txtPantalla.Text == "0" || txtPantalla.Text == "Error")
                txtPantalla.Text = valor;
            else
                txtPantalla.Text += valor;
        }

        private void LimpiarTodo_Click(object sender, RoutedEventArgs e)
        {
            txtPantalla.Text = "0";
        }

        private void BorrarUno_Click(object sender, RoutedEventArgs e)
        {
            if (txtPantalla.Text.Length > 0 && txtPantalla.Text != "0")
            {
                txtPantalla.Text = txtPantalla.Text.Substring(0, txtPantalla.Text.Length - 1);
            }
            if (txtPantalla.Text == "") txtPantalla.Text = "0";
        }

        private void Resultado_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dummy;
                int res = EvaluarExpresionPostFija(txtPantalla.Text, out dummy);
                txtPantalla.Text = res.ToString();
            }
            catch
            {
                //  ENCARGADO DE Captura errores si la expresión matemática está mal formada
                txtPantalla.Text = "Error";
            }
        }

        // --- Logica de Pilas del Proceso

        private int CalcularOperacion(int x, int y, string op)
        {
            switch (op)
            {
                case "+": return x + y;
                case "-": return x - y;
                case "*": return x * y;
                case "/": return y != 0 ? x / y : 0; // si hay alguina divicion entre 0 retorna
                default: return 0;
            }
        }
        private int Precedencia(string op)
        {
            if (op == "*" || op == "/") return 1;
            if (op == "+" || op == "-") return 2;
            return 3;
        }
        private List<string> StringToList(string exp)
        {
            var lista = new List<string>();
            string num = "";
            foreach (char c in exp)
            {
                if (char.IsDigit(c)) num += c;
                else
                {
                    if (num != "") { lista.Add(num); num = ""; }
                    if (operadores.Contains(c.ToString()) || c == '(' || c == ')') lista.Add(c.ToString());
                }
            }
            if (num != "") lista.Add(num);
            return lista;
        }
        private string InFijaToPostFija(string infija)
        {
            string post = "";
            pila.Clear();
            var tokens = StringToList(infija);

            foreach (var t in tokens)
            {
                int n;
                if (int.TryParse(t, out n)) post += t + " ";
                else if (t == "(") pila.Push(t);
                else if (t == ")")
                {
                    while (pila.Count > 0 && pila.Peek() != "(") post += pila.Pop() + " ";
                    if (pila.Count > 0) pila.Pop();
                }
                else
                {
                    while (pila.Count > 0 && pila.Peek() != "(" && Precedencia(pila.Peek()) <= Precedencia(t))
                        post += pila.Pop() + " ";
                    pila.Push(t);
                }
            }
            while (pila.Count > 0) post += pila.Pop() + " ";
            return post.Trim();
        }
        public int EvaluarExpresionPostFija(string infija, out string postfija)
        {
            postfija = InFijaToPostFija(infija);
            var tokens = postfija.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            pila.Clear();

            foreach (var t in tokens)
            {
                int n;
                if (int.TryParse(t, out n)) pila.Push(t);
                else
                {
                    int v2 = int.Parse(pila.Pop());
                    int v1 = int.Parse(pila.Pop());
                    pila.Push(CalcularOperacion(v1, v2, t).ToString());
                }
            }
            return int.Parse(pila.Pop());
        }
    }
}