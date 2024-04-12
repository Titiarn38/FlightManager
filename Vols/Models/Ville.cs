namespace Vols.Models
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    public class Ville
    {
        public string? IdVille { get; set; }
        public string? Nom { get; set; }

        // Une ville peut avoir plusieurs vols
        public virtual ICollection<Vol>? Vols { get; set; }

        public static explicit operator JProperty(Ville v)
        {
            throw new NotImplementedException();
        }
    }
}
