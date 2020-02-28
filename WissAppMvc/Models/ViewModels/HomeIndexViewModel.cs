﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WissAppEntities.Entities;

namespace WissAppMvc.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<UsersModel> Users { get; set; }
        public List<MessagesModel> Messages { get; set; }
    }
}