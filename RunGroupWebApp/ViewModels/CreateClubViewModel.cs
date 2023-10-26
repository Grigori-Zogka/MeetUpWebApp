﻿using System;
using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.ViewModels
{
	public class CreateClubViewModel
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}

