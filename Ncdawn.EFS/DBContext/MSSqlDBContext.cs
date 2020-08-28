using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Ncdawn.EFS.Models;

namespace Ncdawn.EFS.DBContext
{
    public partial class MSSqlDBContext : DbContext
    {
        public MSSqlDBContext()
        {
        }

        public MSSqlDBContext(DbContextOptions<MSSqlDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SysArea> SysArea { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer(dbUrl);
                //optionsBuilder.UseSqlServer("Server=172.26.55.251; Database=AppsDBDLHATest;Persist Security Info=True;User ID = sa; password=1qazXSW@;");
                optionsBuilder.UseSqlServer("Server=ubuntu.dlanqi.com; Database=AppsDBDLHA;Persist Security Info=True;User ID = sa; password=1qazXSW@;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<SysArea>(entity =>
            {
                entity.HasKey(e => e.ObjectId);

                entity.Property(e => e.ObjectId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsOther).IsRequired();

                entity.Property(e => e.IsSpecial).IsRequired();

                entity.Property(e => e.ModificationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Pid)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

        }
    }
}
