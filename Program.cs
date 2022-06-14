using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program : Form
{
    const int WINDOW_WIDTH = 1334;
    const int WINDOW_HEIGHT = 750;
    const int FONT_SIZE = 14;
    const string ImagePath = "./images/";
    const string SoundPath = "./sounds/";
    static bool StartUpFlag = true;
    const int FPS = 30;
    static bool MusicSoundFlag = true;
    //static bool ImageFlag = true;
    string SoundFile ;
    static int Flame_Color = 1;
    static int Flame_Cursor = 0;
    static int CommentCount = 0;
    static List<string> Comments = new List<string>();

    
    static bool DashAnimationFlag = false;

    static Image StartUp_0_img = Image.FromFile(ImagePath + "StartUp_0.png");
    static Image StartUp_1_img = Image.FromFile(ImagePath + "StartUp_1.png");
    static Image Dashboard_img = Image.FromFile(ImagePath + "Dashboard.png");
    static Image Dash_img = Image.FromFile(ImagePath + "Dash.png");
    
    static float transparency = 0;
    static int StartUpTextLocation = 0;

    Random rand = new Random();

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    static byte mScene = 0; //0:起動画面 1:スタート画面 2:ダッシュボード 3:日本地図 4:戦闘画面（メイン） 5：戦闘終了画面

    protected override void OnKeyUp(KeyEventArgs e)
    {
        var c = e.KeyCode;
        Console.WriteLine("KEYUP:" + c);

        if (mScene == 0 && !StartUpFlag && e.KeyCode == Keys.Return)
        {
            MusicSoundFlag = true;
            PushedButton();
            mScene = 1;
            
            return;
        }
        if (mScene == 1)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (Flame_Cursor == 0)
                {
                    string[] lines = File.ReadAllLines("Comments.txt");

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
                    PushedButton();
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
                    DashAnimationFlag = true;

                }
            }

            if (Flame_Cursor == 4 && e.KeyCode == Keys.Return)
            {
                Flame_Cursor = 0;
                MusicSoundFlag = true;
                mScene = 1;
                PushedButton();
            }
            return;
        }
        if (mScene == 3)
        {
            return;
        }

    }
    protected override void OnKeyDown( KeyEventArgs e )
    {

        var c = e.KeyCode;
        Console.WriteLine("KEYDOWM:"+c);

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
                    Flame_Cursor = 4;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                Flame_Cursor++;
                if (Flame_Cursor >= 5)
                {
                    Flame_Cursor = 0;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                Flame_Cursor = 5;
            }
            else if (e.KeyCode == Keys.Left)
            {
                Flame_Cursor = 0;
            }
            if (Flame_Cursor == 5 && e.KeyCode == Keys.Return)
            {
                CommentCount++;
                if (CommentCount > Comments.Count - 1)
                {
                    CommentCount = 0;
                }
                SoundFile = SoundPath + "Comment.wav";
                FreePlaySound();
            }


            return;
        }
    }
    
    public static void Main(string[] args)
    {
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
            if(DashAnimationFlag)
            {
                if (Time_Sleep(200))
                {
                    Console.WriteLine("hello");
                }
            }
        }
    }
    
    public bool Time_Sleep(int sleeptime )
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

    
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        if (mScene == 0 )
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
                SoundFile = SoundPath + "startlog.wav";
                LoopPlay();
       
                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

                ia = FadeOut(transparency);

                g.DrawImage(StartUp_1_img, new Rectangle(0, 0, StartUp_1_img.Width, StartUp_1_img.Height), 0, 0, StartUp_1_img.Width, StartUp_1_img.Height, GraphicsUnit.Pixel, ia);

                transparency += 0.1f;
                
                string[] lines = File.ReadAllLines("StartUpText.txt");
                string text = String.Join("\n", lines);
                using (Font font = new Font("Meiryo UI", 24, FontStyle.Bold, GraphicsUnit.Point))
                {

                    
                    Rectangle rect = new Rectangle(WINDOW_WIDTH / 2 - 500 / 2 , WINDOW_HEIGHT - StartUpTextLocation, 600, 1000000000);

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
            SoundFile = SoundPath + "start.wav";
            LoopPlay();

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
            
            Rectangle rect1 = new Rectangle(places[Flame_Cursor, 0],places[Flame_Cursor, 1],places[Flame_Cursor, 2],places[Flame_Cursor, 3]);
            
            e.Graphics.DrawRectangle(pen, rect1);

            transparency += 0.1f;
        }
        
        else if (mScene == 2 ) 
        {
            System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();

            ia = FadeOut(transparency);

            g.DrawImage(Dash_img, new Rectangle(0, 0, Dash_img.Width, Dash_img.Height),
                 0, 0, Dash_img.Width, Dash_img.Height, GraphicsUnit.Pixel, ia);

	        transparency += 0.1f;

            int[,] places = new int[,]
            {
                { 28,92, 438,75},
                { 28,199, 440,75},
                { 40,312, 125,103},
                { 187,300, 121,116},
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


                Rectangle rect = new Rectangle(720,150, 561,260);

                // Create a StringFormat object with the each line of text, and the block
                // of text centered on the page.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;


                // Draw the text and the surrounding rectangle.

                g.DrawString(text, font, Brushes.White, rect, stringFormat);
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

    private System.Media.SoundPlayer player = null;
    
    private void StopSound()
    {
        if (player != null)
        {
            player.Stop();
            player.Dispose();
            player = null;
        }
    }

    private void PlaySound()
    {
        if (MusicSoundFlag)
        {
            player = new System.Media.SoundPlayer(SoundFile);
            player.Play();
            MusicSoundFlag = false;
        }
    }
    private void FreePlaySound()
    {
            player = new System.Media.SoundPlayer(SoundFile);
            player.Play();
            MusicSoundFlag = false;
        
    }
    private void LoopPlay()
    {
        if (MusicSoundFlag)
        {
            player = new System.Media.SoundPlayer(SoundFile);
            player.PlayLooping();
            MusicSoundFlag = false;
        }
    }
    private void PushedButton()
    {
        player = new System.Media.SoundPlayer(SoundPath + "ButtonPushed.wav");
        player.Play();
    }

}