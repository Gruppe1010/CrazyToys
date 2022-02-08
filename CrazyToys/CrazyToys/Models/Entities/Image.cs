﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Models.Entities
{
    public class Image
    {

        [Column("IdImage")]
        public string Id { get; set; }
        public string UrlLow { get; set; }
        public string UrlHigh { get; set; }

    }
}