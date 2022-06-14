using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using WMPLib;
using System.Linq;
using System.Xml.Linq;

class Program : Form
{
    const int WINDOW_WIDTH = 1334;
    const int WINDOW_HEIGHT = 750;
    const int FONT_SIZE = 14;
    const int FPS = 30;
    const string ImagePath = "./assets/images/";
    const string SoundPath = "./assets/sounds/";
    const string MapPath = "./assets/map/";
    const string CharaPath = "./assets/charas/";
    const string DataPath = "./assets/data/";
    const string OrganizationPath = "./assets/organization/";
    static string GameMapPath = "";
    static string MainGameStu = "MainGame";
    static bool StartUpFlag = true;
    static bool MainSoundFlag = true;
    static bool BackSoundFlag = true;
    static bool MapFirstFlag = true;
    static bool MainGameLoadFlag = true;
    static bool mScene6LoadFlag = true;
    static bool MainGameMarker = false;
    static int BeforeMainGameFrame = 0;
    //static bool ImageFlag = true;
    //string SoundFile ;
    static int nX = 0 ,nY = 0;
    static int Flame_Color = 1;
    static int MainGameFrontBack = 1;
    static int Flame_Cursor = 0;
    static int CommentCount = 0;
    static int Map_Cursor = 0;
    static int MapRound_Cursor = 0;
    static int UpGrade_Cursor = 0;
    static int NormalCharaLength = 0;
    static int MainGameFrame = 0;
    static int TowerStrength = 0;
    static int NowTowerStrength = 0;
    static int LoseFlagX = 0;
    static int LoseFlagY = 0;
    static int MaxMoney = 0;
    static int NowMoney = 0;
    static int MoneyUpSpeed = 0;
    static int EnemyTowerStrength = 0;
    static int EnemyNowTowerStrength = 0;
    static int OrderedCharas = -1;
    static int TowerAttackInterval = 0;
    static int NowTowerAttackInterval = 0;
    static int TowerAttack = 0;
    static int TowerAttackScene = 0;
    static int LevelUpScene = 0;
    static int MaxMoneyStep = 0;
    static int MoneyUpCost = 0;
    static int MoneyUpCostStep = 0;

    static List<int> CharasCost = new List<int>() { 100000, 100000, 100000, 100000, 100000 };
    static List<Image> ListCharaImages = new List<Image>();
    static List<string> Comments = new List<string>();
    static List<string> CharaList = new List<string>();
    static List<string> MapList = new List<string>();
    static List<string> PlaceNames = new List<string>();
    static List<Image> UpgradeCharaImage = new List<Image> { };

    static List<EnemyInfo> EnemyInfoList = new List<EnemyInfo>();
    static List<EnemyRunning> EnemyRunnings = new List<EnemyRunning>();
    static List<CharaInfo> CharaInfoList = new List<CharaInfo>();
    static List<CharaRunning> CharaRunnings = new List<CharaRunning>();

    WindowsMediaPlayer MainMediaPlayer = new WindowsMediaPlayer();
    WindowsMediaPlayer BackMediaPlayer = new WindowsMediaPlayer();

    static int DashAnimationScene = 0;

    static Image StartUp_0_img = Image.FromFile(ImagePath + "StartUp_0.png");
    static Image StartUp_1_img = Image.FromFile(ImagePath + "StartUp_1.png");
    static Image Dashboard_img = Image.FromFile(ImagePath + "Dashboard.png");
    static Image Dash_img = Image.FromFile(ImagePath + "Dash.png");
    static Image DashAnimationImageScene0 = Image.FromFile(ImagePath + "DashAnimationScene0.png");
    static Image DashAnimationImageScene1 = Image.FromFile(ImagePath + "DashAnimationScene1.png");
    static Image DashAnimationImageScene2 = Image.FromFile(ImagePath + "DashAnimationScene2.png");
    static Image DashAnimationImageScene3 = Image.FromFile(ImagePath + "DashAnimationScene3.png");
    static Image Map_img = Image.FromFile(MapPath + "Map.jpg");
    static Image MainGame_img = Image.FromFile(ImagePath + "MainGame.png");
    static Image MapChar = Image.FromFile(MapPath + "char.png");
    static Image MapPoint = Image.FromFile(MapPath + "point.png");
    static Image UpGrade_img = Image.FromFile(ImagePath + "UpGrade.png");
    static Image extrachara_img = Image.FromFile(ImagePath + "extrachara.png");
    static Image goback_img = Image.FromFile(ImagePath + "goback.png");
    static Image charaflame_img = Image.FromFile(ImagePath + "charaflame.png");
    static Image okmark_img = Image.FromFile(ImagePath + "okmark.png");
    static Image CannonStrength_img = Image.FromFile(ImagePath + "CannonStrength.jpg");
    static Image CannonLength_img = Image.FromFile(ImagePath + "CannonLength.jpg");
    static Image CannonCharge_img = Image.FromFile(ImagePath + "CannonCharge.jpg");
    static Image CannonEndurance_img = Image.FromFile(ImagePath + "CannonEndurance.jpg");
    static Image WorkAmount_img = Image.FromFile(ImagePath + "WorkAmount.jpg");
    static Image TotalUp_img = Image.FromFile(ImagePath + "TotalUp.jpg");
    static Image Wallet_img = Image.FromFile(ImagePath + "Wallet.jpg");
    static Image MapFrame_img = Image.FromFile(ImagePath + "mapFrame.png");
    static Image MapFrames_img = Image.FromFile(ImagePath + "mapFrames.png");
    static Image BeforeGameText_img = Image.FromFile(ImagePath + "BeforeGameText.png");
    static Image BeforeGameChara_img = Image.FromFile(ImagePath + "BeforeGameChara.png");
    static Image Explosion_img = Image.FromFile(ImagePath + "explosion.png");
    static Image Dirt_img = Image.FromFile(ImagePath + "Dirt.png");
    static Image Shadow_img = Image.FromFile(ImagePath + "shadow.png");
    static Image TowerAttack_img = Image.FromFile(ImagePath + "tower_ready.png");
    static Image WhiteTowerAttack_img = Image.FromFile(ImagePath + "white_tower_ready.png");
    static Image LevelUp_img = Image.FromFile(ImagePath + "levelUp.png");
    static Image WhiteLevelUp_img = Image.FromFile(ImagePath + "white_levelUp.png");

    static float transparency = 0;
    static int StartUpTextLocation = 0;

    Random rand = new Random();

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch extra_sw = new System.Diagnostics.Stopwatch();

    
    private System.Media.SoundPlayer player = null;

    static byte mScene = 0; //0:起動画面 1:スタート画面 2:ダッシュボード 3:日本地図 4:戦闘画面（メイン） 5：戦闘終了画面 6:パワーアップ画面

    protected override void OnKeyUp(KeyEventArgs e)
    {
        var c = e.KeyCode;
        //Console.WriteLine("KEYUP:" + c);

        if (mScene == 0 && !StartUpFlag && e.KeyCode == Keys.Return)
        {
            MainSoundFlag = true;
            BackSoundFlag = true;
            ButtonPushed();
            mScene = 1;
            
            return;
        }
        if (mScene == 1)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (Flame_Cursor == 0)
                {
                    string[] lines = File.ReadAllLines(DataPath + "Comments.txt");

                    string comment = "";

                    foreach (string line in lines)
                    {
                        if (line == "#")
                        {
                            comment = comment.Replace("#", "");
                            Comments.Add(comment);
                            comment = "";
                        }
                        comment = comment + line;
                    }

                    for (int i = Comments.Count - 1; i > 0; i--)
                    {
                        var j = rand.Next(0, i + 1); // ランダムで要素番号を１つ選ぶ（ランダム要素）
                        var temp = Comments[i]; // 一番最後の要素を仮確保（temp）にいれる
                        Comments[i] = Comments[j]; // ランダム要素を一番最後にいれる
                        Comments[j] = temp; // 仮確保を元ランダム要素に上書き
                    }

                    transparency = 0;
                    mScene = 2;
                    MainSoundFlag = true;
                    ButtonPushed();
                }
            }
            return;
        }
        if (mScene == 2)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (Flame_Cursor == 0)
                {
                    ButtonPushed();
                    DashAnimationScene++;
                }
            }

            if (Flame_Cursor == 5 && e.KeyCode == Keys.Return)
            {
                Flame_Cursor = 0;
                MainSoundFlag = true;
                BackSoundFlag = true;
                mScene = 1;
                ButtonPushed();
            }

            if (Flame_Cursor == 1 && e.KeyCode == Keys.Return)
            {
                ButtonPushed();
                Flame_Cursor = 0;
                mScene = 6;
                transparency = 0;
            }
            return;
            
        }
        if (mScene == 3)
        {
            if (e.KeyCode == Keys.Return && Flame_Cursor == 2)
            {
                PlayBackSound(SoundPath + "Gamestart.wav");

                BeforeMainGameFrame++;
            }

            if (e.KeyCode == Keys.Return && Flame_Cursor == 0)
            {
                transparency = 0;
                Flame_Cursor = 2;
                ButtonPushed();
                
            }
            if (e.KeyCode == Keys.Return && Flame_Cursor == 1)
            {
                transparency = 0;
                mScene = 2;
                Flame_Cursor = 0;
                MainSoundFlag = true;
                DashAnimationScene = -1;
                ButtonPushed();
            }

            return;
        }
        if (mScene == 4)
        {
            return;
        }
        if (mScene == 6)
        {
            if (e.KeyCode == Keys.Return && Flame_Cursor == 1)
            {
                transparency = 0;
                mScene = 2;
                MainSoundFlag = true;
                ButtonPushed();
            }
        }

    }
    protected override void OnKeyDown( KeyEventArgs e )
    {

        var c = e.KeyCode;
        Console.WriteLine("KEYDOWM:"+c);

        if (e.KeyCode == Keys.Escape)
        {
            System.Windows.Forms.Application.Exit();
        }

        if(mScene == 1)
        {
		    if(e.KeyCode == Keys.Up)
		    {
                Flame_Cursor--;
			    if(Flame_Cursor == -1)
			    {
                     Flame_Cursor = 3;
			    }
		    }
		    else if(e.KeyCode == Keys.Down)
		    {
                Flame_Cursor++;
			    if(Flame_Cursor == 4)
			    {
                    Flame_Cursor = 0;
			    }
		    }
		    
            return;
	    }
        if (mScene == 2)
        {
            if (e.KeyCode == Keys.Up)
            {
                Flame_Cursor--;
                if (Flame_Cursor == -1)
                {
                    Flame_Cursor = 5;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                Flame_Cursor++;
                if (Flame_Cursor >= 6)
                {
                    Flame_Cursor = 0;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                Flame_Cursor = 6;
            }
            else if (e.KeyCode == Keys.Left)
            {
                Flame_Cursor = 0;
            }
            if (Flame_Cursor == 6 && e.KeyCode == Keys.Return)
            {
                CommentCount++;
                if (CommentCount > Comments.Count - 1)
                {
                    CommentCount = 0;
                }
                
                PlayBackSound(SoundPath + "Comment.wav");
            }


            return;
        }
        if (mScene == 3)
        {
            if (e.KeyCode == Keys.Down)
            {
                Flame_Cursor++;
                MapRound_Cursor = 0;
                if (Flame_Cursor > 1)
                {
                    Flame_Cursor = 0;
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                Flame_Cursor--;
                MapRound_Cursor = 0;
                if (Flame_Cursor < 0)
                {
                    Flame_Cursor = 1;
                }
            }
            
            if (e.KeyCode == Keys.Left && Flame_Cursor == 0 )
            {
                Map_Cursor--;
                if (Map_Cursor < 0)
                {
                    Map_Cursor = PlaceNames.Count - 1;
                }
            }

            if (e.KeyCode == Keys.Left && Flame_Cursor == 2)
            { 
                MapRound_Cursor--;
                if (MapRound_Cursor < 0)
                {
                    MapRound_Cursor++;
                }
            }

            if (e.KeyCode == Keys.Right && Flame_Cursor == 0 )
            {
                Map_Cursor++;
                if (Map_Cursor > PlaceNames.Count - 1)
                {
                    Map_Cursor = 0;
                }
            }
            if (e.KeyCode == Keys.Right && Flame_Cursor == 2)
            {
                MapRound_Cursor++;
                if (MapRound_Cursor > MapList.Count - 3)
                {
                    MapRound_Cursor--;
                }
            }
            
            

            return;
        }
        if (mScene == 4)
        {
            if(e.KeyCode == Keys.Q)
            {
                FinishMainGame();
            }

            if(TowerAttackScene == 1 && e.KeyCode == Keys.S)
            {
                TowerAttackScene = 2;
            }
            
            if(LevelUpScene == 1 && e.KeyCode == Keys.A)
            {
		LevelUpScene = 0;
		NowMoney -= MoneyUpCost;
		MoneyUpSpeed--;
		MoneyUpCost += MoneyUpCostStep;
		MaxMoney += MaxMoneyStep;
	    }
	    

            if(e.KeyCode == Keys.D1)
            {
                OrderedCharas = 0;
            }
            else if (e.KeyCode == Keys.D2)
            {
                OrderedCharas = 1;
            }
            else if (e.KeyCode == Keys.D3)
            {
                OrderedCharas = 2;
            }
            else if (e.KeyCode == Keys.D4)
            {
                OrderedCharas = 3;
            }
            else if (e.KeyCode == Keys.D5)
            {
                OrderedCharas = 4;
            }

            if(e.KeyCode == Keys.D1 || e.KeyCode == Keys.D2 || e.KeyCode == Keys.D3 || e.KeyCode == Keys.D4 || e.KeyCode == Keys.D5)
            {
                if (NowMoney - CharasCost[OrderedCharas] >= 0)
                {
                    PlayBackSound(SoundPath + "CharOut.wav");

                    NowMoney -= CharasCost[OrderedCharas];

                    CharaRunning chararunning = new CharaRunning();

                    chararunning.X = 1200;
                    chararunning.Y = 0;
                    chararunning.Direction = 3;
                    chararunning.HP = CharaInfoList[OrderedCharas].HP;
                    chararunning.AttackInterval = 0;
                    chararunning.CharaType = OrderedCharas;
                    chararunning.Statement = 1;
                    chararunning.StaticAttackInterval = CharaInfoList[OrderedCharas].AttackInterval;
                    chararunning.Times = 0;
                    chararunning.DamageFlag[0] = true;
                    chararunning.DamageFlag[1] = true;

                    CharaRunnings.Add(chararunning);

                    
                }
                else
                {
                    PlayBackSound(SoundPath + "NotGood.wav");
                }
                MainGameMarker = true;
            }

            return;
        }
        if (mScene == 6)
        {
            PlayBackSound(SoundPath + "comment.wav");
            if (e.KeyCode == Keys.Right && Flame_Cursor == 0)
            {
                UpGrade_Cursor++;
                if (CharaList.Count - 5 < UpGrade_Cursor)
                {
                    UpGrade_Cursor--;
                }

            }
            if (e.KeyCode == Keys.Left && Flame_Cursor == 0)
            {
                UpGrade_Cursor--;
                if (UpGrade_Cursor < 0)
                {
                    UpGrade_Cursor = 0;
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                Flame_Cursor--;
                if (Flame_Cursor < 0)
                {
                    Flame_Cursor = 1;
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                Flame_Cursor++;
                if (Flame_Cursor > 1)
                {
                    Flame_Cursor = 0; 
                }
            }
        }
    }
    
    public static void Main(string[] args)
    {
        InfoLoad();
        Application.Run(new Program());
    }

    protected override void OnLoad( EventArgs e )
    {
        ClientSize = new Size(WINDOW_WIDTH, WINDOW_HEIGHT);
        DoubleBuffered = true;

        this.BackColor = Color.FromArgb(0,0,0);
        this.MaximizeBox = false;

        Task.Run(() =>
        {
           while (true)
           {
               update();
               Thread.Sleep(1000 / FPS);
               Invalidate();
           }
       });
    }

    public void update()
    {
        
        if (mScene == 0)
        {
            if (Time_Sleep(2000))
            {
                if (StartUpFlag == true)
                {
                    transparency = 0;
                }
                StartUpFlag = false;

            }
        }
        if (mScene == 1)
        {
            if (Time_Sleep(50))
            {
                Flame_Color = Flame_Color * -1;
            }
        }
        if (mScene == 2)
        {
            if (Time_Sleep(50))
            {
                Flame_Color = Flame_Color * -1;
            }
            if (DashAnimationScene >= 5)
            {
                mScene = 3;
                Flame_Cursor = 0;
                DashAnimationScene = -1;
            }

            
        }
        if (mScene == 3)
        {
            if (Time_Sleep(50))
            {
                Flame_Color = Flame_Color * -1;
            }
            if (BeforeMainGameFrame != 0)
            {
                if (Extra_Time_Sleep(200))
                {
                    BeforeMainGameFrame++;
                }
            }
            if (BeforeMainGameFrame >= 8)
            {
                mScene = 4;
                MainSoundFlag = true;
                Flame_Cursor = 0;
                transparency = 1.0f;
            }
        }
        if (mScene == 4)
        {
            PlayMainSound(SoundPath + "MainGame.wav");
            
            if (Time_Sleep(200))
            {
                MainGameFrontBack *= -1;
            }

        }
        if (mScene == 6)
        {
            if (Time_Sleep(50))
            {
                Flame_Color = Flame_Color * -1;
            }

        }

    }
   
    
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        if (mScene == 0)
        {
            if (StartUpFlag)
            {

                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

                ia = FadeOut(transparency);

                g.DrawImage(StartUp_0_img, new Rectangle(0, 0, StartUp_0_img.Width, StartUp_0_img.Height), 0, 0, StartUp_0_img.Width, StartUp_0_img.Height, GraphicsUnit.Pixel, ia);

                transparency += 0.1f;


            }
            else
            {
                PlayMainSound(SoundPath + "startlog.wav");

                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

                ia = FadeOut(transparency);

                g.DrawImage(StartUp_1_img, new Rectangle(0, 0, StartUp_1_img.Width, StartUp_1_img.Height), 0, 0, StartUp_1_img.Width, StartUp_1_img.Height, GraphicsUnit.Pixel, ia);

                transparency += 0.1f;

                string[] lines = File.ReadAllLines(DataPath + "StartUpText.txt");
                string text = String.Join("\n", lines);
                using (Font font = new Font("Meiryo UI", 24, FontStyle.Bold, GraphicsUnit.Point))
                {


                    Rectangle rect = new Rectangle(WINDOW_WIDTH / 2 - 500 / 2, WINDOW_HEIGHT - StartUpTextLocation, 600, 1000000000);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;

                    // Draw the text and the surrounding rectangle.

                    g.DrawString(text, font, Brushes.White, rect, stringFormat);

                    StartUpTextLocation++;
                }

            }
        }
        else if (mScene == 1)
        {
            PlayMainSound(SoundPath + "start.wav");

            int[,] places = new int[,]
            {
                {445,420,437,74},
                {394,531,258,71 },
                {681,530,257,70},
                {1195,595,134,121}

            };

            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

            ia = FadeOut(transparency);

            g.DrawImage(Dashboard_img, new Rectangle(0, 0, Dashboard_img.Width, Dashboard_img.Height),
                 0, 0, Dashboard_img.Width, Dashboard_img.Height, GraphicsUnit.Pixel, ia);


            Pen pen;
            if (Flame_Color == 1)
            {
                pen = new Pen(Color.Yellow, 8);
            }
            else
            {
                pen = new Pen(Color.DeepPink, 8);
            }

            Rectangle rect1 = new Rectangle(places[Flame_Cursor, 0], places[Flame_Cursor, 1], places[Flame_Cursor, 2], places[Flame_Cursor, 3]);

            e.Graphics.DrawRectangle(pen, rect1);

            transparency += 0.1f;
        }

        else if (mScene == 2)
        {
            PlayMainSound(SoundPath + "beforefight.wav");

            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

            ia = FadeOut(transparency);

            g.DrawImage(Dash_img, new Rectangle(0, 0, Dash_img.Width, Dash_img.Height),
                 0, 0, Dash_img.Width, Dash_img.Height, GraphicsUnit.Pixel, ia);


            transparency += 0.1f;

            int[,] places = new int[,]
            {
                { 28,92, 438,75},
                { 28,199, 440,75},
                { 28,305, 440,75},
                { 43,417, 125,103},
                { 185,412, 121,116},
                {  4,629 , 118,118},
                { 703,136, 600,285},
            };


            Pen pen;
            if (Flame_Color == 1)
            {
                pen = new Pen(Color.Yellow, 8);
            }
            else
            {
                pen = new Pen(Color.DeepPink, 8);
            }

            Rectangle rect1 = new Rectangle(places[Flame_Cursor, 0], places[Flame_Cursor, 1], places[Flame_Cursor, 2], places[Flame_Cursor, 3]);

            e.Graphics.DrawRectangle(pen, rect1);


            string text = String.Join("\n", Comments[CommentCount]);
            using (Font font = new Font("Meiryo UI", 24, FontStyle.Bold, GraphicsUnit.Point))
            {


                Rectangle rect = new Rectangle(720, 150, 561, 260);

                // Create a StringFormat object with the each line of text, and the block
                // of text centered on the page.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;


                // Draw the text and the surrounding rectangle.

                g.DrawString(text, font, Brushes.White, rect, stringFormat);
            }

            if (DashAnimationScene != 0)
            {
                ia = FadeOut(1.0f);

                if (DashAnimationScene == 1)
                {
                    g.DrawImage(DashAnimationImageScene0, new Rectangle(0, 0, DashAnimationImageScene0.Width, DashAnimationImageScene0.Height),
                 0, 0, DashAnimationImageScene0.Width, DashAnimationImageScene0.Height, GraphicsUnit.Pixel, ia);
                }
                if (DashAnimationScene == 2)
                {
                    g.DrawImage(DashAnimationImageScene1, new Rectangle(0, 0, DashAnimationImageScene1.Width, DashAnimationImageScene1.Height),
                 0, 0, DashAnimationImageScene1.Width, DashAnimationImageScene1.Height, GraphicsUnit.Pixel, ia);
                }
                if (DashAnimationScene == 3)
                {
                    g.DrawImage(DashAnimationImageScene2, new Rectangle(0, 0, DashAnimationImageScene2.Width, DashAnimationImageScene2.Height),
                 0, 0, DashAnimationImageScene2.Width, DashAnimationImageScene2.Height, GraphicsUnit.Pixel, ia);
                }
                if (DashAnimationScene == 4)
                {
                    g.DrawImage(DashAnimationImageScene3, new Rectangle(0, 0, DashAnimationImageScene3.Width, DashAnimationImageScene3.Height),
                 0, 0, DashAnimationImageScene3.Width, DashAnimationImageScene3.Height, GraphicsUnit.Pixel, ia);
                }

                if (Extra_Time_Sleep(50))
                {
                    DashAnimationScene++;
                }
            }
        }
        else if (mScene == 3)
        {
            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

            ia = FadeOut(transparency + Flame_Cursor);

            g.DrawImage(Map_img, new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT),
                 0, 0, WINDOW_WIDTH, WINDOW_HEIGHT, GraphicsUnit.Pixel, ia);
            
            
            ia = FadeOut(transparency);
            transparency += 0.1f;

            if (Flame_Cursor == 2)
            {
                g.DrawImage(MapFrame_img, new Rectangle(109, 184, 1078, 223),
                 0, 0, MapFrame_img.Width, MapFrame_img.Height, GraphicsUnit.Pixel, ia);


                string[] Paths = Directory.GetDirectories(MapPath + PlaceNames[Map_Cursor], "*");

                MapList.Clear();

                MapList.Add("S");

                //Console.WriteLine(Paths[0]);
                foreach (var path in Paths)
                {
                    MapList.Add(path);
                }

                MapList.Add("S");

                var Framework = MapList.GetRange(MapRound_Cursor, 3);


                int[,] MapPlaces = new int[,]
                {
                    { 195,231,212,132},
                    { 536,228,271,147},
                    { 881,231,212,132}
                };

                GameMapPath = Framework[1];
                
                //Console.WriteLine(GameMapPath);

                byte count = 0;
                for (int i = 0; i < Framework.Count; i++)
                {
                    if (Framework[i] != "S")
                    {
                        g.DrawImage(MapFrames_img, new Rectangle(MapPlaces[count, 0], MapPlaces[count, 1], MapPlaces[count, 2], MapPlaces[count, 3]),
                        0, 0, MapFrames_img.Width, MapFrames_img.Height, GraphicsUnit.Pixel, ia);

                        string[] te = Framework[i].Split('.');

                        Framework[i] = te[te.Length - 1];

                        using (Font font = new Font("Meiryo UI", 32, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            Rectangle rect = new Rectangle(MapPlaces[count, 0], MapPlaces[count, 1], MapPlaces[count, 2], MapPlaces[count, 3]);

                            // Create a StringFormat object with the each line of text, and the block
                            // of text centered on the page.
                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Center;
                            stringFormat.LineAlignment = StringAlignment.Center;


                            // Draw the text and the surrounding rectangle.

                            g.DrawString(Framework[i], font, Brushes.Blue, rect, stringFormat);
                        }
                    }

                    count++;
                }
                
            }


            foreach (var Place in PlaceNames)
            {
                XElement xm = XElement.Load(MapPath + Place + "/info.xml");

                // userを取得
                IEnumerable<XElement> us = from item in xm.Elements("position") select item;

                string r = "", t = "";
                foreach (var user in us)
                {
                    r = user.Element("X").Value;
                    t = user.Element("Y").Value;
                }

                int R = int.Parse(r);
                int T = int.Parse(t);

                g.DrawImage(MapPoint, new Rectangle(R - MapChar.Width / 2 + 20, T - MapChar.Height / 2 + 20, 40, 40),
                     0, 0, MapPoint.Width, MapPoint.Height, GraphicsUnit.Pixel, ia);

            }



            string PlaceName = PlaceNames[Map_Cursor];

            // XML読み込み
            XElement xml = XElement.Load(MapPath + PlaceName + "/info.xml");

            // userを取得
            IEnumerable<XElement> users = from item in xml.Elements("position") select item;

            string x = "",y = "";
            foreach (var user in users)
            {
                x = user.Element("X").Value;
                y = user.Element("Y").Value;
            }

            int X = int.Parse(x);
            int Y = int.Parse(y);

            if (MapFirstFlag)
            {
                
                nX = X;
                nY = Y;

                MapFirstFlag = false;
            }

            if (nX != X || nY != Y)
            {
                nX += (X - nX) / 5;
                nY += (Y - nY) / 5;
            }

            g.DrawImage(MapChar, new Rectangle(nX - MapChar.Width / 2 ,nY - MapChar.Height / 2, MapChar.Width, MapChar.Height),
                 0, 0, MapChar.Width, MapChar.Height, GraphicsUnit.Pixel, ia);

            string[] words = PlaceName.Split('.');

            using (Font font = new Font("Meiryo UI", 36, FontStyle.Bold, GraphicsUnit.Point))
            {
                Rectangle rect = new Rectangle(527, 80, 277, 91);

                // Create a StringFormat object with the each line of text, and the block
                // of text centered on the page.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;


                // Draw the text and the surrounding rectangle.

                g.DrawString(words[1], font, Brushes.Black, rect, stringFormat);
            }

            using (Font font = new Font("Meiryo UI", 32, FontStyle.Bold, GraphicsUnit.Point))
            {
                Rectangle rect = new Rectangle(527, 80, 277, 91);

                // Create a StringFormat object with the each line of text, and the block
                // of text centered on the page.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;


                // Draw the text and the surrounding rectangle.

                g.DrawString(words[1], font, Brushes.Gold, rect, stringFormat);
            }

            int[,] places = new int[,]
            {
                { 963,485, 348,80},
                { 6,636, 110,109},
                { 550,230,250,142},
            };


            Pen pen;
            if (Flame_Color == 1)
            {
                pen = new Pen(Color.Yellow, 8);
            }
            else
            {
                pen = new Pen(Color.DeepPink, 8);
            }

            Rectangle rect1 = new Rectangle(places[Flame_Cursor, 0], places[Flame_Cursor, 1], places[Flame_Cursor, 2], places[Flame_Cursor, 3]);

            e.Graphics.DrawRectangle(pen, rect1);


            int[,] BeforeGameTextPlaces = new int[,]
            {
                { 416,322, 181,175},
                { 622,316, 178,180},
                { 827,324, 178,169},
                { 1032,321, 178,175},
                { 1256,326,155,167 },
            };

            int[,] TextImagePlaces = new int[,]
            {
                { 12,8, 181,174},
                { 221,3, 178,180},
                { 423,6, 179,172},
                { 630,6, 186,173},
                { 852,12,148,169 },
            };



            if (BeforeMainGameFrame != 0)
            {
                if (BeforeMainGameFrame >= 5)
                {
                    g.DrawImage(BeforeGameChara_img, new Rectangle(964, 353 + (BeforeMainGameFrame % 2) * 10, 368, 339),
    0, 0, BeforeGameChara_img.Width, BeforeGameChara_img.Height, GraphicsUnit.Pixel, ia);

                    for (int i = 0; i < 5; i++)
                    {
                        g.DrawImage(BeforeGameText_img, new Rectangle(BeforeGameTextPlaces[i, 0] - 200, BeforeGameTextPlaces[i, 1] - 100, BeforeGameTextPlaces[i, 2], BeforeGameTextPlaces[i, 3]),
                TextImagePlaces[i, 0], TextImagePlaces[i, 1], TextImagePlaces[i, 2], TextImagePlaces[i, 3], GraphicsUnit.Pixel, ia);
                    }
                }
                else
                {
                    g.DrawImage(BeforeGameChara_img, new Rectangle(964, 353 + (BeforeMainGameFrame % 2) * 20, 368, 339 - (BeforeMainGameFrame % 2) * 20),
   0, 0, BeforeGameChara_img.Width, BeforeGameChara_img.Height , GraphicsUnit.Pixel, ia);

                    for (int i = 0; i < BeforeMainGameFrame; i++)
                    {
                        g.DrawImage(BeforeGameText_img, new Rectangle(BeforeGameTextPlaces[i, 0] - 200, BeforeGameTextPlaces[i, 1] - 100, BeforeGameTextPlaces[i, 2], BeforeGameTextPlaces[i, 3]),
                TextImagePlaces[i, 0], TextImagePlaces[i, 1], TextImagePlaces[i, 2], TextImagePlaces[i, 3], GraphicsUnit.Pixel, ia);
                    }

                }

            }

        }
        else if (mScene == 4)
        {
            // XML読み込み
            XElement xml = XElement.Load(OrganizationPath + "/info.xml");

            // userを取得
            IEnumerable<XElement> users = from item in xml.Elements("member") select item;

            // XML読み込み
            XElement xm = XElement.Load(GameMapPath + "/info.xml");

            // userを取得
            IEnumerable<XElement> usrs = from item in xm.Elements("member") select item;

            XElement m = XElement.Load(DataPath + "/info.xml");

            // userを取得
            IEnumerable<XElement> uss = from item in m.Elements("Info") select item;



            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

            ia = FadeOut(transparency);

            g.DrawImage(MainGame_img, new Rectangle(0, 0, MainGame_img.Width, MainGame_img.Height),
                 0, 0, MainGame_img.Width, MainGame_img.Height, GraphicsUnit.Pixel, ia);

            
            if (MainGameLoadFlag)
            {
               
                int c = 0;
                foreach(var user in users)
                {
                    Image image = Image.FromFile(OrganizationPath + user.Element("imagePath").Value + "0.png");
                    ListCharaImages.Add(image);

                    CharaInfo charainfo = new CharaInfo();

                    for(int i = 1; i < 6; i++)
                    {
                        charainfo.Image[i-1] = Image.FromFile(OrganizationPath + user.Element("imagePath").Value + i.ToString() + ".png");
                    }

                    
                    charainfo.HP = int.Parse(user.Element("hp").Value);
                    charainfo.Speed = int.Parse(user.Element("speed").Value);
                    charainfo.Attack = int.Parse(user.Element("offensive").Value);
                    charainfo.WaitingTime = int.Parse(user.Element("waitingTime").Value);
                    charainfo.FirstWaitingTime = int.Parse(user.Element("waitingTime").Value);
                    charainfo.AttackInterval = int.Parse(user.Element("AttackInterval").Value);
                    
                    charainfo.Width = int.Parse(user.Element("width").Value);
                    charainfo.Height = int.Parse(user.Element("height").Value);
                    charainfo.Type = user.Element("AttackStyle").Value;

                    CharasCost[c] = int.Parse(user.Element("cost").Value);

                    CharaInfoList.Add(charainfo);
                    c++;
                }
              

                c = 0;
                foreach (var us in usrs)
                {
                    var imagepath = us.Element("imagePath").Value;

                    EnemyInfo enemyinfo = new EnemyInfo();

                    for (int i = 0; i < 5; i++)
                    {
                        enemyinfo.Image[i] = Image.FromFile(GameMapPath + "/" + imagepath + i.ToString() + ".png");
                    }

                    enemyinfo.HP = int.Parse(us.Element("hp").Value);
                    enemyinfo.Speed = int.Parse(us.Element("speed").Value);
                    enemyinfo.Attack = int.Parse(us.Element("Attack").Value);
                    enemyinfo.WaitingTime = int.Parse(us.Element("waitingTime").Value);
                    enemyinfo.StaticWaitingTime = int.Parse(us.Element("waitingTime").Value);
                    enemyinfo.FirstWaitingTime = int.Parse(us.Element("firstWaitingTime").Value);
                    enemyinfo.AttackInterval = int.Parse(us.Element("AttackInterval").Value);
                    enemyinfo.Width = int.Parse(us.Element("width").Value);
                    enemyinfo.Height = int.Parse(us.Element("height").Value);

                    if (c == 0)
                    {
                        EnemyTowerStrength = int.Parse(us.Element("TowerStrength").Value);
                        EnemyNowTowerStrength = int.Parse(us.Element("TowerStrength").Value);
                    }
                    c++;

                    EnemyInfoList.Add(enemyinfo);
                }

                foreach (var asf in uss)
                {
                    TowerStrength = int.Parse(asf.Element("TowerStrength").Value);
                    NowTowerStrength = int.Parse(asf.Element("TowerStrength").Value);
                    MaxMoney = int.Parse(asf.Element("MaxMoney").Value);
                    MoneyUpSpeed = int.Parse(asf.Element("MoneyUpSpeed").Value);
                    TowerAttackInterval = int.Parse(asf.Element("TowerAttackInterval").Value);
                    NowTowerAttackInterval = int.Parse(asf.Element("TowerAttackInterval").Value);
                    TowerAttack = int.Parse(asf.Element("TowerAttack").Value);
                    MaxMoneyStep = int.Parse(asf.Element("MaxMoneyUpStep").Value);
                    MoneyUpCost = int.Parse(asf.Element("MoneyUpCost").Value);
                    MoneyUpCostStep = int.Parse(asf.Element("MoneyUpCostStep").Value);
                    
                }
         
                MainGameLoadFlag = false;
            }

            
            int[,] CharaPlaces = new int[,]
            {
                {295,643,120,89},
                {447,643,120,89},
                {598,643,120,89},
                {751,643,120,89},
                {903,643,120,89}
            };
            

            int[] CostOfCharaPlaces = new int[] { 336, 489, 635, 790, 938 };

            
            int count = 0;

            foreach (var user in users)
            {
                g.DrawImage(ListCharaImages[count], new Rectangle(CharaPlaces[count,0], CharaPlaces[count, 1], CharaPlaces[count, 2], CharaPlaces[count, 3]),
                 0, 0, ListCharaImages[count].Width, ListCharaImages[count].Height, GraphicsUnit.Pixel, ia);

                if(MainGameStu == "MainGame")
                {
                    /*
                    using (Font font = new Font("Meiryo UI", 18, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        Rectangle rect = new Rectangle(CostOfCharaPlaces[count], 705, 100, 31);

                        // Create a StringFormat object with the each line of text, and the block
                        // of text centered on the page.
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;


                        // Draw the text and the surrounding rectangle.

                        g.DrawString(user.Element("cost").Value + "円", font, Brushes.Black, rect, stringFormat);
                    }
                    */
                    using (Font font = new Font("Meiryo UI", 15, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        Rectangle rect = new Rectangle(CostOfCharaPlaces[count], 705, 100, 31);

                        // Create a StringFormat object with the each line of text, and the block
                        // of text centered on the page.
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;


                        // Draw the text and the surrounding rectangle.

                        g.DrawString(user.Element("cost").Value + "円", font, Brushes.DeepPink, rect, stringFormat);
                    }
                }

                
                count++;
            }

            if (MainGameFrame % (MoneyUpSpeed * 50) == 0)
            {
                NowMoney++;
                if (NowMoney >= MaxMoney)
                {
                    NowMoney = MaxMoney;
                }
            }


            for (int i = 0; i < EnemyInfoList.Count ; i++)
            {
                if (EnemyInfoList[i].FirstWaitingTime == 0)
                {
                    EnemyInfoList[i].WaitingTime = EnemyInfoList[i].StaticWaitingTime;

                    EnemyRunning enemyrunning = new EnemyRunning();

                    enemyrunning.X = 113;
                    enemyrunning.Y = 0;
                    enemyrunning.Direction = 3;
                    enemyrunning.HP = EnemyInfoList[i].HP;
                    enemyrunning.AttackInterval = 0;
                    enemyrunning.FirstWaitingTime = EnemyInfoList[i].FirstWaitingTime;
                    enemyrunning.CharaType = i;
                    enemyrunning.Statement = 1;
                    enemyrunning.StaticAttackInterval = EnemyInfoList[i].AttackInterval;
                    enemyrunning.Times = 0;
                    enemyrunning.DamageFlag[0] = true;
                    enemyrunning.DamageFlag[1] = true;

                    EnemyRunnings.Add(enemyrunning);
                }

                if (EnemyInfoList[i].FirstWaitingTime <= 0)
                {
                    EnemyInfoList[i].WaitingTime--;
                }
                if (EnemyInfoList[i].WaitingTime == 0)
                {
                    EnemyInfoList[i].WaitingTime = EnemyInfoList[i].StaticWaitingTime;

                    EnemyRunning enemyrunning = new EnemyRunning();

                    enemyrunning.X = 113;
                    enemyrunning.Y = 0;
                    enemyrunning.Direction = 3;
                    enemyrunning.HP = EnemyInfoList[i].HP;
                    enemyrunning.AttackInterval = 0;
                    enemyrunning.FirstWaitingTime = EnemyInfoList[i].FirstWaitingTime;
                    enemyrunning.CharaType = i;
                    enemyrunning.Statement = 1;
                    enemyrunning.StaticAttackInterval = EnemyInfoList[i].AttackInterval;
                    enemyrunning.Times = 0;
                    enemyrunning.DamageFlag[0] = true;
                    enemyrunning.DamageFlag[1] = true;

                    EnemyRunnings.Add(enemyrunning);
                }

                EnemyInfoList[i].FirstWaitingTime--;

            }
            
            for (int i = EnemyRunnings.Count - 1; i >= 0; i--)
            {

                //marker
                int x = 0;
                if (MainGameFrontBack == 1)
                {
                    x = 1;
                }
               
                
                //chara syoutotsu
                for (int j = 0; j < CharaRunnings.Count; j++)
                {
                    if (CharaRunnings[j].X - CharaInfoList[CharaRunnings[j].CharaType].Width - EnemyRunnings[i].X <= 100)
                    {
                        x = 0;
                        EnemyRunnings[i].X -= EnemyInfoList[EnemyRunnings[i].CharaType].Speed;

                        if(EnemyRunnings[i].AttackInterval <= 0 && EnemyRunnings[i].Statement == 1)
                        {
                            EnemyRunnings[i].Statement = 3;
                            EnemyRunnings[i].Times = 10;
                        }
                    }
                }

                //chara Tower Attack 
                if(EnemyRunnings[i].X + EnemyInfoList[EnemyRunnings[i].CharaType].Width >= 1100 && EnemyRunnings[i].Statement == 1 && EnemyRunnings[i].AttackInterval <= 0)
                {
                    EnemyRunnings[i].Statement = 3;
                    EnemyRunnings[i].Times = 10;
                }

                //update
                if (EnemyRunnings[i].Statement == 5)
                {
                    EnemyRunnings[i].X -= 8;

                    if (EnemyRunnings[i].Y >= 20)
                    {
                        EnemyRunnings[i].Direction = -3;
                    }
                    else if (EnemyRunnings[i].Y <= 0)
                    {
                        EnemyRunnings[i].Direction = 3;
                    }

                    EnemyRunnings[i].Y += EnemyRunnings[i].Direction;

                    if (EnemyRunnings[i].DamageFlag[1] == false && EnemyRunnings[i].Times == 0)
                    {
                        EnemyRunnings.RemoveAt(i);
                        continue;
                    }

                    if (EnemyRunnings[i].Times == 0)
                    {
                        EnemyRunnings[i].Statement = 1;
                        EnemyRunnings[i].AttackInterval = EnemyRunnings[i].StaticAttackInterval;
                        EnemyRunnings[i].Y = 0;
                    }
                    
                }
                
                if (EnemyRunnings[i].Statement != 1 && EnemyRunnings[i].Times == 0) 
                {
                    EnemyRunnings[i].Statement++;
                    EnemyRunnings[i].Times = 10;

                    if(EnemyRunnings[i].Statement == 5)
                    {
                        EnemyRunnings[i].Statement = 1;
                        EnemyRunnings[i].AttackInterval = EnemyRunnings[i].StaticAttackInterval;
                    }

                    if(EnemyRunnings[i].Statement == 4 && EnemyRunnings[i].X + EnemyInfoList[EnemyRunnings[i].CharaType].Width >= 1100)
                    {
                        NowTowerStrength -= EnemyInfoList[EnemyRunnings[i].CharaType].Attack;
                    }
                    if (EnemyRunnings[i].Statement == 4)
                    {
                        PlayBackSound(SoundPath + "CharAttacked.wav");
                    }

                    for (int j = 0; j < CharaRunnings.Count; j++)
                    {
                        if (CharaRunnings[j].X - CharaInfoList[CharaRunnings[j].CharaType].Width - EnemyRunnings[i].X <= 110)
                        {
                            CharaRunnings[j].HP -= EnemyInfoList[EnemyRunnings[i].CharaType].Attack;
                        }
                    }
                }

                
                if (EnemyRunnings[i].HP - EnemyInfoList[EnemyRunnings[i].CharaType].HP / 2 <= 0 && EnemyRunnings[i].DamageFlag[0] == true)
                {
                    EnemyRunnings[i].DamageFlag[0] = false;

                    EnemyRunnings[i].Statement = 5;
                    EnemyRunnings[i].Times = 20;
                }

                //explosion animation
                if (EnemyRunnings[i].Statement == 4 && EnemyRunnings[i].Times > 5)
                {
                    g.DrawImage(Explosion_img, new Rectangle(EnemyRunnings[i].X + EnemyInfoList[EnemyRunnings[i].CharaType].Width - 20, 474 - 10 * i + 60 - EnemyInfoList[EnemyRunnings[i].CharaType].Height - 30, 120, 120),
             0, 0, Explosion_img.Width, Explosion_img.Height, GraphicsUnit.Pixel, ia);

                }
                
                //walking
                if (EnemyRunnings[i].X + EnemyInfoList[EnemyRunnings[i].CharaType].Width < 1100)
                {
                    EnemyRunnings[i].X += EnemyInfoList[EnemyRunnings[i].CharaType].Speed;
                }

                if (EnemyRunnings[i].X + EnemyInfoList[EnemyRunnings[i].CharaType].Width >= 1100 || EnemyRunnings[i].Statement != 1)
                {
                    x = 0;
                }

                //draw
                g.DrawImage(EnemyInfoList[EnemyRunnings[i].CharaType].Image[x + EnemyRunnings[i].Statement - 1], new Rectangle(EnemyRunnings[i].X, 474 - 5 * i + 60 - EnemyInfoList[EnemyRunnings[i].CharaType].Height - EnemyRunnings[i].Y, EnemyInfoList[EnemyRunnings[i].CharaType].Width, EnemyInfoList[EnemyRunnings[i].CharaType].Height),
                 0, 0, EnemyInfoList[EnemyRunnings[i].CharaType].Image[x + EnemyRunnings[i].Statement - 1].Width, EnemyInfoList[EnemyRunnings[i].CharaType].Image[x + EnemyRunnings[i].Statement - 1].Height, GraphicsUnit.Pixel, ia);


                g.DrawImage(Shadow_img, new Rectangle(EnemyRunnings[i].X + 20, 474 - 5 * i + 60, EnemyInfoList[EnemyRunnings[i].CharaType].Width - 40, 4),
                 0, 0, Shadow_img.Width, Shadow_img.Height, GraphicsUnit.Pixel, ia);

                //count
                EnemyRunnings[i].Times--;
                EnemyRunnings[i].AttackInterval--;


                //death
                if (EnemyRunnings[i].HP <= 0)
                {
                    if(EnemyRunnings[i].Statement != 5)
                    {
                        EnemyRunnings[i].Statement = 5;
                        EnemyRunnings[i].Times = 20;

                        EnemyRunnings[i].DamageFlag[1] = false;

                    }
                }
               
            }

            for (int i = CharaRunnings.Count - 1; i >= 0; i--)
            {
                //marker
                int x = 0;
                if (MainGameFrontBack == 1)
                {
                    x = 1;
                }

                if (CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width <= 180 || CharaRunnings[i].Statement != 1)
                {
                    x = 0;
                }

                //syoutotsu hanntei 
                for (int j = 0; j < EnemyRunnings.Count; j++)
                {
                    if (CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width - EnemyRunnings[j].X <= 100)
                    {
                        x = 0;
                        CharaRunnings[i].X += CharaInfoList[CharaRunnings[i].CharaType].Speed;

                        if (CharaRunnings[i].AttackInterval <= 0 && CharaRunnings[i].Statement == 1)
                        {
                            CharaRunnings[i].Statement = 3;
                            CharaRunnings[i].Times = 10;
                        }

                        
                    }
                }
                
                //tower attack
                if (CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width <= 180 && CharaRunnings[i].Statement == 1 && CharaRunnings[i].AttackInterval <= 0)
                {
                    CharaRunnings[i].Statement = 3;
                    CharaRunnings[i].Times = 10;
                }


                //update
                if (CharaRunnings[i].Statement == 5)
                {
                    CharaRunnings[i].X += 8;

                    if(CharaRunnings[i].Y >= 20)
                    {
                        CharaRunnings[i].Direction = -3;
                    }
                    else if( CharaRunnings[i].Y <= 0)
                    {
                        CharaRunnings[i].Direction = 3;
                    }

                    CharaRunnings[i].Y += CharaRunnings[i].Direction;

                    if (CharaRunnings[i].DamageFlag[1] == false && CharaRunnings[i].Times == 0)
                    {
                        CharaRunnings.RemoveAt(i);
                        continue;
                    }

                    if (CharaRunnings[i].Times == 0)
                    {
                        CharaRunnings[i].Statement = 1;
                        CharaRunnings[i].AttackInterval = CharaRunnings[i].StaticAttackInterval;
                        CharaRunnings[i].Y = 0;
                    }

                }


                if (CharaRunnings[i].Statement != 1 && CharaRunnings[i].Times == 0)
                {
                    CharaRunnings[i].Statement++;
                    CharaRunnings[i].Times = 10;

                    if (CharaRunnings[i].Statement == 5)
                    {
                        CharaRunnings[i].Statement = 1;
                        CharaRunnings[i].AttackInterval = CharaRunnings[i].StaticAttackInterval;
                    }
                    if (CharaRunnings[i].Statement == 4 && CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width <= 180)
                    {
                        EnemyNowTowerStrength -= CharaInfoList[CharaRunnings[i].CharaType].Attack;
                    }
                    if(CharaRunnings[i].Statement == 4)
                    {
                        PlayBackSound(SoundPath + "CharAttacked.wav");
                    }

                    for (int j = 0; j < EnemyRunnings.Count; j++)
                    {
                        if (CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width - EnemyRunnings[j].X <= 110)
                        {
                            EnemyRunnings[j].HP -= CharaInfoList[CharaRunnings[i].CharaType].Attack;
                        }
                    }

                }

                if (CharaRunnings[i].HP - CharaInfoList[CharaRunnings[i].CharaType].HP / 2 <= 0 && CharaRunnings[i].DamageFlag[0] == true)
                {
                    CharaRunnings[i].DamageFlag[0] = false;

                    CharaRunnings[i].Statement = 5;
                    CharaRunnings[i].Times = 20;
                }



                //explosion
                if (CharaRunnings[i].Statement == 4 && CharaRunnings[i].Times > 5)
                {
                    g.DrawImage(Explosion_img, new Rectangle(CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width - 20 - 70, 474 - 10 * i + 60 - CharaInfoList[CharaRunnings[i].CharaType].Height - 30, 120, 120),
             0, 0, Explosion_img.Width, Explosion_img.Height, GraphicsUnit.Pixel, ia);

                }
                
                //walking
                if (CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width > 180)
                {
                    CharaRunnings[i].X -= CharaInfoList[CharaRunnings[i].CharaType].Speed;
                }

                
                //draw
                g.DrawImage(CharaInfoList[CharaRunnings[i].CharaType].Image[x + CharaRunnings[i].Statement - 1], new Rectangle(CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width, 474 - 5 * i + 60 - CharaInfoList[CharaRunnings[i].CharaType].Height - CharaRunnings[i].Y, CharaInfoList[CharaRunnings[i].CharaType].Width, CharaInfoList[CharaRunnings[i].CharaType].Height),
                 0, 0, CharaInfoList[CharaRunnings[i].CharaType].Image[x + CharaRunnings[i].Statement - 1].Width, CharaInfoList[CharaRunnings[i].CharaType].Image[x + CharaRunnings[i].Statement - 1].Height, GraphicsUnit.Pixel, ia);


                g.DrawImage(Shadow_img, new Rectangle(CharaRunnings[i].X - CharaInfoList[CharaRunnings[i].CharaType].Width + 20 , 474 - 5 * i + 60, CharaInfoList[CharaRunnings[i].CharaType].Width - 40, 4),
                 0, 0, Shadow_img.Width, Shadow_img.Height, GraphicsUnit.Pixel, ia);


                CharaRunnings[i].Times--;
                CharaRunnings[i].AttackInterval--;

                //death
                if (CharaRunnings[i].HP <= 0)
                {
                    if(CharaRunnings[i].Statement != 5)
                    {
                        CharaRunnings[i].Statement = 5;
                        CharaRunnings[i].Times = 20;

                        CharaRunnings[i].DamageFlag[1] = false;

                    }
                }

            }

            if (MainGameFrame % MoneyUpSpeed == 0)
            {
                NowMoney++;
                if (NowMoney >= MaxMoney)
                {
                    NowMoney = MaxMoney;
                }
            }
            MainGameFrame++;
            if (MainGameFrame % MoneyUpSpeed == 0)
            {
                NowMoney++;
                if (NowMoney >= MaxMoney)
                {
                    NowMoney = MaxMoney;
                }
            }

            NowTowerAttackInterval--;

            if(NowTowerAttackInterval <= 0 && TowerAttackScene == 0)
            {
                TowerAttackScene = 1;
            }
            
            if(TowerAttackScene == 1 && MainGameFrontBack == 1)
            {
                g.DrawImage(TowerAttack_img, new Rectangle(1146,594, 194, 165),
                 0, 0, TowerAttack_img.Width, TowerAttack_img.Height, GraphicsUnit.Pixel, ia);
            }
            else if (TowerAttackScene == 1)
            {
                g.DrawImage(WhiteTowerAttack_img, new Rectangle(1146, 594, 194, 165),
                 0, 0, TowerAttack_img.Width, TowerAttack_img.Height, GraphicsUnit.Pixel, ia);
            }
            
            if( NowMoney >= MoneyUpCost && LevelUpScene == 0)
            {
		LevelUpScene = 1;
	    }
	    if(NowMoney < MoneyUpCost)
	    {
		LevelUpScene = 0;
	    }
	    
	    if(LevelUpScene == 1 && MainGameFrontBack == 1)
	    {
	        g.DrawImage(LevelUp_img, new Rectangle(0, 629, 178, 122),
                     0, 0, LevelUp_img.Width, LevelUp_img.Height, GraphicsUnit.Pixel, ia);
            	
	    }
	    else if(LevelUpScene == 1)
	    {
	        g.DrawImage(WhiteLevelUp_img, new Rectangle(0, 629, 178, 122),
                     0, 0, WhiteLevelUp_img.Width, WhiteLevelUp_img.Height, GraphicsUnit.Pixel, ia);
            	
	    }
	    
	    if(MoneyUpSpeed == 2)
	    {
		LevelUpScene = 2;
	    }
	    if(LevelUpScene == 2)
	    {
		g.DrawImage(LevelUp_img, new Rectangle(0, 629, 178, 122),
                     0, 0, LevelUp_img.Width, LevelUp_img.Height, GraphicsUnit.Pixel, ia);
	    }
	 

            if(TowerAttackScene == 2)
            {
                for (int r = 0; r < EnemyRunnings.Count; r++)
                {  
                    if (EnemyRunnings[r].X > 500)
                    {
                        EnemyRunnings[r].HP -= TowerAttack;
                    }
                }
                TowerAttackScene = 0;
                NowTowerAttackInterval = TowerAttackInterval;

            }
            
            if(MainGameStu == "MainGame")
            {
                using (Font font = new Font("Arial", 18, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect = new Rectangle(1026, 158, 156, 28);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;


                    // Draw the text and the surrounding rectangle.

                    g.DrawString(NowTowerStrength.ToString() + "/" + TowerStrength.ToString(), font, Brushes.Gold, rect, stringFormat);
                }

                using (Font font = new Font("Arial", 18, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect = new Rectangle(120, 250, 156, 28);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;


                    // Draw the text and the surrounding rectangle.

                    g.DrawString(EnemyNowTowerStrength.ToString() + "/" + EnemyTowerStrength.ToString(), font, Brushes.Gold, rect, stringFormat);
                }


                using (Font font = new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect = new Rectangle(1000, 8, 330, 65);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;

                    // Draw the text and the surrounding rectangle.

                    g.DrawString(NowMoney.ToString() + "/" + MaxMoney.ToString() + "円", font, Brushes.Orange, rect, stringFormat);
                }
                
                
                if(LevelUpScene != 2)
                {
			using (Font font = new Font("Arial", 18, FontStyle.Bold, GraphicsUnit.Point))
       		         {
	                    Rectangle rect = new Rectangle(0, 715, 85, 35);

                    	// Create a StringFormat object with the each line of text, and the block
                    	// of text centered on the page.
                 	   StringFormat stringFormat = new StringFormat();
                	    stringFormat.Alignment = StringAlignment.Far;
        	            stringFormat.LineAlignment = StringAlignment.Far;


	                    // Draw the text and the surrounding rectangle.

                   	 g.DrawString(MoneyUpCost.ToString() + "円", font, Brushes.Gold, rect, stringFormat);
                	}
                
			
			using (Font font = new Font("Arial",26 , FontStyle.Bold, GraphicsUnit.Point))
        	        {
                	    Rectangle rect = new Rectangle(80, 600, 30, 40);

                    	// Create a StringFormat object with the each line of text, and the block
                    	// of text centered on the page.
                    	StringFormat stringFormat = new StringFormat();
                    	stringFormat.Alignment = StringAlignment.Far;
                    	stringFormat.LineAlignment = StringAlignment.Far;


                   	 // Draw the text and the surrounding rectangle.

                    	g.DrawString((7 - MoneyUpSpeed).ToString(), font, Brushes.Gray, rect, stringFormat);
	                }

		}
		else if(LevelUpScene == 2)
		{
		    	using (Font font = new Font("Arial",20 , FontStyle.Bold, GraphicsUnit.Point))
        	        {
                	    Rectangle rect = new Rectangle(80, 600, 70, 35);

                    	// Create a StringFormat object with the each line of text, and the block
                    	// of text centered on the page.
                    	StringFormat stringFormat = new StringFormat();
                    	stringFormat.Alignment = StringAlignment.Far;
                    	stringFormat.LineAlignment = StringAlignment.Far;


                   	 // Draw the text and the surrounding rectangle.

                    	g.DrawString("Max", font, Brushes.Red, rect, stringFormat);
	                }

		}
                
            }


            if (MainGameStu == "MainGame")
            {
                if (MainGameMarker)
                {
                    Pen pen = new Pen(Color.Yellow, 8);

                    Rectangle rect1 = new Rectangle(CharaPlaces[OrderedCharas, 0], CharaPlaces[OrderedCharas, 1], CharaPlaces[OrderedCharas, 2], CharaPlaces[OrderedCharas, 3]);

                    e.Graphics.DrawRectangle(pen, rect1);
                }

                if (Extra_Time_Sleep(100) && MainGameMarker)
                {
                    MainGameMarker = false;
                    OrderedCharas = -1;
                }

            }

            if (NowTowerStrength < 0 && MainGameStu == "MainGame")
            {
                MainGameStu = "Lose";
            }

            if (EnemyNowTowerStrength < 0 && MainGameStu == "MainGame")
            {
                MainGameStu = "Win";
            }

            if (MainGameStu == "Win")
            {
                if (Extra_Time_Sleep(3000))
                {
                    MainGameStu = "EndWin";
                }
            }

            if (MainGameStu == "Lose")
            {
                
                if (Extra_Time_Sleep(3000))
                {
                    MainGameStu = "EndLose";
                }
            }

            
            if (MainGameStu == "Lose" || MainGameStu == "EndLose")
            {
                g.DrawImage(Dirt_img, new Rectangle(258, 635, 775, 105),
                0, 0, Dirt_img.Width, Dirt_img.Height, GraphicsUnit.Pixel, ia);

                StopBackSound();
                StopMainSound();

                
                using (Font font = new Font("Arial", 78, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect = new Rectangle(WINDOW_WIDTH / 2 - 260, WINDOW_HEIGHT / 2 - 60, 520, 120);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;


                    // Draw the text and the surrounding rectangle.

                    g.DrawString("敗北...", font, Brushes.Black, rect, stringFormat);
                }
                using (Font font = new Font("Arial", 75, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect = new Rectangle(WINDOW_WIDTH / 2 - 260, WINDOW_HEIGHT / 2 - 60, 520, 120);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;


                    // Draw the text and the surrounding rectangle.

                    g.DrawString("敗北...", font, Brushes.White, rect, stringFormat);
                }

                if (MainGameFrame % 2 == 0)
                {
                    LoseFlagX = rand.Next(1127, 184 + 1127);
                    LoseFlagY = rand.Next(239, 282 + 239);

                }

                if (MainGameStu == "EndLose")
                {
                    if (Time_Sleep(50))
                    {
                        transparency -= 0.05f;
                    }
                }


                g.DrawImage(Explosion_img, new Rectangle(LoseFlagX - 80,LoseFlagY - 80, 120, 120),
             0, 0, Explosion_img.Width, Explosion_img.Height, GraphicsUnit.Pixel, ia);
            }


            if (MainGameStu == "Win" || MainGameStu == "EndWin")
            {
                g.DrawImage(Dirt_img, new Rectangle(258, 635, 775, 105),
                0, 0, Dirt_img.Width, Dirt_img.Height, GraphicsUnit.Pixel, ia);

                StopBackSound();
                StopMainSound();

                
                using (Font font = new Font("Arial", 78, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect = new Rectangle(WINDOW_WIDTH / 2 - 260, WINDOW_HEIGHT / 2 - 60, 520, 120);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;


                    // Draw the text and the surrounding rectangle.

                    g.DrawString("勝利...", font, Brushes.Black, rect, stringFormat);
                }
                using (Font font = new Font("Arial", 75, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect = new Rectangle(WINDOW_WIDTH / 2 - 260, WINDOW_HEIGHT / 2 - 60, 520, 120);

                    // Create a StringFormat object with the each line of text, and the block
                    // of text centered on the page.
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;


                    // Draw the text and the surrounding rectangle.

                    g.DrawString("勝利...", font, Brushes.White, rect, stringFormat);
                }

                if (MainGameFrame % 2 == 0)
                {
                    LoseFlagX = rand.Next(8, 121 + 8);
                    LoseFlagY = rand.Next(220, 302 + 220);

                }

                if (MainGameStu == "EndWin")
                {
                    if (Time_Sleep(50))
                    {
                        transparency -= 0.05f;
                    }
                }


                g.DrawImage(Explosion_img, new Rectangle(LoseFlagX - 80,LoseFlagY - 80, 120, 120),
             0, 0, Explosion_img.Width, Explosion_img.Height, GraphicsUnit.Pixel, ia);
            }

            if(transparency <= 0)
            {
                MainGameStu = "WaitNext";
            }

            if(MainGameStu == "WaitNext")
            {
                FinishMainGame();
            }

            MainGameFrame++;
            

        }
        else if (mScene == 6)
        {
            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

            ia = FadeOut(transparency);

            g.DrawImage(UpGrade_img, new Rectangle(0, 0, UpGrade_img.Width, UpGrade_img.Height),
                 0, 0, UpGrade_img.Width, UpGrade_img.Height, GraphicsUnit.Pixel, ia);

            transparency += 0.1f;

            int[,] FlamePlaces = new int[,]
            {
                { -107,188, 260,213},
                { 182,188, 260,213},
                { 477,100, 377,302}, 
                { 884,192, 260,213},
                { 1177,190 ,260,213}
            };
            int[,] ImagePlaces = new int[,]
            {
                { -105,224, 109,94},
                { 188,226, 109,94},
                { 486,148, 156,135},
                { 888,230, 109,94},
                { 1183,222 ,109,94}
            };
            int[,] NamePlaces = new int[,]
            {
                { -107,188, 250,31},
                { 182,188, 250,31},
                { 477,100, 361,47},
                { 884,192, 250,31},
                { 1177,190 ,250,31}
            };
            int[,] LevelPlaces = new int[,]
            {
                { 85,287, 58,31},
                { 373,281, 58,31},
                { 752,241, 85,37},
                { 1074,293, 58,31},
                { 1600,190 ,58,31}
            };
            int[,] XpPlaces = new int[,]
            {
                { 41,335, 101,34},
                { 330,333, 101,34},
                { 692,304, 151,48},
                { 1032,334, 101,34},
                { 1600,190 ,101,34}
            };

            int[,] OkPlaces = new int[,]
            {
                { 65,226, 60,60},
                { 349,227, 60,60},
                { 714,151,90 ,90},
                { 1053,230, 60,60},
                { 1600,190 ,60,60}
            };

            int[] FontSize = new int[] { 20, 20, 30, 20 ,20};

            string[] Comments = new string[] {"EXキャラクター達のパワーアップ画面に移動します",
                "攻撃力がアップします", "射程処理がアップします", "発射可能になるまでのチャージ時間が速くなります",
                "自分の城の耐久力がアップします",
                "プレイヤーの仕事効率を上げ\nお金の増加スピードが速くなります", "敵を倒すたびに得られるお金をアップさせることができます", 
                "お財布のサイズをアップさせ\n戦闘中に所持できるお金が増えます" };


            // XML読み込み
            XElement xml = XElement.Load(CharaPath + "normal/info.xml");

            // userを取得
            IEnumerable<XElement> members = from item in xml.Elements("member") select item;

            
            var images = new Image[]{ extrachara_img, CannonStrength_img, CannonLength_img, CannonCharge_img, CannonEndurance_img, WorkAmount_img, TotalUp_img, Wallet_img };
            
            if (mScene6LoadFlag)
            {
                CharaList.Clear();

                CharaList.Add("S");
                CharaList.Add("S");


                CharaList.Add("A");

                int temporary = 0;
                foreach (var member in members)
                {
                    Image li = Image.FromFile(CharaPath + "normal/" + member.Element("imagePath").Value);
                    UpgradeCharaImage.Add(li);

                    CharaList.Add(temporary.ToString());

                    temporary++;
                }

                CharaList.AddRange(new List<string>() { "B", "C", "D", "E", "F", "G", "H", "A", "S", "S" });

                mScene6LoadFlag = false;
  
            }

            
            var Framework = CharaList.GetRange(UpGrade_Cursor, 5);

            byte count = 0;

            Font font = new Font("Meiryo UI", FontSize[count], FontStyle.Bold, GraphicsUnit.Point);
            Rectangle rect4 = new Rectangle(67, 534, 1258, 143);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            string[] patterns = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

            
            foreach (string framework in Framework)
            {
                
                //Console.Write(framework);
                for (int i = 0; i < patterns.Length; i++)
                {
                    if (framework == patterns[i])
                    {
                        g.DrawImage(images[i], new Rectangle(FlamePlaces[count, 0], FlamePlaces[count, 1], FlamePlaces[count, 2], FlamePlaces[count, 3]),
                 0, 0, images[i].Width, images[i].Height, GraphicsUnit.Pixel, ia);

                        if (count == 2)
                        {
                            g.DrawString(Comments[i], font, Brushes.White, rect4, stringFormat);
                        }
                    }
                }

                
                int coun = 0;
                
                foreach (var member in members)
                {
                    int i = 0; 
                    if (int.TryParse(framework,out i))
                    {
                        if (coun == int.Parse(framework))
                        {
                            font = new Font("Meiryo UI", FontSize[count], FontStyle.Bold, GraphicsUnit.Point);

                            g.DrawImage(charaflame_img, new Rectangle(FlamePlaces[count, 0], FlamePlaces[count, 1], FlamePlaces[count, 2], FlamePlaces[count, 3]),
                            0, 0, charaflame_img.Width, charaflame_img.Height, GraphicsUnit.Pixel, ia);

                            g.DrawImage(UpgradeCharaImage[coun], new Rectangle(ImagePlaces[count, 0], ImagePlaces[count, 1], ImagePlaces[count,2], ImagePlaces[count,3]),
                             0, 0, UpgradeCharaImage[coun].Width, UpgradeCharaImage[coun].Height, GraphicsUnit.Pixel, ia);

                          
                            Rectangle rect1 = new Rectangle(NamePlaces[count, 0], NamePlaces[count, 1], NamePlaces[count, 2], NamePlaces[count, 3]);
                            Rectangle rect2 = new Rectangle(LevelPlaces[count, 0] , LevelPlaces[count, 1] , LevelPlaces[count, 2], LevelPlaces[count, 3]);
                            Rectangle rect3 = new Rectangle(XpPlaces[count, 0] , XpPlaces[count, 1] , XpPlaces[count, 2], XpPlaces[count, 3]);
                            
                            
                            // Draw the text and the surrounding rectangle.
                            g.DrawString(member.Element("name").Value, font, Brushes.White, rect1, stringFormat);
                            g.DrawString(member.Element("level").Value, font, Brushes.Gold, rect2, stringFormat);
                            g.DrawString(member.Element("xp").Value, font, Brushes.Gold, rect3, stringFormat);

                            if (count == 2)
                            {
                                font = new Font("Meiryo UI", 20, FontStyle.Bold, GraphicsUnit.Point);

                                g.DrawString(member.Element("comment").Value, font, Brushes.White, rect4, stringFormat);
                            }

                            if (member.Element("canUse").Value == "true")
                            {
                                g.DrawImage(okmark_img, new Rectangle(OkPlaces[count, 0], OkPlaces[count, 1], OkPlaces[count, 2], OkPlaces[count, 3]),
                           0, 0, okmark_img.Width, okmark_img.Height, GraphicsUnit.Pixel, ia);

                            }

                        }
                        coun++;
                    }
                }
                
                count++;
            }
            
            if (Flame_Cursor == 0)
            {
                Pen pen;
                if (Flame_Color == 1)
                {
                    pen = new Pen(Color.Yellow, 8);
                }
                else
                {
                    pen = new Pen(Color.DeepPink, 8);
                }

                Rectangle rectX = new Rectangle(477, 100, 377, 302);

                e.Graphics.DrawRectangle(pen, rectX);
            }
            if (Flame_Cursor == 1)
            {
                Pen pen;
                if (Flame_Color == 1)
                {
                    pen = new Pen(Color.Yellow, 8);
                }
                else
                {
                    pen = new Pen(Color.DeepPink, 8);
                }

                Rectangle rectX = new Rectangle(4, 634, 114, 114);

                e.Graphics.DrawRectangle(pen, rectX);
            }
        }
    }
    

    public System.Drawing.Imaging.ImageAttributes FadeOut(float transparency)
    {
        //ColorMatrixオブジェクトの作成
        System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();
        //ColorMatrixの行列の値を変更して、アルファ値が0.5に変更されるようにする
        cm.Matrix00 = 1;
        cm.Matrix11 = 1;
        cm.Matrix22 = 1;
        cm.Matrix33 = transparency;
        cm.Matrix44 = 1;

        //ImageAttributesオブジェクトの作成
        System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
        //ColorMatrixを設定する
        ia.SetColorMatrix(cm);

        //ImageAttributesを使用して画像を描画
        return ia;
       
    }
    void PlayMainSound(string sourcepath)
    {
        if (MainSoundFlag)
        {
            MainMediaPlayer.controls.stop();
            MainMediaPlayer.settings.setMode("loop", true);
            MainMediaPlayer.URL = sourcepath;
            MainSoundFlag = false;
        }
    }
    void PlayBackSound(string sourcepath)
    {
        if (BackSoundFlag)
        {
            player = new System.Media.SoundPlayer(sourcepath);
            player.Play();
        }
    }
    void StopMainSound()
    {
        MainMediaPlayer.controls.stop();
    }
    void StopBackSound()
    {
        player.Stop();
    }
    void ButtonPushed()
    {
        BackMediaPlayer.controls.stop();
        BackMediaPlayer.URL = SoundPath + "ButtonPushed.wav";
        //BackMediaPlayer.controls.play();
    }

    public bool Time_Sleep(int sleeptime)
    {
        TimeSpan ts = sw.Elapsed;

        if (ts.TotalMilliseconds == 0)
        {
            sw.Start();
        }
        else if (ts.TotalMilliseconds > sleeptime)
        {
            sw.Reset();
            return true;
        }
        return false;
    }

    public bool Extra_Time_Sleep(int sleeptime)
    {
        TimeSpan ts = extra_sw.Elapsed;

        if (ts.TotalMilliseconds == 0)
        {
            extra_sw.Start();
        }
        else if (ts.TotalMilliseconds > sleeptime)
        {
            extra_sw.Reset();
            return true;
        }
        return false;
    }

    static void InfoLoad()
    {
        string[] Paths = Directory.GetDirectories(MapPath, "*");
        

        foreach(string path in Paths)
        {
            string[] tem = path.Split('/');
            PlaceNames.Add(tem[tem.Length - 1 ]);
        }

        // XML読み込み
        XElement xml = XElement.Load(CharaPath + "normal/info.xml");

        // userを取得
        IEnumerable<XElement> members = from item in xml.Elements("member") select item;

        foreach (var member in members)
        {
            NormalCharaLength++;
        }

    }

    static void FinishMainGame()
    {
        transparency = 0;
        mScene = 2;
        Flame_Cursor = 0;
        MainSoundFlag = true;
        DashAnimationScene = -1;

        MapFirstFlag = true;
        MapRound_Cursor = 0;
        Map_Cursor = 0;
        MapList.Clear();

        BeforeMainGameFrame = 0;

        MainGameStu = "MainGame";
        MainGameLoadFlag = true;
        TowerAttackScene = 0;
       
        NowMoney = 0;

        CharaInfoList.Clear();
        CharaRunnings.Clear();

        EnemyInfoList.Clear();
        EnemyRunnings.Clear();
    }

    public class EnemyInfo
    {
        public int Width;
        public int Height;
        public int HP;
        public int Speed;
        public int Attack;
        public int FirstWaitingTime;
        public int WaitingTime;
        public int StaticWaitingTime;
        public int AttackInterval;
        public Image[] Image = new Image[5];
        public string Type;
    }

    public class EnemyRunning
    {
        public int X;
        public int Y;
        public int Direction;
        public int FirstWaitingTime;
        public int HP;
        public int AttackInterval;
        public int StaticAttackInterval;
        public int Statement;
        public int CharaType;
        public int Times;
        public bool[] DamageFlag = new bool[2];
    }

    public class CharaInfo
    {
        public int Width;
        public int Height;
        public int HP;
        public int Speed;
        public int Attack;
        public int FirstWaitingTime;
        public int WaitingTime;
        public int AttackInterval;
        public Image[] Image = new Image[5];
        public string Type;
    }

    public class CharaRunning
    {
        public int X;
        public int Y;
        public int Direction;
        public int HP;
        public int AttackInterval;
        public int StaticAttackInterval;
        public int Statement;
        public int CharaType;
        public int Times;
        public bool[] DamageFlag = new bool[2];
    }
}