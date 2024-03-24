using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.Models
{
    public interface BooksPrototype
    {
        BooksPrototype Clone();
    }
    public class Product : BooksPrototype
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1, 100000000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 100000000)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 100000000)]
        public double Price100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]


        public Category Category { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }
        [ValidateNever]
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }

        public BooksPrototype Clone()
        {
            var cloneProduct = new Product
            {
                Title = this.Title,
                Description = this.Description,
                ISBN = this.ISBN,
                Author = this.Author,
                ListPrice = this.ListPrice,
                Price50 = this.Price50,
                Price100 = this.Price100,
                ImageUrl = this.ImageUrl,
                CategoryId = this.CategoryId,
                CoverTypeId = this.CoverTypeId
            };
            return cloneProduct;
        }
    }
}
