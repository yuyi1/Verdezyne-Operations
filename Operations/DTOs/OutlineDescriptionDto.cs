using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Operations.Models;

namespace Operations.DTOs
{
    public class OutlineDescriptionDto
    {
        public int Id { get; set; }
        [Display(Name = "Outline Topic")]
        public int OutlineTopicId { get; set; }
        public string Name { get; set; }
        public string OutlineTopicName { get; set; }
        public int ValuesState { get; set; }
        public Nullable<bool> Checked { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime LastUpdate { get; set; }

        public virtual OutlineTopic OutlineTopic { get; set; }
        public virtual List<OutlineTopic> OutlineTopics { get; set; }

        public IEnumerable<SelectListItem> OutlineTopicSelect
        {
            get { return new SelectList(OutlineTopics, "Id", "Name"); }
        }
    }
}