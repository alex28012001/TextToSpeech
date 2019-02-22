using BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voice.Models
{
    public class DataForPlayer
    {
        public AudioDto Audio { get; set; }
        public PlayerProperties PlayerProperties { get; set; }
        public bool IsLiked { get; set; }
    }
}