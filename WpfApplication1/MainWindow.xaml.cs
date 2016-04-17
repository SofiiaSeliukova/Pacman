using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.IO;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        private List<Image> gameCharacterImages;
        private string[] gameCharacterImagesPath = {"inky.png","blinky.png","pinky.png","clyde.png","pacman.png","blueghost.png","eyes.png"};
        private BitmapImage blueGhostBmp;
        private BitmapImage fruitBmp;
        private BitmapImage ghostEyesBmp;
        private List<UIElement> pellets;
        private Field field;
        private DispatcherTimer moveTimer;
        private DispatcherTimer wanderingModeTimer;
        private DispatcherTimer huntingModeTimer;
        private DispatcherTimer escapingModeTimer;
        private EnterName enterNameWindow;
        private LoadLibraries loadLibraryWindow;
        private static List<IGhostsMovingAlgorithm> Plugins { get; set; }
        private static List<string> PluginsNames { get; set; }
       
        public MainWindow()
        {
           
            InitializeComponent();
            Loaded += (x, y) => Keyboard.Focus(GameField);
            enterNameWindow = new EnterName();
            enterNameWindow.ShowDialog();
            Plugins = new List<IGhostsMovingAlgorithm>();
            PluginsNames = new List<string>();
            LoadPlugins();
            if (Plugins.Count != 0)
            {
                loadLibraryWindow = new LoadLibraries();

                loadLibraryWindow.Libraries.DataContext = Plugins.Select(x => new { Value = x.GetType().ToString() }).ToList();
                loadLibraryWindow.ShowDialog();
                

            }

            pellets = new List<UIElement>();
            gameCharacterImages = new List<Image>(5);
            
            moveTimer = new DispatcherTimer();
            wanderingModeTimer = new DispatcherTimer();
            huntingModeTimer = new DispatcherTimer();
            escapingModeTimer = new DispatcherTimer();
            escapingModeTimer.Interval = new TimeSpan(0,0,5);
           
            wanderingModeTimer.Interval = new TimeSpan(0, 0, 7);
            huntingModeTimer.Interval = new TimeSpan(0, 0, 20);
            moveTimer.Interval = new TimeSpan(0,0,0,0,300);
           
            wanderingModeTimer.Tick += new System.EventHandler(this.wndMode_Tick);
            huntingModeTimer.Tick += new System.EventHandler(this.hntMode_Tick);
            escapingModeTimer.Tick += new System.EventHandler(this.escMode_Tick);
            moveTimer.Tick += new System.EventHandler(this.tmrMove_Tick);
            Score.Text = "";

            SetImages();
            StartGame();

            
            
        }

        private void SetImages()
        {
            for (int i = 0; i < 5;i++ )
            {
                Image img = new Image ();
                BitmapImage bmpimg = new BitmapImage();
                bmpimg.BeginInit();
                Uri uri = new Uri(@"Resources\" + gameCharacterImagesPath[i], UriKind.Relative);
                PngBitmapDecoder decoder = new PngBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                bmpimg.UriSource = uri;
                bmpimg.EndInit();
                img.Source = bmpimg;
                gameCharacterImages.Add(img);

            }
            
            blueGhostBmp = new BitmapImage();
            blueGhostBmp.BeginInit();
            Uri uri2 = new Uri(@"Resources\" + gameCharacterImagesPath[5], UriKind.Relative);
            PngBitmapDecoder decoder2 = new PngBitmapDecoder(uri2, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            blueGhostBmp.UriSource = uri2;
            blueGhostBmp.EndInit();

            ghostEyesBmp = new BitmapImage();
            ghostEyesBmp.BeginInit();
            Uri uri3 = new Uri(@"Resources\" + gameCharacterImagesPath[6], UriKind.Relative);
            PngBitmapDecoder decoder3 = new PngBitmapDecoder(uri3, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            ghostEyesBmp.UriSource = uri3;
            ghostEyesBmp.EndInit();
           
          }

        private void escMode_Tick(object sender, EventArgs e)
        {
            escapingModeTimer.Interval = new TimeSpan(0, 0, 5);
           
            field.HuntingMode();
            huntingModeTimer.Start();
            escapingModeTimer.Stop();
            
        }

        private void hntMode_Tick(object sender, EventArgs e)
        {
            field.WanderingMode();
            
            
            huntingModeTimer.Stop();
            wanderingModeTimer.Start();
        }

        private void wndMode_Tick(object sender, EventArgs e)
        {
           field.HuntingMode();
           
            
            wanderingModeTimer.Stop();
            huntingModeTimer.Start();
        }

        private void tmrMove_Tick(object sender, EventArgs e)
        {
          
          field.MoveCharacters();
        }

        void GameCharacter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Redraw(GameField);
            
        }

        public  void DrawDot(Brush brush, Dot dot)
        {
            int x, y;
            UIElement uielem;
             if (dot as Fruit == null)
             {
                 uielem = new Ellipse();
                 ((Ellipse)uielem).Fill = brush;
                 ((Ellipse)uielem).Width = dot.Width;
                 ((Ellipse)uielem).Height = dot.Height;
                
                 if (dot as BigDot == null)
                 {
                     y = dot.CoordY * field.Cols + dot.Width / 2;
                     x = dot.CoordX * field.Rows + dot.Height / 2;
                 }
                 else
                 {
                     y = dot.CoordY * field.Cols + dot.Width / 4;
                     x = dot.CoordX * field.Rows + dot.Height / 4;
                 }

           
             }
             else
             {
                 uielem = new Image();
                 ((Image)uielem).Width = dot.Width;
                 ((Image)uielem).Height = dot.Height;
                 y = dot.CoordY * field.Cols + dot.Width/6;
                 x = dot.CoordX * field.Rows + dot.Height/6;
                 Canvas.SetTop(uielem, dot.CoordY * field.Cols);
                 Canvas.SetLeft(uielem, dot.CoordX * field.Rows);
                 ((Image)uielem).Source = fruitBmp;
                
             }
          
                 Canvas.SetTop(uielem, y);
                 Canvas.SetLeft(uielem, x);
            
             pellets.Add(uielem);
            
         }

        private string GetPathToFruitImageByType(FruitType fruitType)
        {
            switch (fruitType)
            {
                case (FruitType.Apple):
                    {
                        return @"Resources\apple.png";
                         
                    }
                case (FruitType.Cherry):
                    {
                        return @"Resources\cherry.png";
                        
                    }
                case (FruitType.Strawberry):
                    {
                        return @"Resources\strawberry.png";
                        
                    }
                case (FruitType.Orange):
                    {
                        return @"Resources\orange.png";
                        
                    }
             case (FruitType.Melon):
                    {
                        return @"Resources\melon.png";
                       
                    }
                default: return String.Empty; 
            }
        }
       
        public  void DrawGameCharacter (Canvas canvas, int i,GameCharacter gamecharacter)
        {
            gameCharacterImages[i].Width = gamecharacter.Width;
            gameCharacterImages[i].Height = gamecharacter.Height;
            Canvas.SetTop(gameCharacterImages[i], gamecharacter.CurrentPoint.CoordY * field.Cols);
            Canvas.SetLeft(gameCharacterImages[i], gamecharacter.CurrentPoint.CoordX * field.Rows);
            canvas.Children.Add(gameCharacterImages[i]);
           }

        public  void DrawTile(Canvas canvas, bool setTop, bool setLeft, Brush brush, Tile tile)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = brush;
            rectangle.StrokeThickness = 2;  
            rectangle.Width = tile.TileHeight;
            rectangle.Height = tile.TileWidth;
            if (setTop)
            {
                Canvas.SetTop(rectangle, tile.CoordY * field.Cols);

            }
            else
            {
                Canvas.SetBottom(rectangle, tile.CoordY * field.Cols);

            }
            if (setLeft)
            {

                Canvas.SetLeft(rectangle, tile.CoordX * field.Rows);
            }
            else
            {
                    Canvas.SetRight(rectangle, tile.CoordX * field.Rows);
            }
            canvas.Children.Add(rectangle);
        }
       
        public  void DrawDots(Canvas canvas)
        {
          
                foreach (Dot d in field.Dots)
                    DrawDot(Brushes.White, d);
                foreach (UIElement e in pellets)
                    canvas.Children.Add(e);
        }
       
        public  void DrawWalls(Canvas canvas)
        {
            for (int i = 0; i < field.Rows; i++)
            {
                for (int j = 0; j < field.Cols; j++)
                {
                    if (field.MatrixForWalls[i, j] == 1 || field.MatrixForWalls[i, j] == 2)
                    {
                        DrawTile(canvas, true, true, Brushes.Blue, new Tile(i, j));
                    }
                }
            }
        }
       
        public  void Redraw(Canvas canvas)
        {
            foreach (UIElement e in pellets)
                canvas.Children.Remove(e);
            foreach (Image e in gameCharacterImages)
                canvas.Children.Remove(e);
            pellets.Clear();
            DrawDots(canvas);
            DrawCharacters(canvas);
        }

        private void StartGame()
        {
            GameField.Children.Clear();
            pellets.Clear();

            field = new Field(20, 20, enterNameWindow.playerName);
            Lives.Text = "Lives : " + field.GetLives().ToString();
            Levels.Text = "Level : " + field.GetLevelNumber().ToString();
            PlayerName.Text = "Player : " + field.GetPlayerName();
            Score.Text = field.GetScore().ToString();

            field.PropertyChanged += Level_PropertyChanged;
            field.PropertyChanged += Score_PropertyChanged;
            field.PropertyChanged += Mode_PropertyChanged;
            field.PropertyChanged += Lives_PropertyChanged;
            field.PropertyChanged += FreeLives_PropertyChanged;
            field.PropertyChanged += GameOver_PropertyChanged;
            field.PropertyChanged += GameCharacter_PropertyChanged;

            field.GenerateWalls();
            field.GenerateMaze();
            field.GenerateDots();

            field.InitializePath();
            field.InitializeGhostPath();
            if (loadLibraryWindow.selectedIndex != null)
                field.InitializeCharacters(Plugins[(int)loadLibraryWindow.selectedIndex]);
            else
                field.InitializeCharacters();

            NewGamePreparation();
        
        }

        private void FreeLives_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FreeLives")
            {
                FreeLives.Text = "Free Lives : " + ((Player)sender).FreeLives;
            }
            else if (e.PropertyName == "FreeLivesDown")
            {
                AnotherAttempt();
                FreeLives.Text = "Free Lives : " + ((Player)sender).FreeLives;
            }
        }
       
        private void NextLevelGame()
        {
            StopAll();
            GameField.Children.Clear();
            pellets.Clear();
            field.ResetCharacter();
            field.GenerateWalls();
            field.GenerateMaze();
            field.GenerateDots();
            NewGamePreparation();
         
        }
       private void NewGamePreparation()
        {

            Lives.Text = "Lives : " + field.GetLives().ToString();
            Levels.Text = "Level :" + field.GetLevelNumber().ToString();
            Score.Text = field.GetScore().ToString();

            
           

            DrawWalls(GameField);
            DrawDots(GameField);
            DrawCharacters(GameField);

            String path = GetPathToFruitImageByType(field.GetLevelFruitType());
            if (path != null)
            {
                fruitBmp = new BitmapImage();
                fruitBmp.BeginInit();
                Uri uri = new Uri(path, UriKind.Relative);
                PngBitmapDecoder decoder = new PngBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                fruitBmp.UriSource = uri;
                fruitBmp.EndInit();
            }

            InfoTextBlock.Text = "READY!";
            InfoTextBlock.Foreground = Brushes.Yellow;
            moveTimer.Start();
            wanderingModeTimer.Start();
        }
        private void Level_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Level")
            NextLevelGame();
        }
      
        private void DrawCharacters(Canvas canvas)
        {
            DrawGameCharacter(canvas,4,field.Pacman);
            
            if (!field.IsEscapingMode)
            {
               
                    DrawGameCharacter(canvas, 0, field.Inky);
               
                    DrawGameCharacter(canvas,2, field.Pinky);
              
                    DrawGameCharacter(canvas, 1, field.Blinky);
               
                    DrawGameCharacter(canvas, 3, field.Clyde);
            }
            else
            {
                
                DrawGameCharacterEscaping(canvas, field.Inky);
               
                DrawGameCharacterEscaping(canvas, field.Pinky);
               
                DrawGameCharacterEscaping(canvas,field.Blinky);
             
                DrawGameCharacterEscaping(canvas,  field.Clyde);
            }
        }

        private void DrawGameCharacterEscaping(Canvas canvas, Ghost ghost)
        {
            
            Image img = new Image ();
           if(ghost.Visible) 
            img.Source = blueGhostBmp;
           else
               img.Source = ghostEyesBmp;
            img.Width = ghost.Width;
            img.Height = ghost.Height;
            Canvas.SetTop(img, ghost.CurrentPoint.CoordY * field.Cols);
            Canvas.SetLeft(img, ghost.CurrentPoint.CoordX * field.Rows);
            gameCharacterImages.Add(img);
            canvas.Children.Add(img);
        }
        
        private void GameOver_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "gameOver")
            {
                StopAll();
                InfoTextBlock.Text = "GAME OVER";
                InfoTextBlock.Foreground = Brushes.Red;
                Lives.Text = "Lives : 0";
                FreeLives.Text = "Free Lives : 0";
                field.ResetCharacter();

            }
               
        }
        
        private void AnotherAttempt()
        {
            field.ResetCharacter();
            Redraw(GameField);
            Thread.Sleep(500);
            moveTimer.Start();
            wanderingModeTimer.Start();
        }
        
        private void StopAll()
        {
            moveTimer.Stop();
            escapingModeTimer.Stop();
            huntingModeTimer.Stop();
            wanderingModeTimer.Stop();
            
        }
       
       private void Lives_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Lives")
            {
                Lives.Text = "Lives: " + ((Player)sender).Lives.ToString();
             
                AnotherAttempt();
            }
        }

        private void Mode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Mode")
            {
                field.EscapingMode();
                huntingModeTimer.Stop();
                wanderingModeTimer.Stop();
                if (!escapingModeTimer.IsEnabled)
                {
                    escapingModeTimer.Start();
                }
                else if (escapingModeTimer.IsEnabled)
                {
                    escapingModeTimer.Interval += new TimeSpan(0, 0, 5);
                }
               
            }

        }

        private void Score_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Score")
                Score.Text = ((Player)sender).Score.ToString();
        }
        
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    {
                        field.Pacman.AttemptedDirection = Direction.Left;
                       
                        break;

                    }
                case Key.Right:
                    {
                        field.Pacman.AttemptedDirection = Direction.Right;
                      
                        break;
                    }
                case Key.Up:
                    {
                        field.Pacman.AttemptedDirection = Direction.Top;
                        
                        break;
                    }
                case Key.Down:
                    {
                        field.Pacman.AttemptedDirection = Direction.Bottom;
                      
                        break;
                    }
                default:
                    field.Pacman.AttemptedDirection = Direction.None;
                    break;
            }

            
        }
       
        private void EscapingModeInitiated(object source, System.EventArgs e)
       {
           if (escapingModeTimer.IsEnabled)
           {
               escapingModeTimer.IsEnabled = false;
               
           }
           field.EscapingMode();
            

           escapingModeTimer.IsEnabled = true;

       }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            enterNameWindow = new EnterName();
            enterNameWindow.ShowDialog();
            StartGame();
        }

        private void Records_Click(object sender, RoutedEventArgs e)
        {
            Records recordsWindow = new Records();
            recordsWindow.Show();
        }

        static void ToLog(string text)
        {
            try
            {
                string folder = System.AppDomain.CurrentDomain.BaseDirectory;
                using (StreamWriter sw = new StreamWriter(folder + @"\Log.txt", true))
                {
                    sw.WriteLine(text + " : " + DateTime.Now);
                }
            }
            catch { }
        }

        private static void LoadPlugins()
        {
            try
            {
                Plugins = new List<IGhostsMovingAlgorithm>();
                string folder = System.AppDomain.CurrentDomain.BaseDirectory;
                string[] files = System.IO.Directory.GetFiles(folder, "*.dll");
                
                foreach (string file in files)
                {
                    IGhostsMovingAlgorithm plugin = IsPlugin(file);
                    if (plugin != null)
                    { 
                        Plugins.Add(plugin);
                        PluginsNames.Add(file.Substring(file.LastIndexOf('\\')+1));
                    }
                }
            }
            catch (Exception e)
            {
                ToLog(e.Message);
            }
        }

        private static IGhostsMovingAlgorithm IsPlugin(byte[] file)
        {
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(file);
                foreach (Type type in assembly.GetTypes())
                {
                    Type iface = type.GetInterface("IGhostsMovingAlgorithm");
                    if (iface != null)
                    {
                        IGhostsMovingAlgorithm plugin = (IGhostsMovingAlgorithm)Activator.CreateInstance(type);
                        return plugin;
                    }
                }
            }
            catch (Exception e)
            {
                ToLog(e.Message);
            }
            return null;
        }

        private static IGhostsMovingAlgorithm IsPlugin(string file_url)
        {
            try
            {
                byte[] b = System.IO.File.ReadAllBytes(file_url);
                return IsPlugin(b);
            }
            catch (Exception e)
            {
                ToLog(e.Message);
            }
            return null;
        }

    }
    
}
