using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models.DTOs
{
    [Table("tblOrders")]
    public class OrderDTO
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderCreatedAt { get; set; }

        //UserId is a foreign key
        [ForeignKey("UserId")]
        public virtual UserDTO Users { get; set; }
    }
}