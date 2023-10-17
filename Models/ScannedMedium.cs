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

    [Display(Name = "Published Date")]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? PublishedDate { get; set; }

    public string? Publisher { get; set; }

    public int? Pages { get; set; }

    [Display(Name = "ISBN")]
    public string Isbn { get; set; } = null!;

    [Display(Name = "Is Read")]
    public bool? IsRead { get; set; }

    [Display(Name = "Reading Periods")]
    public string? ReadingPeriods { get; set; }

    public string? Comments { get; set; }

    public string? Summary { get; set; }

    [Display(Name = "Cover Path")]
    public string? CoverPath { get; set; }

    [Display(Name = "Is Audio Book")]
    public bool? IsAudioBook { get; set; }

    [Display(Name = "Is Paper Book")]
    public bool? IsPaperBook { get; set; }

    [Display(Name = "Is PDF Book")]
    public bool? IsPdfbook { get; set; }

    [Display(Name = "Is Donated")]
    public bool? IsDonated { get; set; }

    [Display(Name = "Donation Date")]
    public DateTime? DonationDate { get; set; }

    public string? Medium { get; set; }

    public string? Quality { get; set; }

    [Display(Name = "Is OK to Donate")]
    public bool? OktoDonate { get; set; }
}
