using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFConcurrency
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
