﻿// <auto-generated />
using Clamify.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Clamify.IntegrationTests.Migrations
{
    [DbContext(typeof(ClamifyContext))]
    [Migration("20221002010034_AddExampleTable")]
    partial class AddExampleTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Clamify.Entities.Example", b =>
                {
                    b.Property<int>("ExampleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ExampleID"));

                    b.HasKey("ExampleID");

                    b.ToTable("Example", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
