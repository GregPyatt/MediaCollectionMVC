using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCollectionMVC.Models;

public partial class ScannedMedium
{
    [Key]
    public int MediaId { get; set; }

    public string? Title { get; set; }

    public string? Authors { get; set; }

    [Display(Name = "Author")]
    public string? Authors_LNFN { get; set; }

    public string? Categories { get; set; }

    [NotMapped]
    public List<SelectListItem>? CategoryNames { get; set; }

    [Display(Name = "Published Date")]
    // Somehow, when using a DisplayFormat the date won't show up in the Edit view.
    //[DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
    //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? PublishedDate { get; set; }

    public string? Publisher { get; set; }

    [NotMapped]
    public List<SelectListItem>? PublisherNames { get; set; }

    public int? Pages { get; set; }

    [Display(Name = "ISBN")]
    public string Isbn { get; set; } = null!;

    [Display(Name = "Is Read")]
    public bool IsRead { get; set; }  //was null

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

    [NotMapped]
    public List<SelectListItem>? MediumTypes { get; set; }

    public string? Quality { get; set; }

    [Display(Name = "Is OK to Donate")]
    public bool? OktoDonate { get; set; }

    [Display(Name = "Is Audio Book")]
    [NotMapped]
    //Added non-nullable boolean values based on the database for use in views & asp-for type="checkbox" that require non-null bool.
    public bool IsAudioBookCheckbox
    {
        get { return IsAudioBook.GetValueOrDefault() ? true : false; }
        set => IsAudioBook = value;
    }

    [Display(Name = "Is Paper Book")]
    [NotMapped]
    //Added non-nullable boolean values based on the database for use in views & asp-for type="checkbox" that require non-null bool.
    public bool IsPaperBookCheckbox
    {
        get { return IsPaperBook.GetValueOrDefault() ? true : false; }
        set => IsPaperBook = value;
    }

    [Display(Name = "Is PDF Book")]
    [NotMapped]
    //Added non-nullable boolean values based on the database for use in views & asp-for type="checkbox" that require non-null bool.
    public bool IsPdfBookCheckbox
    {
        get { return IsPdfbook.GetValueOrDefault() ? true : false; }
        set => IsPdfbook = value;
    }

    [Display(Name = "Is Donated")]
    [NotMapped]
    //Added non-nullable boolean values based on the database for use in views & asp-for type="checkbox" that require non-null bool.
    public bool IsDonatedCheckbox
    {
        get { return IsDonated.GetValueOrDefault() ? true : false; }
        set => IsDonated = value;
    }


    [Display(Name = "Is OK to Donate")]
    [NotMapped]
    //Added non-nullable boolean values based on the database for use in views & asp-for type="checkbox" that require non-null bool.
    public bool OktoDonateCheckbox
    {
        get { return OktoDonate.GetValueOrDefault() ? true : false; }
        set => OktoDonate = value;
    }


}
