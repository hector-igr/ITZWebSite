using System;
using System.Collections.Generic;
using System.Text;

namespace ForgeLibs.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Scope { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string CoverImageName { get; set; }
        public string Job { get; set; }
        public bool Model { get; set; }
        public int Importance { get; set; }
    }
}
