using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaMinas.Models
{
    public class Level
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mines { get; set; }
        public Level()
        {
            Width = 5;
            Height = 5;
            Mines = 25 / 6;
        }
        public Level(int w, int h, int m)
        {
            Width = W(w);
            Height = H(h);
            Mines = M(Width, Height, m);
        }
        public Level(Level source)
        {
            Width = W(source.Width);
            Height = H(source.Height);
            Mines = M(Width, Height, source.Mines);
        }
        public Level(Difficulty difficulty)
        {
            if (difficulty == Difficulty.easy)
            {
                Width = 8;
                Height = 8;
                Mines = 8 * 8 / 6;
            }
            else if (difficulty == Difficulty.medium)
            {
                Width = 16;
                Height = 12;
                Mines = 16 * 12 / 6;
            }
            else if (difficulty == Difficulty.hard)
            {
                Width = 24;
                Height = 16;
                Mines = 24 * 16 / 6;
            }
        }
        private int M(int w, int h, int m)
        {
            int maxMines = w * h - 9;
            if (m < 1)
            {
                m = (w * h) / 6;
            }
            else if (m > maxMines)
            {
                m = maxMines;
            }
            return m;
        }
        private int W(int w)
        {
            if (w < 5)
            {
                return 5;
            }
            else if (w > 25)
            {
                return 25;
            }
            return w;
        }
        private int H(int h)
        {
            if (h < 5)
            {
                return 5;
            }
            else if (h > 16)
            {
                return 16;
            }
            return h;
        }
    }
    public enum Difficulty
    {
        none,
        easy,
        medium,
        hard,
        custom
    }
}
