using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuestionBank.Models
{
    public partial class QBDBContext : DbContext
    {
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Buggy> Buggy { get; set; }
        public virtual DbSet<Lectures> Lectures { get; set; }
        public virtual DbSet<Levels> Levels { get; set; }
        public virtual DbSet<QuestionsCorrects> QuestionsCorrects { get; set; }
        public virtual DbSet<QuestionsTests> QuestionsTests { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<Types> Types { get; set; }

        public QBDBContext(DbContextOptions<QBDBContext> options)
 : base(options)
        { }

        public QBDBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=QBDB;Integrated Security=True;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(250);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(75);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Tel)
                    .HasColumnName("tel")
                    .HasMaxLength(20);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("((2))");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(75);

                entity.Property(e => e.Vip)
                    .HasColumnName("vip")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VipDatetime)
                    .HasColumnName("vip_datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Buggy>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.QId).HasColumnName("Q_id");

                entity.Property(e => e.QType).HasColumnName("Q_type");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Lectures>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Levels>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title).HasColumnName("title");
            });

            modelBuilder.Entity<QuestionsCorrects>(entity =>
            {
                entity.ToTable("Questions_Corrects");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LectureId).HasColumnName("lecture_id");

                entity.Property(e => e.Levels)
                    .HasColumnName("levels")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.QFalse).HasColumnName("q_false");

                entity.Property(e => e.QTrue).HasColumnName("q_true");

                entity.Property(e => e.Result).HasColumnName("result");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<QuestionsTests>(entity =>
            {
                entity.ToTable("Questions_Tests");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.A).HasColumnName("a");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.B).HasColumnName("b");

                entity.Property(e => e.C).HasColumnName("c");

                entity.Property(e => e.D).HasColumnName("d");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.E).HasColumnName("e");

                entity.Property(e => e.LectureId).HasColumnName("lecture_id");

                entity.Property(e => e.Levels)
                    .HasColumnName("levels")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Result).HasColumnName("result");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.LectureId).HasColumnName("lecture_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Types>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title).HasColumnName("title");
            });
        }
    }
}
