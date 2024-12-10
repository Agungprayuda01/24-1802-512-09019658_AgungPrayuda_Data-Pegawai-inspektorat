using System.ComponentModel.DataAnnotations;

namespace Data_Pegawai_Inspektorat.Models
{
    public class Pegawai
    {
        [Key] public required string Nik { get; set; }
        public required string Name { get; set; }
        public string? alamat { get; set; }
        public string? Ttl { get; set; }
        public string? role { get; set; }
        public string? email { get; set; }
        public required string path { get; set; }

    }
}
