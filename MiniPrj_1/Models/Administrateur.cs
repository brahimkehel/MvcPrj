//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiniPrj_1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Administrateur
    {
        public int id { get; set; }
    
        public virtual Utilisateur Utilisateur { get; set; }
    }
}
