﻿using GalaSoft.MvvmLight;
using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.ViewModel
{
    public class BiographyViewModel: ViewModelBase
    {
        private List<PhotoModel> _images;
        private string _title="Biography Title";
        private string _description="Description";
        public BiographyViewModel()
        {
        }
        public List<PhotoModel> Images
        {
            get
            {
                if (_images == null)
                    _images = new List<PhotoModel>();
                return _images;
            }
            set
            {
                _images = value;
                RaisePropertyChanged("Images");
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title.Equals(value))
                    return;
                _title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description.Equals(value))
                    return;
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
    }
}
