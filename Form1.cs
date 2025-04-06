namespace Grey_Wolf_Optimizer
{
    public partial class Form1 : Form
    {
        // Algorithm settings
        const int PopulationSize = 20;
        const int MaxIterations = 100;
        const int CanvasSize = 500;
        const double RangeMin = -10;
        const double RangeMax = 10;

        List<double[]> wolves = new List<double[]>();
        double[] alpha = new double[2];
        double[] beta = new double[2];
        double[] delta = new double[2];
        Random rand = new Random();
        int iteration = 0;
        System.Windows.Forms.Timer timer;

        // UI elements for adjusting speed
        NumericUpDown numericUpDownSpeed;
        Label labelSpeed;

        public Form1()
        {
            InitializeComponents();
            // Set form size
            this.ClientSize = new Size(CanvasSize, CanvasSize + 50);

            // Initialize wolves
            InitializeWolves();

            // Setup timer (WinForms Timer)
            timer = new System.Windows.Forms.Timer();
            timer.Interval = (int)numericUpDownSpeed.Value; // initial interval
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // Initialization of UI components
        private void InitializeComponents()
        {
            this.SuspendLayout();
            // 
            // labelSpeed
            // 
            labelSpeed = new Label();
            labelSpeed.Location = new Point(10, CanvasSize + 10);
            labelSpeed.Size = new Size(150, 20);
            labelSpeed.Text = "Interval (ms):";
            // 
            // numericUpDownSpeed
            // 
            numericUpDownSpeed = new NumericUpDown();
            numericUpDownSpeed.Location = new Point(170, CanvasSize + 10);
            numericUpDownSpeed.Minimum = 10;
            numericUpDownSpeed.Maximum = 10000;
            numericUpDownSpeed.Value = 10000; // initial interval
            numericUpDownSpeed.Increment = 10;
            numericUpDownSpeed.ValueChanged += NumericUpDownSpeed_ValueChanged;
            // 
            // Form1
            // 
            this.Controls.Add(labelSpeed);
            this.Controls.Add(numericUpDownSpeed);
            this.Name = "Form1";
            this.Text = "Grey Wolf Optimizer Visualization";
            this.ResumeLayout(false);
        }

        // NumericUpDown change event handler to update timer interval
        private void NumericUpDownSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Interval = (int)numericUpDownSpeed.Value;
            }
        }

        // Initialize wolf positions
        void InitializeWolves()
        {
            wolves.Clear();
            for (int i = 0; i < PopulationSize; i++)
            {
                wolves.Add(new double[]
                {
                    RangeMin + rand.NextDouble() * (RangeMax - RangeMin),
                    RangeMin + rand.NextDouble() * (RangeMax - RangeMin)
                });
            }
        }

        // Timer updates algorithm logic and redraws the form
        void Timer_Tick(object sender, EventArgs e)
        {
            if (iteration >= MaxIterations)
            {
                timer.Stop();
                return;
            }

            UpdateWolves();
            this.Invalidate(); // call OnPaint for redrawing
            iteration++;
        }

        // Update wolves' positions according to the GWO algorithm
        void UpdateWolves()
        {
            // Sort wolves by objective function value
            var sorted = wolves.OrderBy(w => ObjectiveFunction(w)).ToList();
            Array.Copy(sorted[0], alpha, 2);
            Array.Copy(sorted[1], beta, 2);
            Array.Copy(sorted[2], delta, 2);

            double a = 2.0 - iteration * (2.0 / MaxIterations);

            for (int i = 0; i < PopulationSize; i++)
            {
                double[] newPos = new double[2];
                for (int d = 0; d < 2; d++)
                {
                    double r1 = rand.NextDouble();
                    double r2 = rand.NextDouble();
                    double A1 = 2 * a * r1 - a;
                    double C1 = 2 * r2;
                    double D_alpha = Math.Abs(C1 * alpha[d] - wolves[i][d]);
                    double X1 = alpha[d] - A1 * D_alpha;

                    r1 = rand.NextDouble();
                    r2 = rand.NextDouble();
                    double A2 = 2 * a * r1 - a;
                    double C2 = 2 * r2;
                    double D_beta = Math.Abs(C2 * beta[d] - wolves[i][d]);
                    double X2 = beta[d] - A2 * D_beta;

                    r1 = rand.NextDouble();
                    r2 = rand.NextDouble();
                    double A3 = 2 * a * r1 - a;
                    double C3 = 2 * r2;
                    double D_delta = Math.Abs(C3 * delta[d] - wolves[i][d]);
                    double X3 = delta[d] - A3 * D_delta;

                    newPos[d] = (X1 + X2 + X3) / 3.0;

                    // Clamp position within allowed range
                    newPos[d] = Math.Max(RangeMin, Math.Min(RangeMax, newPos[d]));
                }
                wolves[i] = newPos;
            }
        }

        // Objective function (sphere function)
        double ObjectiveFunction(double[] pos)
        {
            return pos[0] * pos[0] + pos[1] * pos[1];
        }

        // Drawing logic (override OnPaint)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Background
            g.Clear(Color.White);

            // Draw all wolves
            foreach (var w in wolves)
                DrawPoint(g, w, Brushes.Gray);

            // Highlight top 3 wolves
            DrawPoint(g, alpha, Brushes.Red);     // Alpha
            DrawPoint(g, beta, Brushes.Orange);   // Beta
            DrawPoint(g, delta, Brushes.Green);   // Delta

            // Optimal point (minimum)
            DrawPoint(g, new double[] { 0, 0 }, Brushes.Blue, 6);

            // Display iteration info
            g.DrawString($"Iteration: {iteration}", new Font("Arial", 12), Brushes.Black, 10, 10);
        }

        // Helper method to draw a point by coordinates
        void DrawPoint(Graphics g, double[] pos, Brush brush, int size = 4)
        {
            // Scale coordinates to canvas size
            int x = (int)((pos[0] - RangeMin) / (RangeMax - RangeMin) * CanvasSize);
            int y = (int)((pos[1] - RangeMin) / (RangeMax - RangeMin) * CanvasSize);
            // Invert Y coordinate because Y increases downward in WinForms
            g.FillEllipse(brush, x - size / 2, CanvasSize - y - size / 2, size, size);
        }
    }
}
