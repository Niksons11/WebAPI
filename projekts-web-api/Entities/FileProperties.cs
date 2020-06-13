using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Entities
{
    public class FileProperties
    {
        public int Id { get; set; }
        public string FileNameExtension { get; set; }
    }
}
