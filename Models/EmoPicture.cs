using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Models
{
    public class EmoPicture
    {
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "La ruta es demasido larga.")]
        public string Path { get; set; }


        public virtual ObservableCollection<EmoFace> Faces { get; set; }
    }
}