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
            validacion.EjecutarSeguro(() =>
            {
                if (!validacion.NoVacio(txtPantalla.Text)) return;

                string exp = txtPantalla.Text.Trim();

                // 🔴 Validación básica de expresión
                if (!ExpresionValida(exp))
                {
                    txtPantalla.Text = "Error";
                    return;
                }

                string dummy;
                int res = EvaluarExpresionPostFija(exp, out dummy);
                txtPantalla.Text = res.ToString();
            });
        }
        private string ProcesarMultiplicacionImplicita(string exp)
        {
            string resultado = "";

            for (int i = 0; i < exp.Length; i++)
            {
                char actual = exp[i];
                resultado += actual;

                if (i < exp.Length - 1)
                {
                    char siguiente = exp[i + 1];

                    // casos: )(
                    if (actual == ')' && siguiente == '(')
                        resultado += "*";

                    // casos: número(
                    else if (char.IsDigit(actual) && siguiente == '(')
                        resultado += "*";

                    // casos: )número
                    else if (actual == ')' && char.IsDigit(siguiente))
                        resultado += "*";
                }
            }

            return resultado;
        }
        private bool ExpresionValida(string exp)
        {
            int balance = 0;
            char prev = '\0';

            for (int i = 0; i < exp.Length; i++)
            {
                char c = exp[i];

                if (!(char.IsDigit(c) || "+-*/()".Contains(c)))
                    return false;

                // balance de paréntesis
                if (c == '(') balance++;
                if (c == ')') balance--;

                if (balance < 0) return false;

                //  operador después de operador
                if ("+-*/".Contains(c) && "+-*/".Contains(prev))
                    return false;

                //  empieza con operador (excepto - opcional si quieres)
                if (i == 0 && "+*/".Contains(c))
                    return false;

                //  operador antes de cerrar paréntesis
                if (c == ')' && "+-*/".Contains(prev))
                    return false;

                prev = c;
            }

            //  termina en operador
            if ("+-*/".Contains(prev))
                return false;

            return balance == 0;
        }


        // --- Logica de Pilas del Proceso

        private int CalcularOperacion(int x, int y, string op)
        {
            switch (op)
            {
                case "+": return x + y;
                case "-": return x - y;
                case "*": return x * y;
                case "/":
                    if (y == 0)
                        throw new Exception("No se puede dividir entre 0");

                    return x / y;
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
            infija = ProcesarMultiplicacionImplicita(infija);
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
                    if (pila.Count < 2)
                    {
                        validacion.Error("Expresión inválida");
                        return 0;
                    }

                    int v2 = int.Parse(pila.Pop());
                    int v1 = int.Parse(pila.Pop());
                    pila.Push(CalcularOperacion(v1, v2, t).ToString());
                }
            }
            if (pila.Count != 1)
            {
                validacion.Error("Expresión mal formada");
                return 0;
            }

            return int.Parse(pila.Pop());
        }
    }
}