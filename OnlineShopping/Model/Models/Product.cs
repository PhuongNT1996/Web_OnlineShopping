namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Images = new HashSet<Image>();
            Product_Cart = new HashSet<Product_Cart>();
            Product_Order_Details = new HashSet<Product_Order_Details>();
            Promotions = new HashSet<Promotion>();
        }

        [Key]
        public int Product_ID { get; set; }

        public int Catalogue_ID { get; set; }

        public bool Is_Sale { get; set; }

        [Required]
        [StringLength(250)]
        public string Product_Name { get; set; }

        private float price;

        public float Price
        {
            get { return (float)Math.Round(price); }
            set { price = value; }
        }


        public int Level_Trending { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public int? Products_Available { get; set; }

        public int? Total_Sold { get; set; }

        public DateTime? Created_Date { get; set; }

        [Required]
        [StringLength(50)]
        public string Created_Username { get; set; }

        [Column(TypeName = "ntext")]
        public string Guarantee_Description { get; set; }

        [StringLength(250)]
        public string Title_Image { get; set; }

        public float? Tax_Percent { get; set; }

        [StringLength(250)]
        public string Manufacturer { get; set; }

        public virtual Catalogue Catalogue { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Cart> Product_Cart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Order_Details> Product_Order_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Promotion> Promotions { get; set; }

        private float realPrice;

        public float RealPrice
        {
            get { return getRealPrice(); }
            set { realPrice = value; }
        }

        private float getRealPrice()
        {
            return (float)Math.Round((this.Price - (this.Price * this.DiscountPercent / 100)));
        }

        private int discountPercent;

        public int DiscountPercent
        {
            get
            {
                int discountPercent = 0;
                foreach (Promotion item in this.Promotions)
                {
                    discountPercent += (int)item.Discount_Percent;
                }
                return discountPercent;
            }
            set { discountPercent = value; }
        }

    }
}
