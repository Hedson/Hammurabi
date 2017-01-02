using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Hammurabi.Data;

namespace Hammurabi.Migrations
{
    [DbContext(typeof(RestaurantContext))]
    partial class RestaurantContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Hammurabi.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddPrice");

                    b.Property<string>("Name");

                    b.HasKey("IngredientID");

                    b.ToTable("Ingredient");
                });

            modelBuilder.Entity("Hammurabi.Models.Meal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("PreparationTime");

                    b.Property<decimal>("Price");

                    b.HasKey("ID");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("Hammurabi.Models.MealIngredient", b =>
                {
                    b.Property<int>("MealIngredientID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IngredientID");

                    b.Property<int>("MealID");

                    b.HasKey("MealIngredientID");

                    b.HasIndex("IngredientID");

                    b.HasIndex("MealID");

                    b.ToTable("MealIngredient");
                });

            modelBuilder.Entity("Hammurabi.Models.MealIngredient", b =>
                {
                    b.HasOne("Hammurabi.Models.Ingredient", "Ingredient")
                        .WithMany("MealIngeredient")
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Hammurabi.Models.Meal", "Meal")
                        .WithMany("MealIngredients")
                        .HasForeignKey("MealID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
