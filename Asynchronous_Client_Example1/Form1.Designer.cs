namespace Asynchronous_Client_Example1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.addressTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.canvas = new System.Windows.Forms.PictureBox();
			this.connectButton = new System.Windows.Forms.Button();
			this.disconnectButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.remotePortTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.localPortTextbox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
			this.SuspendLayout();
			// 
			// addressTextBox
			// 
			this.addressTextBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.addressTextBox.ForeColor = System.Drawing.SystemColors.Window;
			this.addressTextBox.Location = new System.Drawing.Point(650, 34);
			this.addressTextBox.Name = "addressTextBox";
			this.addressTextBox.Size = new System.Drawing.Size(140, 20);
			this.addressTextBox.TabIndex = 3;
			this.addressTextBox.Text = "127.0.0.1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(650, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Address";
			// 
			// canvas
			// 
			this.canvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.canvas.Location = new System.Drawing.Point(12, 12);
			this.canvas.Name = "canvas";
			this.canvas.Size = new System.Drawing.Size(632, 426);
			this.canvas.TabIndex = 7;
			this.canvas.TabStop = false;
			// 
			// connectButton
			// 
			this.connectButton.Location = new System.Drawing.Point(650, 200);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(75, 23);
			this.connectButton.TabIndex = 8;
			this.connectButton.Text = "Connect";
			this.connectButton.UseVisualStyleBackColor = true;
			this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
			// 
			// disconnectButton
			// 
			this.disconnectButton.Location = new System.Drawing.Point(650, 415);
			this.disconnectButton.Name = "disconnectButton";
			this.disconnectButton.Size = new System.Drawing.Size(75, 23);
			this.disconnectButton.TabIndex = 9;
			this.disconnectButton.Text = "Disconnect";
			this.disconnectButton.UseVisualStyleBackColor = true;
			this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(650, 74);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Remote Port";
			// 
			// remotePortTextBox
			// 
			this.remotePortTextBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.remotePortTextBox.ForeColor = System.Drawing.SystemColors.Window;
			this.remotePortTextBox.Location = new System.Drawing.Point(650, 90);
			this.remotePortTextBox.Name = "remotePortTextBox";
			this.remotePortTextBox.Size = new System.Drawing.Size(140, 20);
			this.remotePortTextBox.TabIndex = 4;
			this.remotePortTextBox.Text = "1010";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(650, 158);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Local Port";
			// 
			// localPortTextbox
			// 
			this.localPortTextbox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.localPortTextbox.ForeColor = System.Drawing.SystemColors.Window;
			this.localPortTextbox.Location = new System.Drawing.Point(650, 174);
			this.localPortTextbox.Name = "localPortTextbox";
			this.localPortTextbox.Size = new System.Drawing.Size(140, 20);
			this.localPortTextbox.TabIndex = 10;
			this.localPortTextbox.Text = "1011";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.localPortTextbox);
			this.Controls.Add(this.disconnectButton);
			this.Controls.Add(this.connectButton);
			this.Controls.Add(this.canvas);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.remotePortTextBox);
			this.Controls.Add(this.addressTextBox);
			this.Name = "Form1";
			this.Text = "Game Client";
			((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion
		private System.Windows.Forms.TextBox addressTextBox;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox remotePortTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox localPortTextbox;
    }
}

