namespace Vols.Models
{
    using System;

    public class Vol
    {
        public string? IdVol { get; set; }
        public string? Reference { get; set; }
        public string? VilleDepart { get; set; }
        public string?  VilleArrivee { get; set; }
        public DateTime DateDepart { get; set; }
        public DateTime DateArrivee { get; set; }

        //pour faire du relationnel
        public string? IdVille { get; set; }
        public virtual Ville? Ville { get; set; }
    }
}
