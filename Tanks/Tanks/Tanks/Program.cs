using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanks.View;
using Controller;
using Tanks.Model;
using Tanks.Model.Entities;
using System.Drawing;

namespace Tanks
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm form;

            

            int width;
            int height;

            if (args.Length > 0)
            {
                width = int.Parse(args[0]);
                height = int.Parse(args[1]);
            }
            else
            {
                width = 800;
                height = 600;
            }
            if (args.Length > 2)
            {
                DynamicEntity.speed = int.Parse(args[2]);
            }

            form = new MainForm(new Size(width, height));

            PacmanController pacmanController = new PacmanController(form);
            if (args.Length > 4)
            {
                pacmanController.SetCountObject(int.Parse(args[3]), int.Parse(args[4]));
            }

            form.SetController(pacmanController);

            if (args.Length > 5)
            {
                DynamicEntity.speed = int.Parse(args[5]);
            }

            pacmanController.NewGame();


            Application.Run(form);
        }
    }
}
