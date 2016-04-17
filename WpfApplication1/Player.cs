using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApplication1
{
    public class Player : INotifyPropertyChanged
    {
        
        private int score;
        private int lives;
        private int freeLives;
        private bool gameOver;
        private string name;
        private Level level;

        public event PropertyChangedEventHandler PropertyChanged;

        public Player(string name)
        {
            score = 0;
            lives = 3;
            freeLives = 0;
            gameOver = false;
            this.name = name;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int Score
        {
            get { return score; } 
            set 
            {
                if (value >= 0)
                {
                    score = value;
                    
                    NotifyPropertyChanged("Score");
                }
            } 
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Lives
        {
            get { return lives; }
            set 
            {
                if (value >= 0)
                {
                   
                    lives = value;
                    if (lives == 0 && freeLives == 0)
                    {
                        GameOver = true;
                    }
                    else
                    {
                        NotifyPropertyChanged("Lives");
                    }
                    
                }
            }
        }

        public int FreeLives
        {
            get { return freeLives; }
            set {
               
                if (value < freeLives)
                {
                    freeLives = value;
                    if (freeLives != 0)
                        NotifyPropertyChanged("FreeLivesDown");
                    else GameOver = true;
                }
                else
                {
                    freeLives = value;
                    NotifyPropertyChanged("FreeLives");
                }
                
              
            }
        }

        public Level CurrentLevel
        {
            get { return level; }
            set 
            { 
                level = value; 
            
            }
        }

        public bool GameOver
        {
            get { return gameOver; }
            set
            {
                gameOver = value;
                if (gameOver)
                {
                    NotifyPropertyChanged("gameOver");
                    AddToDB();
                }
            }
        }

        private void AddToDB()
        {
            PlayerDB player = new PlayerDB { Name = this.name, Score = this.Score, Level = this.level.LevelNumber };
            using (PlayerContext db = new PlayerContext())
            {
                PlayerDB pl = db.Players.Find(player.Name);
                if (pl != null)
                {
                    if (pl.Score < player.Score)
                    {
                        pl.Score = player.Score;
                    }
                }
                else { db.Players.Add(player); }
                db.SaveChanges();

            }
        }
    }
}
