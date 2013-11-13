using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Music_Player.Model
{
    public class SongModelValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            SongModel song = (value as BindingGroup).Items[0] as SongModel;
            if (song.Year < 1920 || song.Year > DateTime.Now.Year)
            {
                return new ValidationResult(false,
                    "Song year tag cannot be less than 1920 nor can it be larger than current year");
            }
            else if(song.Rating>255 || song.Rating<0)
            {
                return new ValidationResult(false,
                    "Rating tag has to be in range [0-255]");
            }
            else if (song.Album.IndexOf('\"') > -1 || song.Artist.IndexOf('\"') > -1 || song.Genre.IndexOf('\"') > -1 || song.Title.IndexOf('\"') > -1)
            {
                return new ValidationResult(false,
                    "String tags cannot contain \" signs");
            }
            return ValidationResult.ValidResult;
        }
    }

}
