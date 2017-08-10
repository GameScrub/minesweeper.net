using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MineSweeper.Forms
{
    public partial class Main : Form
    {
        private int _bomb = 99;
        private Manager _game;
        private const int StartHeight = 50;
        private int _x = 30;
        private int _y = 16;
        private DateTime _startTime;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;

        public Main()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            ClearBoard();

            _totalElapsedTime = TimeSpan.Zero;
            _currentElapsedTime = TimeSpan.Zero;

            toolStripLabelTime.Text = "Time: ";

            timerGame.Stop();

            _game = new Manager(_x, _y, _bomb);
            
            Draw();

            DisplayMines();
        }

        private void Draw()
        {
            for (var i = 0; i < _x; i++)
                for (var j = 0; j < _y; j++)
                {
                    var button = new Button
                    {
                        Width = 20,
                        Height = 20,
                        Name = "btn_" + i + "_" + j,
                        Location = new Point(i*20, j*20)
                    };

                    button.MouseUp += Button_MouseUp;                    

                    pnlMineField.Controls.Add(button);
                }

            var point = new Point(_x*20, _y*20);
            Width = point.X + 40;
            Height = point.Y + 80 + StartHeight;

            pnlMineField.Width = Width;
            pnlMineField.Height = Height;
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {

            if (_game.Finished() || _game.MineExploded())
                return;

            if (!timerGame.Enabled)
            {
                _startTime = DateTime.Now;

                _totalElapsedTime = _currentElapsedTime;

                timerGame.Start();
            }        
                
            

            var button = (Button) sender;
            var point = new Point
            {
                X = int.Parse(button.Name.Split('_')[1]),
                Y = int.Parse(button.Name.Split('_')[2])
            };

            switch (e.Button)
            {
                case MouseButtons.Right:
                    _game.PlantFlag(point.X, point.Y);
                    break;
                case MouseButtons.Left:
                    _game.PickField(point.X, point.Y);
                    break;
            }

            if (_game.Finished())
            {
                timerGame.Stop();
                MessageBox.Show("YOU WON!");                
            }

            if (_game.MineExploded())
            {
                timerGame.Stop();
                MessageBox.Show("Game Over");
                ReDraw(true);                
            }
            else
                ReDraw();
                            
        }

        private void ReDraw(bool showMines = false)
        {
            foreach (Control control in pnlMineField.Controls)
            {
                if (!(control is Button))
                    continue;

                var button = (Button) control;

                var point = new Point
                {
                    X = int.Parse(button.Name.Split('_')[1]),
                    Y = int.Parse(button.Name.Split('_')[2])
                };

                var field = _game.MineFields.First(f => (f.X == point.X) && (f.Y == point.Y));


                if (field.IsOpenField && (field.BombsNearMe > 0))
                {
                    button.TabStop = false;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.Text = field.BombsNearMe.ToString();

                    switch (field.BombsNearMe)
                    {
                        case 1:
                            button.ForeColor = Color.Blue;
                            break;
                        case 2:
                            button.ForeColor = Color.Crimson;
                            break;
                        case 3:
                            button.ForeColor = Color.BlueViolet;
                            break;
                        case 4:
                            button.ForeColor = Color.Brown;
                            break;
                        case 6:
                            button.ForeColor = Color.DarkGreen;
                            break;
                        case 7:
                            button.ForeColor = Color.HotPink;
                            break;
                        case 8:
                            button.ForeColor = Color.Salmon;
                            break;
                    }
                }
                else if (field.IsOpenField && (field.BombsNearMe == 0))
                    button.Visible = false;
                else if (field.HasFlag)                    
                    button.Text = "!";                
                else if (field.HasBomb && field.IsOpenField)
                {
                    button.BackColor = Color.Red;
                    button.ForeColor = Color.Black;
                    button.Text = "X";
                }
                else if(showMines && field.HasBomb)
                    button.Text = "X";
                else
                    button.Text = "";
            }

            DisplayMines();
           
        }
       
        private void DisplayMines()
        {
            toolStripLabelMines.Text = "Mines: " + _game.BombsLeft().ToString();
        }

        private void ClearBoard()
        {
            pnlMineField.Controls.Clear();            
        }

        private void expertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _x = 30;
            _y = 16;
            _bomb = 99;
            StartGame();
        }

        private void intermediateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _x = 16;
            _y = 16;
            _bomb = 40;
            StartGame();
        }

        private void beginnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _x = 9;
            _y = 9;
            _bomb = 10;
            StartGame();
        }

        private void timerGame_Tick(object sender, EventArgs e)
        {            
            var timeSinceStartTime = DateTime.Now - _startTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours, timeSinceStartTime.Minutes, timeSinceStartTime.Seconds);
            
            _currentElapsedTime = timeSinceStartTime + _totalElapsedTime;
            
            toolStripLabelTime.Text = "Time: " + _currentElapsedTime;
            
        }
    }
}