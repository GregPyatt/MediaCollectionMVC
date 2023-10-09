using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaCollectionMVC.Models;

public partial class ScannedMedium
{
    [Key]
    public int MediaId { get; set; }

    public string? Title { get; set; }

    public string? Authors { get; set; }

    public string? Categories { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string? Publisher { get; set; }

    public int? Pages { get; set; }

    public string Isbn { get; set; } = null!;

    public bool? IsRead { get; set; }

    public string? ReadingPeriods { get; set; }

    public string? Comments { get; set; }

    public string? Summary { get; set; }

    public string? CoverPath { get; set; }

    public bool? IsAudioBook { get; set; }

    public bool? IsPaperBook { get; set; }

    public bool? IsPdfbook { get; set; }

    public bool? IsDonated { get; set; }

    public DateTime? DonationDate { get; set; }

    public string? Medium { get; set; }

    public string? Quality { get; set; }

    public bool? OktoDonate { get; set; }
}
