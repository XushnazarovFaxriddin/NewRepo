using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaqolAppApi.Models;

public class Muallif
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MuallifId { get; set; }
    public string Ism { get; set; }
    public string Familya { get; set; }
    public string Passport { get; set; }
    public string? RasmUrl { get; set; }
    public DateTime TugilganKuni { get; set; }
}

public class MuallifPost
{
    public string Ism { get; set; }
    public string Familya { get; set; }
    public string Passport { get; set; }
    public DateTime TugilganKuni { get; set; }
    public IFormFile Rasm { get; set; }
}