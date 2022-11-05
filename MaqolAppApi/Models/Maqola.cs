using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaqolAppApi.Models;

public class Maqola
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MaqolaId { get; set; }
    [MaxLength(255)]
    public string Sarlavha { get; set; }
    public string Batafsil { get; set; }
    [DefaultValue(0)]
    public int KurishlarSoni { get; set; }
    public DateTime? YaratilganVaqti { get; set; }
    public string? RasmUrl { get; set; }

    [ForeignKey(nameof(Muallif))]
    public Guid MuallifId { get; set; }
    public Muallif Muallif { get; set; }

    public MaqolaView ToMaqolaView()
    {
        MaqolaView maqolaView = new MaqolaView();
        maqolaView.MaqolaId = this.MaqolaId.ToString();
        maqolaView.MuallifId = this.MuallifId.ToString();
        maqolaView.Sarlavha = this.Sarlavha;
        maqolaView.Batafsil = this.Batafsil;
        maqolaView.YaratilganVaqti = this.YaratilganVaqti;
        maqolaView.KurishlarSoni = this.KurishlarSoni;
        maqolaView.RasmUrl = this.RasmUrl;
        if (this.Muallif != null)
            maqolaView.MuallifNomi = this.Muallif.Ism + " " + this.Muallif.Familya;
        return maqolaView;
    }
}


public class MaqolaPost
{
    [MaxLength(255)]
    public string Sarlavha { get; set; }
    public string Batafsil { get; set; }
    public IFormFile Rasm { get; set; }

    public Guid MuallifId { get; set; }

    public Maqola ToMaqola()
    {
        Maqola maqola = new Maqola();
        maqola.Sarlavha = Sarlavha;
        maqola.Batafsil = Batafsil;
        maqola.MuallifId = MuallifId;
        return maqola;
    }
}

public class MaqolaView
{
    public string MaqolaId { get; set; }
    public string Sarlavha { get; set; }
    public string Batafsil { get; set; }
    public string MuallifNomi { get; set; }
    public string MuallifId { get; set; }
    public string RasmUrl { get; set; }
    public DateTime? YaratilganVaqti { get; set; }
    public int KurishlarSoni { get; set; }
}