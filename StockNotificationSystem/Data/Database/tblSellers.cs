using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Database
{
    public partial class tblSellers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
