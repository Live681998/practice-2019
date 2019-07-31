using Controller;
using Tanks.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanks.Controller;

namespace Tanks.View
{
    public partial class MainForm : Form, IViewController
    {

        private PacmanController pacmanController;

        private bool newGame = false;

        public bool ActiveTimer { get => timer.Enabled; set => timer.Enabled = value; }

        public int MapWidth => imgCanvas.Width;
        public int MapHeight => imgCanvas.Height;

        public int FormWidth => Width;

        public MainForm(Size size)
        {
            MaximumSize = size;
            MinimumSize = size;
            BackColor = Color.Black;
            InitializeComponent();
            imgCanvas.Size = new Size(size.Width - 16, size.Height - 41);

        }

        public void SetController(PacmanController pacmanController)
        {
            this.pacmanController = pacmanController;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            pacmanController.Timer();
        }

        public void Render(bool isGame = true)
        {
            Bitmap bitmap = new Bitmap(imgCanvas.Width, imgCanvas.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            pacmanController.Walls.ForEach(wall => wall.Draw(graphics));

            pacmanController.Bullets.ForEach(bullet => bullet.Draw(graphics));

            pacmanController.Tanks.ForEach(tank => tank.Draw(graphics));

            pacmanController.Apples.ForEach(apple => apple.Draw(graphics));

            pacmanController.Rivers.ForEach(river => river.Draw(graphics));

            pacmanController.Booms.ForEach(boom => boom.Draw(graphics));

            if (isGame)
            {
                pacmanController.Player.Draw(graphics);

                graphics.DrawImage(Sprite.Image, new Rectangle(15, 20, 12, 12), new Rectangle(0, 143, 12, 12), GraphicsUnit.Pixel);

                graphics.DrawString(pacmanController.Score.ToString(), new Font(FontFamily.GenericSansSerif, 22, FontStyle.Bold),
                                new SolidBrush(Color.White), new PointF(35, 10));
            }
            else if (newGame)
            {
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Black)),
                   new Rectangle(0, 0, MapWidth, MapHeight));

                graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 64, FontStyle.Bold),
                                new SolidBrush(Color.LightSeaGreen), new PointF((MapWidth - 600) / 2, (MapHeight - 400) / 2));

                graphics.DrawString($"Score: {pacmanController.Score}", new Font(FontFamily.GenericSansSerif, 28, FontStyle.Bold),
                                new SolidBrush(Color.LightSeaGreen), new PointF((MapWidth - 200) / 2, (MapHeight - 160) / 2));

                if (DateTime.Now.Millisecond < 600)
                {
                    graphics.DrawString("Press any key", new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold),
                                    new SolidBrush(Color.LightSeaGreen), new PointF((MapWidth - 200) / 2, (MapHeight) / 2));
                }
            }

            imgCanvas.Image = bitmap;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            pacmanController.IsGame = true;
            newGame = true;
            pacmanController.PlayerInitialization();
            btnStart.Dispose();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            pacmanController.KeyDown(e.KeyCode);
        }
    }
}
