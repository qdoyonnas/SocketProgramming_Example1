using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asynchronous_Server_Example1
{
	public partial class Form1 : Form
	{
		public AsynchronousServer server;

		public Form1()
		{
			server = new AsynchronousServer();

			InitializeComponent();
		}

		#region Button Methods

		private void startButton_Click( object sender, EventArgs e )
		{
			string sPort = localPortTextbox.Text;
			if( sPort == string.Empty ) { return; }

			server.Start( sPort );
		}

		private void stopButton_Click( object sender, EventArgs e )
		{
			server.Stop();
		}

		#endregion

        delegate void UpdatePlayerCountDelegate( int count );
        public void UpdatePlayerCount( int count )
        {
            if( playerCountLabel.InvokeRequired ) {
                UpdatePlayerCountDelegate del = new UpdatePlayerCountDelegate( UpdatePlayerCount );
                Invoke( del, count );
                return;
            }

            playerCountLabel.Text = count.ToString();
        }

        delegate void PrintLogDelegate( string log );
        public void PrintLog( string log )
        {
            if( logTextBox.InvokeRequired ) {
                PrintLogDelegate del = new PrintLogDelegate( PrintLog );
                Invoke( del, log );
                return;
            }

            if( logTextBox.Text != string.Empty ) {  log = "\n" + log; }
            logTextBox.AppendText(log);
        }
	}
}
