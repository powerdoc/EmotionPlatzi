﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Models
{
    public class Home
    {
        public int id { get; set; }
        public string WelcomeMessage { get; set; }
        public string FooterMessage { get; set; } = "Created by @Werdoc";
    }
}