using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UMKM_C_.Models;

namespace UMKM_C_.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Bahan> Bahan { get; set; }
        public DbSet<Pengeluaran_harian> Pengeluaran_harian { get; set; }
        public DbSet<Pengeluaran_bulanan> Pengeluaran_bulanan { get; set; }
        public DbSet<Pemasukan_harian> Pemasukan_harian { get; set; }
        public DbSet<Pemasukan_bulanan> Pemasukan_bulanan { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;Database=UMKM;User=root;Password=4456", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pemasukan_bulanan>().HasOne(p => p.Pemasukan_harian).WithMany(p => p.Pemasukan_bulanan)
            .HasForeignKey(p => p.HarianId).OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pengeluaran_bulanan>().HasOne(p => p.Pengeluaran_harian).WithMany(p => p.Pengeluaran_bulanan)
            .HasForeignKey(p => p.HarianId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
