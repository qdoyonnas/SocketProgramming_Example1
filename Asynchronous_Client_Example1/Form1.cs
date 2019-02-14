using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Asynchronous_Client_Example1
{
    public partial class Form1 : Form
    {
        public Random random;

        public int boxSize = 20;
        public PictureBox playerBox;
        public Point playerVel;

        public AsynchronousClient client;

        public Thread gameThread;
        public bool gameOn = false;

        public Form1()
        {
            client = new AsynchronousClient();

            InitializeComponent();
            random = new Random();
        }

        #region Button Events

        private void connectButton_Click( object sender, EventArgs e )
        {
            if( !gameOn ) {
                string sAddress = addressTextBox.Text;
                string sPort = portTextBox.Text;

                if( sAddress == string.Empty
                    || sPort == string.Empty ) { return; }

                client.Connect(sAddress, sPort);
                if( client.readyToSend ) {
                    Console.WriteLine("Successfully connected");
                    AddPlayer();

                    StartGame();
                }
            }
        }

        private void disconnectButton_Click( object sender, EventArgs e )
        {
            gameOn = false;
            if( playerBox != null ) {
                canvas.Controls.Remove(playerBox);
            }
        }

        #endregion

        private void AddPlayer()
        {
            if( playerBox != null ) {
                canvas.Controls.Remove(playerBox);
            }
            playerBox = new PictureBox();
            playerBox.BackColor = Color.Red;
            playerBox.Bounds = new Rectangle(random.Next(0, canvas.Bounds.Width - boxSize), random.Next(0, canvas.Bounds.Height - boxSize), boxSize, boxSize);

            playerVel = Point.Empty;
            while( Math.Abs(playerVel.X) < 3 
                || Math.Abs(playerVel.Y) < 3 )
            {
                playerVel = new Point(random.Next(-4, 4), random.Next(-4, 4));
            }

            canvas.Controls.Add(playerBox);
        }

        delegate void SetPlayerPositionDelegate( int x, int y);
        public void SetPlayerPosition( int x, int y )
        {
            if( playerBox.InvokeRequired ) {
                SetPlayerPositionDelegate del = new SetPlayerPositionDelegate( SetPlayerPosition );
                Invoke( del, new object[] { x, y } );
            } else {
                playerBox.Bounds = new Rectangle(x, y, boxSize, boxSize);
                if( playerBox.Bounds.Left <= 0
                    || playerBox.Bounds.Right >= canvas.Bounds.Width )
                {
                    playerVel = new Point(-playerVel.X, playerVel.Y);
                }

                if( playerBox.Bounds.Top <= 0
                    || playerBox.Bounds.Bottom >= canvas.Bounds.Height )
                {
                    playerVel = new Point(playerVel.X, -playerVel.Y);
                }
            }

            client.SendPosition(playerBox.Bounds.Location);
        }

        public void StartGame()
        {
            if( !gameOn ) {
                gameThread = new Thread( new ThreadStart( RunGame ) );
                gameThread.Start();
                gameOn = true;
            }
        }

        // Running on separate Thread
        void RunGame()
        {
            while( gameOn ) {
                SetPlayerPosition(playerBox.Bounds.Location.X + playerVel.X, playerBox.Bounds.Location.Y + playerVel.Y);

                Thread.Sleep(33);
            }
        }
    }
}
