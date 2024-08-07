﻿using System.ComponentModel.DataAnnotations;

namespace gp_backend.Core.Models
{
    public class FileContent
    {
        [Key]
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public FileDescription FileDescription { get; set; }
        public int FileDescriptionId { get; set; }
    }
}
