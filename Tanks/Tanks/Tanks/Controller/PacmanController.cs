using Tanks.Model;
using Tanks.Model.Entities;
using Tanks.Model.EntitiesViewModel;
using Tanks.View;
using Tanks.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Controller
{
    public class PacmanController
    {
        public int BlockSize;
        private IViewController view;
        public KolobokViewModel Player { get; set; }
        public List<TankViewModel> Tanks { get; set; }
        public List<BulletViewModel> Bullets { get; set; }
        public List<AppleViewModel> Apples { get; set; }
        public List<BoomViewModel> Booms { get; set; }
        public List<WallViewModel> Walls { get; set; }
        public List<RiverViewModel> Rivers { get; set; }
        public int Score { get; set; }
        private BindingList<Log> logs;
        private StatisticsForm form;
        private KolobokViewModel reservePlayer;

        public bool IsGame = false;
        private Stopwatch timer;
        private Random rnd;

        private int appleCount = 5;
        private int tankCount = 5;

        delegate void func(float dt);
        public void MainLoop()
        {
            float dt = timer.ElapsedMilliseconds / 1000f;
            timer.Restart();

            ResetReload(Player, dt);
            Tanks.ForEach(tank => ResetReload(tank, dt));

            
            CreateApple(2);
            CreateTank(1);

            RotateTank(0.5f);

            Collision(dt);

            ShootTanks();

            if (!StatisticsForm.isClosed)
            {
                RefreshLog();
                form.RefreshDgv(logs);
            }

            func f;
            if (IsGame)
            {
                f = Player.SetSprite;
                Booms.ForEach(i => f += i.SetSprite);
                f(dt);
            }

            Booms.Where(boom => boom.EndAnimation()).ToList()
                 .ForEach(bang => Booms.Remove(bang));

            view.Render(IsGame);
        }



        public void SetCountObject(int tanks, int apples)
        {
            tankCount = tanks;
            appleCount = apples;
        }

        public PacmanController(IViewController view)
        {
            this.view = view;
            BlockSize = view.FormWidth / 50;
            rnd = new Random();
        }

        public void NewGame()
        {
            StartGame();
        }

        public void StartGame()
        {
            LoadLevel(1);
            Tanks = new List<TankViewModel>();
            Bullets = new List<BulletViewModel>();
            Apples = new List<AppleViewModel>();
            Booms = new List<BoomViewModel>();
            
            Score = 0;
            while (Tanks.Count < tankCount)
            {
                CreateTank(100);
            }
            while (Apples.Count < appleCount)
            {
                CreateApple(100);
            }
            timer = new Stopwatch();
            timer.Start();
            view.ActiveTimer = true;
            MainLoop();
        }

        private void LoadLevel(int lvl)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "level", lvl.ToString() + ".lvl");

            if (File.Exists(path))
            {
                Walls = new List<WallViewModel>();
                Rivers = new List<RiverViewModel>();


                using (StreamReader sr = File.OpenText(path))
                {
                    int y = 0;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        for (int x = 0; x < line.Length; x++)
                        {
                            switch (line[x])
                            {
                                case 'p':
                                    reservePlayer = new KolobokViewModel(BlockSize * x, BlockSize * y, 15, 15, Direction.UP);
                                    break;
                                case 'v':
                                    Walls.Add(new WallViewModel(BlockSize * x, BlockSize * y, BlockSize, BlockSize, true));
                                    break;
                                case 'w':
                                    Walls.Add(new WallViewModel(BlockSize * x, BlockSize * y, BlockSize, BlockSize, false));
                                    break;
                                case 'r':
                                    Rivers.Add(new RiverViewModel(BlockSize * x, BlockSize * y, BlockSize, BlockSize));
                                    break;
                            }
                        }

                        y++;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException($"Level {lvl} not found");
            }
        }

        public void KeyDown(Keys key)
        {
            if (!IsGame)
            {
                IsGame = true;
                StartGame();
                PlayerInitialization();
                return;
            }
            switch (key)
            {
                case Keys.Left:
                    Player.ChangeDirection(Direction.LEFT);
                    break;
                case Keys.Up:
                    Player.ChangeDirection(Direction.UP);
                    break;
                case Keys.Right:
                    Player.ChangeDirection(Direction.RIGHT);
                    break;
                case Keys.Down:
                    Player.ChangeDirection(Direction.DOWN);
                    break;
                case Keys.Space:
                    CreateBullet(Player);
                    break;
                case Keys.R:
                    StartGame();
                    break;
                case Keys.P:
                    InitializationLog();
                    break;
            }
        }

        public void Timer()
        {
            MainLoop();
        }

        private void ResetReload(IShootingEntity shooter, float dt)
        {
            if (shooter != null)
            {
                if (shooter.Recharge > 0)
                {
                    shooter.Recharge -= dt;
                }
            }
        }

        private void Collision(float dt)
        {
            var delWalls = new List<WallViewModel>();
            var delTanks = new List<TankViewModel>();
            var delApples = new List<AppleViewModel>();
            var delBullets = new List<BulletViewModel>();

            if (IsGame)
            {
                DynamicEntity player = Player.Clone() as DynamicEntity;
                player.Move(dt);

                if (ObjectInScreen(player) &&
                    Rivers.Find(river => IsCollision(river, player)) == null &&
                    Walls.Find(wall => IsCollision(wall, player)) == null)
                {
                    bool isFree = true;

                    foreach (var tank in Tanks)
                    {
                        if (IsCollision(tank, player))
                        {
                            isFree = false;
                            CreateBang(player);
                            GameOver();
                        }
                    }

                    if (isFree)
                    {
                        Player.Move(dt);
                    }
                }

                Apples.ForEach(apple =>
                {
                    if (IsCollision(player, apple))
                    {
                        delApples.Add(apple);
                        Score++;
                    }
                });
            }

            Tanks.ForEach(t =>
            {
                t.Move(dt);
                if (!ObjectInScreen(t) ||
                    Walls.Find(wall => IsCollision(wall, t)) != null ||
                    Rivers.Find(river => IsCollision(river, t)) != null ||
                    Tanks.Find(tnk => tnk != t ? IsCollision(tnk, t) : false) != null)
                {
                    t.ChangeDirection();
                    t.Move(dt);
                }
            });

            Bullets.ForEach(bullet =>
            {
                bool delBullet = false;

                if (!ObjectInScreen(bullet))
                {
                    delBullet = true;
                }

                Walls.ForEach(wall =>
                {
                    if (IsCollision(bullet, wall))
                    {
                        if (wall.Destroyable)
                        {
                            delWalls.Add(wall);
                        }
                        delBullet = true;
                        CreateBang(bullet);
                    }
                });

                Tanks.ForEach(tank =>
                {
                    if (IsCollision(tank, bullet) && bullet.Shooter != tank)
                    {
                        delBullet = true;
                        delTanks.Add(tank);
                        CreateBang(tank);
                    }
                });

                if (IsGame)
                {
                    if (IsCollision(bullet, Player) && bullet.Shooter != Player)
                    {
                        delBullet = true;
                        CreateBang(Player);
                        GameOver(); 
                    }
                }

                if (delBullet)
                {
                    delBullets.Add(bullet);
                }
                else
                {
                    bullet.Move(dt);
                }

            });

            delWalls.ForEach(wall => Walls.Remove(wall));
            delTanks.ForEach(tank => Tanks.Remove(tank));
            delApples.ForEach(apple => Apples.Remove(apple));
            delBullets.ForEach(bullet => Bullets.Remove(bullet));
        }

        private bool IsCollision(BaseEntity entity1, BaseEntity entity2)
        {
            return (entity1.X + entity1.Width > entity2.X) &&
                (entity1.X <= entity2.X + entity2.Width) &&
                (entity1.Y + entity1.Height > entity2.Y) &&
                (entity1.Y <= entity2.Y + entity2.Height);
        }

        private bool ObjectInScreen(BaseEntity entity)
        {
            return (entity.X >= 0) && (entity.X + entity.Width <= view.MapWidth) &&
                (entity.Y >= 0) && (entity.Y + entity.Height <= view.MapHeight);
        }

        private void CreateTank(int percent)
        {
            if (Tanks.Count < tankCount && rnd.Next(100) <= percent)
            {
                int x = rnd.Next(view.MapWidth - BlockSize);
                int y = rnd.Next(view.MapHeight - BlockSize);
                var tank = new TankViewModel (x, y, 16, 16, (Direction)rnd.Next(4));

                if (Player != null)
                {
                    if (Walls.Find(wall => IsCollision(wall, tank)) == null &&
                    IsCollision(Player, tank) == false &&
                    Tanks.Find(tnk => tnk != tank ? IsCollision(tnk, tank) : false) == null)
                    {
                        Tanks.Add(tank);
                    }
                }
                else
                {
                    if (Walls.Find(wall => IsCollision(wall, tank)) == null &&
                    IsCollision(reservePlayer, tank) == false &&
                    Tanks.Find(tnk => tnk != tank ? IsCollision(tnk, tank) : false) == null)
                    {
                        Tanks.Add(tank);
                    }
                }

            }
        }

        private void CreateApple(int percent)
        {
            if (Apples.Count < appleCount && rnd.Next(100) <= percent)
            {
                int x = rnd.Next(view.MapWidth - BlockSize);
                int y = rnd.Next(view.MapHeight - BlockSize);
                var apple = new AppleViewModel(x, y, 12, 12);

                if (Player != null)
                {
                    if (Walls.Find(wall => IsCollision(wall, apple)) == null &&
                        IsCollision(Player, apple) == false)
                    {
                        Apples.Add(apple);
                    }
                }
                else
                {
                    if (Walls.Find(wall => IsCollision(wall, apple)) == null &&
                       IsCollision(reservePlayer, apple) == false)
                    {
                        Apples.Add(apple);
                    }
                }

            }
        }

        private void GameOver()
        {
            IsGame = false;
            
        }

        private void RotateTank(float percent)
        {
            Tanks.ForEach(tank =>
            {
                if (rnd.Next(100) < percent)
                {
                    tank.ChangeDirection((Direction)rnd.Next(4));
                }
            });
        }

        private void CreateBullet(DynamicEntity entity)
        {
            if ((entity as IShootingEntity).Recharge > 0)
            {
                return;
            }

            float x = entity.X + entity.Width / 2 - 6;
            float y = entity.Y + entity.Height / 2 - 6;

            Bullets.Add(new BulletViewModel(Convert.ToInt32(x), Convert.ToInt32(y), 10, 10, entity.Direction, entity));
            (entity as IShootingEntity).Recharge = 0.3f;
        }

        private void ShootTanks()
        {
            if (IsGame)
            {
                Rectangle p = new Rectangle(Player.X, Player.Y, Player.Width, Player.Height);

                foreach (var tank in Tanks)
                {
                    bool onTheWay = true;
                    Direction lastDir = tank.Direction;

                    if (p.X + p.Width < tank.X && p.Y + p.Height > tank.Y + tank.Height / 2 && p.Y < tank.Y + tank.Height / 2)
                    {
                        tank.Direction = Direction.LEFT;
                    }
                    else if (p.X > tank.X + tank.Width && p.Y + p.Height > tank.Y + tank.Height / 2 && p.Y < tank.Y + tank.Height / 2)
                    {
                        tank.Direction = Direction.RIGHT;
                    }
                    else if (p.Y + p.Width < tank.Y && p.X + p.Width > tank.X + tank.Width / 2 && p.X < tank.X + tank.Width / 2)
                    {
                        tank.Direction = Direction.UP;
                    }
                    else if (p.Y > tank.Y + tank.Width && p.X + p.Width > tank.X + tank.Width / 2 && p.X < tank.X + tank.Width / 2)
                    {
                        tank.Direction = Direction.DOWN;
                    }
                    else
                    {
                        onTheWay = false;
                    }

                    if (tank.Direction != lastDir && tank.Recharge < 0.1f)
                    {
                        tank.Recharge = 0.5f;
                    }

                    if (onTheWay)
                    {
                        CreateBullet(tank);
                    }

                }
            }
        }


        private void InitializationLog()
        {
            RefreshLog();
            form = new StatisticsForm(logs);
            form.Show();
        }

        private void RefreshLog()
        {
            logs = null;

            logs = new BindingList<Log>();

            logs.Add(new Log("Kolobok", Player.X, Player.Y));

            foreach (TankViewModel tank in Tanks)
            {
                logs.Add(new Log("Tank", tank.X, tank.Y));
            }

            foreach (AppleViewModel apple in Apples)
            {
                logs.Add(new Log("Apple", apple.X, apple.Y));
            }

            foreach (WallViewModel wall in Walls)
            {
                logs.Add(new Log("Wall", wall.X, wall.Y));
            }

            foreach (BulletViewModel bullet in Bullets)
            {
                logs.Add(new Log("Bullet", bullet.X, bullet.Y));
            }
        }

        private void CreateBang(BaseEntity entity)
        {
            Booms.Add(new BoomViewModel(entity.X + (entity.Width - 30) / 2, entity.Y + (entity.Height - 30) / 2, BlockSize, BlockSize));
        }

        public void PlayerInitialization()
        {
            Player = reservePlayer;
        }
    }
}
