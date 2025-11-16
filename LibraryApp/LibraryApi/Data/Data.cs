using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;
using Microsoft.Extensions.Options;
using LibraryApi.DTOs;

namespace LibraryApi.Data;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Authors> Authors { get; set; }

    public virtual DbSet<Bookcovers> Bookcovers { get; set; }

    public virtual DbSet<Books> Books { get; set; }

    public virtual DbSet<Booksubjects> Booksubjects { get; set; }

    public virtual DbSet<Customers> Customers { get; set; }

    public virtual DbSet<Issues> Issues { get; set; }

    public virtual DbSet<Exhibitions> Exhibitions { get; set; }
    public virtual DbSet<ExhibitionBooks> ExhibitionBooks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authors>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("authors_pkey");

            entity.ToTable("authors");

            entity.Property(e => e.Key)
                .HasMaxLength(128)
                .HasColumnName("key");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.Birthdate)
                .HasMaxLength(20)
                .HasColumnName("birthdate");
            entity.Property(e => e.Deathdate)
                .HasMaxLength(20)
                .HasColumnName("deathdate");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Wikipedia)
                .HasMaxLength(255)
                .HasColumnName("wikipedia");
        });

        modelBuilder.Entity<Bookcovers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookcovers_pkey");

            entity.ToTable("bookcovers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookKey)
                .HasMaxLength(128)
                .HasColumnName("book_key");
            entity.Property(e => e.Coverfile).HasColumnName("coverfile");

            entity.HasOne(d => d.BookKeyNavigation).WithMany(p => p.Bookcovers)
                .HasForeignKey(d => d.BookKey)
                .HasConstraintName("fk_bookcovers_books");
        });

        modelBuilder.Entity<Books>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("books_pkey");

            entity.ToTable("books");

            entity.Property(e => e.Key)
                .HasMaxLength(128)
                .HasColumnName("key");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Firstpublishdate)
                .HasMaxLength(20)
                .HasColumnName("firstpublishdate");
            entity.Property(e => e.Subtitle)
                .HasMaxLength(500)
                .HasColumnName("subtitle");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");

            entity.HasMany(d => d.Authorkey).WithMany(p => p.Bookkey)
                .UsingEntity<Dictionary<string, object>>(
                    "Bookauthors",
                    r => r.HasOne<Authors>().WithMany()
                        .HasForeignKey("Authorkey")
                        .HasConstraintName("fk_bookauthors_authors"),
                    l => l.HasOne<Books>().WithMany()
                        .HasForeignKey("Bookkey")
                        .HasConstraintName("fk_bookauthors_books"),
                    j =>
                    {
                        j.HasKey("Bookkey", "Authorkey").HasName("bookauthors_pkey");
                        j.ToTable("bookauthors");
                        j.IndexerProperty<string>("Bookkey")
                            .HasMaxLength(128)
                            .HasColumnName("bookkey");
                        j.IndexerProperty<string>("Authorkey")
                            .HasMaxLength(128)
                            .HasColumnName("authorkey");
                    });
        });

        modelBuilder.Entity<Booksubjects>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("booksubjects_pkey");

            entity.ToTable("booksubjects");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookKey)
                .HasMaxLength(128)
                .HasColumnName("book_key");
            entity.Property(e => e.Subject)
                .HasMaxLength(400)
                .HasColumnName("subject");

            entity.HasOne(d => d.BookKeyNavigation).WithMany(p => p.Booksubjects)
                .HasForeignKey(d => d.BookKey)
                .HasConstraintName("fk_booksubjects_books");
        });

        modelBuilder.Entity<Customers>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.HasIndex(e => e.Email, "customers_email_key").IsUnique();

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Zip)
                .HasMaxLength(20)
                .HasColumnName("zip");
        });

        modelBuilder.Entity<Issues>(entity =>
        {
            entity.HasKey(e => e.Issueid).HasName("issues_pkey");

            entity.ToTable("issues");

            entity.Property(e => e.Issueid).HasColumnName("issueid");
            entity.Property(e => e.Bookkey)
                .HasMaxLength(128)
                .HasColumnName("bookkey");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Dateofissue).HasColumnName("dateofissue");
            entity.Property(e => e.Returndate).HasColumnName("returndate");
            entity.Property(e => e.Returnuntil).HasColumnName("returnuntil");

            entity.HasOne(d => d.BookkeyNavigation).WithMany(p => p.Issues)
                .HasForeignKey(d => d.Bookkey)
                .HasConstraintName("fk_issues_books");

            entity.HasOne(d => d.Customer).WithMany(p => p.Issues)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("fk_issues_customers");
        });

        modelBuilder.Entity<Exhibitions>(entity =>
        {
            entity.HasKey(e => e.ExhibitionId).HasName("exhibitions_pkey");
            entity.ToTable("exhibitions");

            entity.Property(e => e.ExhibitionId).HasColumnName("exhibition_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
            entity.Property(e => e.CoverBookKey)
                .HasMaxLength(128)
                .HasColumnName("cover_book_key");

            entity.HasOne(e => e.CoverBook)
                .WithMany()
                .HasForeignKey(e => e.CoverBookKey)
                .HasConstraintName("fk_exhibitions_cover_book");
        });

        modelBuilder.Entity<ExhibitionBooks>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("exhibition_books_pkey");
            entity.ToTable("exhibition_books");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExhibitionId).HasColumnName("exhibition_id");
            entity.Property(e => e.BookKey)
                .HasMaxLength(128)
                .HasColumnName("book_key");
            entity.Property(e => e.OrderNumber).HasColumnName("order_number");

            entity.HasOne(d => d.Exhibition)
                .WithMany(p => p.ExhibitionBooks)
                .HasForeignKey(d => d.ExhibitionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_exhibition_books_exhibitions");

            entity.HasOne(d => d.Book)
                .WithMany()
                .HasForeignKey(d => d.BookKey)
                .HasConstraintName("fk_exhibition_books_books");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
