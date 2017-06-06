using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using YarakiiBot.Model;

namespace YarakiiBot.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20170606190952_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("YarakiiBot.Model.SongRequest", b =>
                {
                    b.Property<int>("SongRequestId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Link");

                    b.Property<int>("UserId");

                    b.HasKey("SongRequestId");

                    b.ToTable("SongRequests");
                });

            modelBuilder.Entity("YarakiiBot.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Points");

                    b.Property<string>("Username");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
        }
    }
}
