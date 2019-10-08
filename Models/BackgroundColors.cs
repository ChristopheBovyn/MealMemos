using System;
using System.Collections.Generic;

namespace MealMemos.Models
{
    public class BackgroundColors
    {
        public List<BackgroundColors> defaultsBgColors { get; }
        public int red { get; }
        public int green { get; }
        public int blue { get; }

        public BackgroundColors()
        {
            this.defaultsBgColors = new List<BackgroundColors>();
            setDefaultBackgroundsColor();
        }

        public BackgroundColors(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.defaultsBgColors = new List<BackgroundColors>();
            setDefaultBackgroundsColor();
        }

        private void setDefaultBackgroundsColor()
        {
            this.defaultsBgColors.Add(new BackgroundColors(255, 0, 0));
            
            this.defaultsBgColors.Add(new BackgroundColors(0, 255, 0));
            this.defaultsBgColors.Add(new BackgroundColors(0, 0, 255));
            this.defaultsBgColors.Add(new BackgroundColors(255, 255, 0));
            this.defaultsBgColors.Add(new BackgroundColors(0, 255, 255));
            this.defaultsBgColors.Add(new BackgroundColors(255, 0, 255));
            this.defaultsBgColors.Add(new BackgroundColors(128, 0, 128));
        }
    }
}