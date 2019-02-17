using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Asynchronous_Client_Example1
{
    public struct Player
    {
        public PictureBox pictureBox;
        public Point velocity;
        public long timeStamp;
    }

    public partial class Form1 : Form
    {
        public Random random;

        public int boxSize = 20;

        Player localPlayer;
        Dictionary<int, Player> remotePlayers = new Dictionary<int, Player>();

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
                string sRemotePort = remotePortTextBox.Text;
                string sLocalPort = localPortTextbox.Text;

                if( sAddress == string.Empty 
                    || sRemotePort == string.Empty
                    || sLocalPort == string.Empty ) { return; }

                client.Connect( sAddress, sRemotePort, sLocalPort );
                if( client.readyToSend ) {
					client.StartListening();
                    AddPlayer();

                    StartGame();
                }
            }
        }

        private void disconnectButton_Click( object sender, EventArgs e )
        {
            gameOn = false;
            if( localPlayer.pictureBox != null ) {
                canvas.Controls.Remove(localPlayer.pictureBox);
            }
            client.Disconnect();
        }

        #endregion

        public delegate void UpdatePlayerDelegate( int identity, long timeStamp, int x, int y);
        public void UpdatePlayer( int identity, long timeStamp, int x, int y )
        {
            if( canvas.InvokeRequired ) {
                UpdatePlayerDelegate del = new UpdatePlayerDelegate( UpdatePlayer );
                Invoke( del, identity, timeStamp, x, y );
                return;
            }

            if( identity == client.identity ) { return; }

            Player player;
            if( !remotePlayers.TryGetValue(identity, out player) ) {
                AddPlayer(identity, timeStamp, x, y);
                return;
            }

            if( timeStamp > player.timeStamp ) {
                SetPlayerPosition(player, timeStamp, x, y);
            }
        }

        private void AddPlayer( int identity = -1, long timeStamp = -1, int x = -1, int y = -1 )
        {
            bool isLocalPlayer = ( identity == -1 || identity == client.identity );

            Player player = new Player();
            player.pictureBox = new PictureBox();
            if( isLocalPlayer ) {
                player.pictureBox.Bounds = new Rectangle(random.Next(0, canvas.Bounds.Width - boxSize), random.Next(0, canvas.Bounds.Height - boxSize), boxSize, boxSize);
                player.pictureBox.BackColor = Color.Red;
            } else {
                player.pictureBox.Bounds = new Rectangle(x, y, boxSize, boxSize);
                player.pictureBox.BackColor = Color.FromArgb(random.Next(int.MaxValue / 2, int.MaxValue));
                player.timeStamp = timeStamp;
            }

            player.velocity = Point.Empty;
            while( Math.Abs(player.velocity.X) < 3 
                || Math.Abs(player.velocity.Y) < 3 )
            {
                player.velocity = new Point(random.Next(-4, 4), random.Next(-4, 4));
            }

            if( isLocalPlayer && localPlayer.pictureBox != null ) {
                canvas.Controls.Remove(localPlayer.pictureBox);
            }
            canvas.Controls.Add(player.pictureBox);
            if( isLocalPlayer ) {
                localPlayer = player;
            } else {
                remotePlayers.Add(identity, player);
            }
        }

        delegate void SetPlayerPositionDelegate( Player player, long timeStamp, int x, int y);
        public void SetPlayerPosition( Player player, long timeStamp, int x, int y )
        {
            if( player.pictureBox.InvokeRequired ) {
                SetPlayerPositionDelegate del = new SetPlayerPositionDelegate( SetPlayerPosition );
                Invoke( del, x, y );
            } else {
                player.pictureBox.Bounds = new Rectangle(x, y, boxSize, boxSize);
                player.timeStamp = timeStamp;
            }
        }

        delegate void SetLocalPlayerPositionDelegate( int x, int y);
        public void SetLocalPlayerPosition( int x, int y )
        {
            if( localPlayer.pictureBox.InvokeRequired ) {
                SetLocalPlayerPositionDelegate del = new SetLocalPlayerPositionDelegate( SetLocalPlayerPosition );
                Invoke( del, x, y );
                return;
            }

            localPlayer.pictureBox.Bounds = new Rectangle(x, y, boxSize, boxSize);

            if( localPlayer.pictureBox.Bounds.Left <= 0
                || localPlayer.pictureBox.Bounds.Right >= canvas.Bounds.Width )
            {
                localPlayer.velocity = new Point(-localPlayer.velocity.X, localPlayer.velocity.Y);
            }

            if( localPlayer.pictureBox.Bounds.Top <= 0
                || localPlayer.pictureBox.Bounds.Bottom >= canvas.Bounds.Height )
            {
                localPlayer.velocity = new Point(localPlayer.velocity.X, -localPlayer.velocity.Y);
            }

            client.SendPosition(localPlayer.pictureBox.Bounds.Location);
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
                SetLocalPlayerPosition(localPlayer.pictureBox.Bounds.Location.X + localPlayer.velocity.X, localPlayer.pictureBox.Bounds.Location.Y + localPlayer.velocity.Y);

                Thread.Sleep(33);
            }
        }
    }
}
