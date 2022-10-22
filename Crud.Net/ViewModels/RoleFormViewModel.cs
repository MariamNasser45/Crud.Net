﻿using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Crud.Net.ViewModels
{
    public class RoleFormViewModel
    {
        // define only name of which role need to add

        [Required , StringLength(200)]
        public string Name { get; set; }

    }
}
