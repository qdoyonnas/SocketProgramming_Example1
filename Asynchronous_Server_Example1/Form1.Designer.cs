namespace Asynchronous_Server_Example1
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
			if (disposing && (components != null))
			{
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
			this.localPortTextbox = new System.Windows.Forms.TextBox();
			this.startButton = new System.Windows.Forms.Button();
			this.stopButton = new System.Windows.Forms.Button();
			this.logTextBox = new System.Windows.Forms.RichTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.playerCountLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// localPortTextbox
			// 
			this.localPortTextbox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.localPortTextbox.ForeColor = System.Drawing.SystemColors.Window;
			this.localPortTextbox.Location = new System.Drawing.Point(15, 31);
			this.localPortTextbox.Name = "localPortTextbox";
			this.localPortTextbox.Size = new System.Drawing.Size(140, 20);
			this.localPortTextbox.TabIndex = 11;
			this.localPortTextbox.Text = "1010";
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(50, 57);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(75, 23);
			this.startButton.TabIndex = 12;
			this.startButton.Text = "Start";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// stopButton
			// 
			this.stopButton.Location = new System.Drawing.Point(47, 277);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(75, 23);
			this.stopButton.TabIndex = 13;
			this.stopButton.Text = "Stop";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// logTextBox
			// 
			this.logTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.logTextBox.ForeColor = System.Drawing.Color.White;
			this.logTextBox.Location = new System.Drawing.Point(161, 15);
			this.logTextBox.Name = "logTextBox";
			this.logTextBox.Size = new System.Drawing.Size(288, 288);
			this.logTextBox.TabIndex = 14;
			this.logTextBox.Text = "";
            this.logTextBox.HideSelection = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(12, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Local Port";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(15, 99);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Player Count:";
			// 
			// playerCountLabel
			// 
			this.playerCountLabel.AutoSize = true;
			this.playerCountLabel.ForeColor = System.Drawing.Color.White;
			this.playerCountLabel.Location = new System.Drawing.Point(85, 99);
			this.playerCountLabel.Name = "playerCountLabel";
			this.playerCountLabel.Size = new System.Drawing.Size(13, 13);
			this.playerCountLabel.TabIndex = 17;
			this.playerCountLabel.Text = "0";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
			this.ClientSize = new System.Drawing.Size(459, 312);
			this.Controls.Add(this.playerCountLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.logTextBox);
			this.Controls.Add(this.stopButton);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.localPortTextbox);
			this.Name = "Form1";
			this.Text = "Game Server";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox localPortTextbox;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.RichTextBox logTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label playerCountLabel;
	}
}

