
namespace CannyEdgeDetection
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ButInput = new System.Windows.Forms.Button();
            this.ButGaus = new System.Windows.Forms.Button();
            this.ButSobel = new System.Windows.Forms.Button();
            this.ButSuppression = new System.Windows.Forms.Button();
            this.ButThreshold = new System.Windows.Forms.Button();
            this.ButHysteresys = new System.Windows.Forms.Button();
            this.BoxInput = new Emgu.CV.UI.ImageBox();
            this.BoxProcess = new Emgu.CV.UI.ImageBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtSize = new System.Windows.Forms.TextBox();
            this.TxtSigma = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BoxInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // ButInput
            // 
            this.ButInput.Location = new System.Drawing.Point(0, -1);
            this.ButInput.Name = "ButInput";
            this.ButInput.Size = new System.Drawing.Size(94, 29);
            this.ButInput.TabIndex = 0;
            this.ButInput.Text = "Files";
            this.ButInput.UseVisualStyleBackColor = true;
            this.ButInput.Click += new System.EventHandler(this.ButInput_Click);
            // 
            // ButGaus
            // 
            this.ButGaus.Location = new System.Drawing.Point(90, -1);
            this.ButGaus.Name = "ButGaus";
            this.ButGaus.Size = new System.Drawing.Size(94, 29);
            this.ButGaus.TabIndex = 1;
            this.ButGaus.Text = "Gaussian";
            this.ButGaus.UseVisualStyleBackColor = true;
            this.ButGaus.Click += new System.EventHandler(this.ButGaus_Click);
            // 
            // ButSobel
            // 
            this.ButSobel.Location = new System.Drawing.Point(179, -1);
            this.ButSobel.Name = "ButSobel";
            this.ButSobel.Size = new System.Drawing.Size(94, 29);
            this.ButSobel.TabIndex = 2;
            this.ButSobel.Text = "Sobel";
            this.ButSobel.UseVisualStyleBackColor = true;
            this.ButSobel.Click += new System.EventHandler(this.ButSobel_Click);
            // 
            // ButSuppression
            // 
            this.ButSuppression.Location = new System.Drawing.Point(268, -1);
            this.ButSuppression.Name = "ButSuppression";
            this.ButSuppression.Size = new System.Drawing.Size(104, 29);
            this.ButSuppression.TabIndex = 3;
            this.ButSuppression.Text = "Suppression";
            this.ButSuppression.UseVisualStyleBackColor = true;
            this.ButSuppression.Click += new System.EventHandler(this.ButSuppression_Click);
            // 
            // ButThreshold
            // 
            this.ButThreshold.Location = new System.Drawing.Point(368, -1);
            this.ButThreshold.Name = "ButThreshold";
            this.ButThreshold.Size = new System.Drawing.Size(94, 29);
            this.ButThreshold.TabIndex = 4;
            this.ButThreshold.Text = "Threshold";
            this.ButThreshold.UseVisualStyleBackColor = true;
            this.ButThreshold.Click += new System.EventHandler(this.ButThreshold_Click);
            // 
            // ButHysteresys
            // 
            this.ButHysteresys.Location = new System.Drawing.Point(459, -1);
            this.ButHysteresys.Name = "ButHysteresys";
            this.ButHysteresys.Size = new System.Drawing.Size(94, 29);
            this.ButHysteresys.TabIndex = 5;
            this.ButHysteresys.Text = "Hysteresys";
            this.ButHysteresys.UseVisualStyleBackColor = true;
            this.ButHysteresys.Click += new System.EventHandler(this.ButHysteresys_Click);
            // 
            // BoxInput
            // 
            this.BoxInput.Location = new System.Drawing.Point(0, 34);
            this.BoxInput.Name = "BoxInput";
            this.BoxInput.Size = new System.Drawing.Size(713, 604);
            this.BoxInput.TabIndex = 2;
            this.BoxInput.TabStop = false;
            // 
            // BoxProcess
            // 
            this.BoxProcess.Location = new System.Drawing.Point(720, 34);
            this.BoxProcess.Name = "BoxProcess";
            this.BoxProcess.Size = new System.Drawing.Size(697, 604);
            this.BoxProcess.TabIndex = 2;
            this.BoxProcess.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(655, 663);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(655, 694);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sigma";
            // 
            // TxtSize
            // 
            this.TxtSize.Location = new System.Drawing.Point(720, 656);
            this.TxtSize.Name = "TxtSize";
            this.TxtSize.Size = new System.Drawing.Size(125, 27);
            this.TxtSize.TabIndex = 8;
            // 
            // TxtSigma
            // 
            this.TxtSigma.Location = new System.Drawing.Point(720, 691);
            this.TxtSigma.Name = "TxtSigma";
            this.TxtSigma.Size = new System.Drawing.Size(125, 27);
            this.TxtSigma.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 731);
            this.Controls.Add(this.TxtSigma);
            this.Controls.Add(this.TxtSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoxProcess);
            this.Controls.Add(this.BoxInput);
            this.Controls.Add(this.ButHysteresys);
            this.Controls.Add(this.ButThreshold);
            this.Controls.Add(this.ButSuppression);
            this.Controls.Add(this.ButSobel);
            this.Controls.Add(this.ButGaus);
            this.Controls.Add(this.ButInput);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.BoxInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxProcess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButInput;
        private System.Windows.Forms.Button ButGaus;
        private System.Windows.Forms.Button ButSobel;
        private System.Windows.Forms.Button ButSuppression;
        private System.Windows.Forms.Button ButThreshold;
        private System.Windows.Forms.Button ButHysteresys;
        private Emgu.CV.UI.ImageBox BoxInput;
        private Emgu.CV.UI.ImageBox BoxProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtSize;
        private System.Windows.Forms.TextBox TxtSigma;
    }
}

