namespace PrimulMeuCursDeGrafica
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBox1;
        private Button button1;
        private TextBox textBox1;
        private System.Windows.Forms.Timer evolutionTimer; // Am specificat explicit System.Windows.Forms.Timer

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            textBox1 = new TextBox();
            evolutionTimer = new System.Windows.Forms.Timer(components); // Am specificat explicit System.Windows.Forms.Timer
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(10, 2);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(760, 500);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.BackColor = Color.White;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            // 
            // button1
            // 
            button1.Location = new Point(12, 520);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(100, 30);
            button1.TabIndex = 1;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(this.Button1_Click);
            // 
            // textBox1
            // 
            textBox1.Location = new Point(120, 520);
            textBox1.Margin = new Padding(3, 2, 3, 2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(650, 30);
            textBox1.TabIndex = 2;
            // 
            // evolutionTimer
            // 
            evolutionTimer.Interval = 1; // Intervalul este de 1 secundă
            evolutionTimer.Tick += new EventHandler(this.EvolutionTimer_Tick);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Evoluția Graf-ului";
            Load += new EventHandler(Form1_Load);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
