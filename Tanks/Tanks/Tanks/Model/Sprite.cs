using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Tanks.Model
{
    public class Sprite
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int? FPS { get; private set; }
        public int? Count { get; private set; }
        private int? index;
        public bool EndAnimation { get; private set; }
        public static Image Image { get; private set; }

        private void LoadImage()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "sprite.png");
            if (File.Exists(path))
            {
                Image = Image.FromFile(path);
            }
            else
            {
                throw new FileNotFoundException($"\"{path}\" is not found!");
            }
        }

        public Sprite(int x, int y, int width, int height)
        {
            if (Image == null)
            {
                LoadImage();
            }
            else
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }

        public Sprite(int x, int y, int width, int height, int count, int fps)
        {
            if (Image == null)
            {
                LoadImage();
            }
            else
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Count = count;
                FPS = fps;
                index = 0;
            }
        }

        public void SetFrame(int dx)
        {
            index += dx * FPS;
            if (index > Count)
            {
                index -= Count;
                EndAnimation = true;
            }
        }

        public void Draw(Graphics g, int x, int y)
        {
            if (FPS == null)
            {
                g.DrawImage(Image, new Rectangle(x, y, Width, Height), new Rectangle(X, Y, Width, Height), GraphicsUnit.Pixel);
            }
            else
            {
                g.DrawImage(Image, new Rectangle(x, y, Width, Height), new Rectangle(X + (int)index * Width, Y, Width, Height), GraphicsUnit.Pixel);
            }
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
