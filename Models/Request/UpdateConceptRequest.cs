﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thu6_pvo_dictionary.Models.Request
{
    public class UpdateConceptRequest
    {
        [Required]
        public int ConceptId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
