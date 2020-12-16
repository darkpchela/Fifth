﻿// <auto-generated />
using Fifth.Services.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fifth.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Fifth.Models.SessionData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Started")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "CreatorId" }, "IX_GameInfoDatas_CreatorId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Fifth.Models.SessionTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "SessionId" }, "IX_SessionTags_SessionId");

                    b.ToTable("SessionTags");
                });

            modelBuilder.Entity("Fifth.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Value" }, "UQ_Text")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Fifth.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Login" }, "UQ_Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Fifth.Models.SessionData", b =>
                {
                    b.HasOne("Fifth.Models.User", "Creator")
                        .WithMany("Sessions")
                        .HasForeignKey("CreatorId")
                        .HasConstraintName("FK_GameSessions_To_Users")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Fifth.Models.SessionTag", b =>
                {
                    b.HasOne("Fifth.Models.SessionData", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .HasConstraintName("FK_SessionTags_To_GameSessions")
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Fifth.Models.User", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
