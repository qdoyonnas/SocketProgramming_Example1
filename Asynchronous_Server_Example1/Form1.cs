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


	}
}
