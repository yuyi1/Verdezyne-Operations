using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Components.DictionaryAdapter;

namespace Operations.DTOs
{
    public class CalibrationStandardDto
    {
        public int Id { get; set; }
        [Display(Name = "Calibration Type")]
        public string CalibrationName { get; set; }
        
        [Display(Name = "Calibration Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CalibrationDate { get; set; }
        [Required]
        public string Folder { get; set; }
        [Required]
        public string Filename { get; set; }
        public string Tablename { get; set; }

        public IEnumerable<SelectListItem> CalibrationTypeListItems
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>
                {
                    new SelectListItem {Value = "Anion", Text = "Anion", Selected = false},
                    new SelectListItem {Value = "Cation", Text = "Cation", Selected = false},
                    new SelectListItem {Value = "Iron", Text = "Iron", Selected = false}
                };

                return items;
            }
        }



    }

    
}