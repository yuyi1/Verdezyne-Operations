using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Operations.Models;

namespace Operations.DTOs
{
    public class OutlineItemsDto
    {
        public string Rundate { get; set; }
        public Run Run { get; set; }
        public int ProductId { get; set; }
        public int MachineId { get; set; }
        public IQueryable<OutlineItem> OutlineItems { get; set; }
        public IQueryable<Machine> Machines { get; set; }
        public IQueryable<Tank> Tanks { get; set; }
        public IQueryable<Product> Products { get; set; }
        public IQueryable<OutlineTopic> OutlineTopics { get; set; }
        public IQueryable<OutlineDescription> OutlineDescriptions { get; set; }

        public IEnumerable<SelectListItem> ProductSelectListItems
        {
            get { return new SelectList(Products, "Id", "Name"); }
        }
        public IEnumerable<SelectListItem> MachineSelectListItems
        {
            get { return new SelectList(Machines, "Id", "Name"); }
        }
    }
}