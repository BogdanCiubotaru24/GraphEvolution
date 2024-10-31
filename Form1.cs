using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace PrimulMeuCursDeGrafica
{
    public partial class Form1 : Form
    {
        private bool isRunning = false;
        private List<(int, int, double)> edges;
        private List<List<(double, double)>> population;
        private int numberOfNodes;
        private int populationSize = 20;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cod pentru inițializare suplimentară, dacă este necesar
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                // Stop evolution
                evolutionTimer.Stop();
                button1.Text = "Start";
                isRunning = false;
            }
            else
            {
                // Start evolution
                string filename = "coordonate.txt";
                ReadGraphData(filename);

                // Inițializăm populația cu poziții aleatoare pentru fiecare nod
                Random random = new Random();
                population = new List<List<(double, double)>>();

                for (int i = 0; i < populationSize; i++)
                {
                    var coordinates = new List<(double, double)>();
                    for (int j = 0; j < numberOfNodes; j++)
                    {
                        double x = random.NextDouble() * 100;
                        double y = random.NextDouble() * 100;
                        coordinates.Add((x, y));
                    }
                    population.Add(coordinates);
                }

                evolutionTimer.Start();
                button1.Text = "Stop";
                isRunning = true;
            }
        }

        private void EvolutionTimer_Tick(object sender, EventArgs e)
        {
            // Evoluăm populația pentru o generație
            population = EvolvePopulationStep(population);
            double fitnessValue = Fitness(population[0]);
            textBox1.Text = $"Evoluție în progres... Fitness: {fitnessValue}";

            // Plot the graph
            PlotGraph(population[0]);
        }

        public void ReadGraphData(string filename)
        {
            string? directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = directoryPath != null ? Path.Combine(directoryPath, filename) : filename;

            // Asigurăm fișierul cu datele corecte
            CreateCorrectGraphFile(filePath);

            // Citim datele din fișier
            var lines = File.ReadLines(filePath).ToList();

            try
            {
                // Prima linie este numărul de noduri
                numberOfNodes = int.Parse(lines[0].Trim());

                // Restul liniilor sunt muchiile și ponderile
                edges = lines.Skip(1)
                             .Select(line => line.Split(' '))
                             .Select(parts => (int.Parse(parts[0]), int.Parse(parts[1]), double.Parse(parts[2])))
                             .ToList();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Eroare la citirea fișierului: {ex.Message}");
                throw;
            }
        }

        public void CreateCorrectGraphFile(string filePath)
        {
            // Dacă fișierul nu există sau conținutul este incorect, îl suprascriem cu valorile corecte
            bool createNewFile = false;

            if (!File.Exists(filePath))
            {
                createNewFile = true;
            }
            else
            {
                // Verificăm dacă fișierul are un conținut corespunzător
                var lines = File.ReadAllLines(filePath);
                if (lines.Length == 0 || !int.TryParse(lines[0].Trim(), out int parsedNodes))
                {
                    createNewFile = true;
                }
            }

            if (createNewFile)
            {
                using (var writer = new StreamWriter(filePath, false)) // False la al doilea parametru pentru a suprascrie fișierul
                {
                    writer.WriteLine("6"); // Numărul de noduri
                    writer.WriteLine("0 1 20");
                    writer.WriteLine("0 2 20");
                    writer.WriteLine("1 2 20");
                    writer.WriteLine("2 3 25");
                    writer.WriteLine("2 5 45");
                    writer.WriteLine("3 4 20");
                    writer.WriteLine("4 5 15");
                }
            }
        }

        public static double CalculateDistance((double, double) point1, (double, double) point2)
        {
            return Math.Sqrt(Math.Pow(point1.Item1 - point2.Item1, 2) + Math.Pow(point1.Item2 - point2.Item2, 2));
        }

        public double Fitness(List<(double, double)> nodes)
        {
            double totalDeviation = 0;
            foreach (var (start, end, targetDistance) in edges)
            {
                double actualDistance = CalculateDistance(nodes[start], nodes[end]);
                totalDeviation += Math.Pow(actualDistance - targetDistance, 2);
            }

            // Cu cât deviația totală este mai mică, cu atât fitness-ul este mai mare
            return -totalDeviation;
        }

        public List<List<(double, double)>> EvolvePopulationStep(List<List<(double, double)>> population)
        {
            var random = new Random();
            var fitnessScores = population.Select(graph => Fitness(graph)).ToList();
            var sortedPopulation = population.Zip(fitnessScores, (graph, score) => new { graph, score })
                                             .OrderByDescending(x => x.score)
                                             .Select(x => x.graph)
                                             .ToList();

            // Selectăm cei mai buni indivizi (elită) pentru a forma baza noii generații
            var nextGeneration = sortedPopulation.Take(population.Count / 2).ToList();

            // Generăm restul populației prin crossover și mutație
            while (nextGeneration.Count < population.Count)
            {
                var parent1 = nextGeneration[random.Next(nextGeneration.Count)];
                var parent2 = nextGeneration[random.Next(nextGeneration.Count)];
                var child = Crossover(parent1, parent2);
                nextGeneration.Add(Mutate(child, mutationRate: 0.1));
            }

            return nextGeneration;
        }

        public List<(double, double)> Crossover(List<(double, double)> parent1, List<(double, double)> parent2)
        {
            var child = new List<(double, double)>();
            var random = new Random();

            // Utilizăm un crossover bazat pe medie pentru a menține punctele cât mai departe
            for (int i = 0; i < parent1.Count; i++)
            {
                double x = (parent1[i].Item1 + parent2[i].Item1) / 2;
                double y = (parent1[i].Item2 + parent2[i].Item2) / 2;
                child.Add((x, y));
            }

            return child;
        }

        public List<(double, double)> Mutate(List<(double, double)> graph, double mutationRate = 0.1)
        {
            var random = new Random();
            for (int i = 0; i < graph.Count; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    graph[i] = (graph[i].Item1 + random.NextDouble() * 2 - 1, graph[i].Item2 + random.NextDouble() * 2 - 1);
                }
            }
            return graph;
        }

        public void PlotGraph(List<(double, double)> nodes)
        {
            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Pen nodePen = new Pen(Color.Blue, 3);
                Pen edgePen = new Pen(Color.Gray, 1);

                // Draw edges
                foreach (var (start, end, _) in edges)
                {
                    g.DrawLine(edgePen, (float)nodes[start].Item1 * 5, (float)nodes[start].Item2 * 5,
                                      (float)nodes[end].Item1 * 5, (float)nodes[end].Item2 * 5);
                }

                // Draw nodes
                foreach (var point in nodes)
                {
                    g.DrawEllipse(nodePen, (float)point.Item1 * 5, (float)point.Item2 * 5, 5, 5);
                }
            }
            pictureBox1.Image = bitmap;
        }
    }
}
