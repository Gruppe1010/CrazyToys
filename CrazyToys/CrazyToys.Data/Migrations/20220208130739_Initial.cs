using Microsoft.EntityFrameworkCore.Migrations;

namespace CrazyToys.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgeGroups",
                columns: table => new
                {
                    AgeGroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Interval = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeGroups", x => x.AgeGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Colours",
                columns: table => new
                {
                    ColourId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colours", x => x.ColourId);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Toys",
                columns: table => new
                {
                    ToyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColourID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toys", x => x.ToyId);
                    table.ForeignKey(
                        name: "FK_Toys_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Toys_Colours_ColourID",
                        column: x => x.ColourID,
                        principalTable: "Colours",
                        principalColumn: "ColourId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategorySubCategory",
                columns: table => new
                {
                    CategoriesID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubCategoriesID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySubCategory", x => new { x.CategoriesID, x.SubCategoriesID });
                    table.ForeignKey(
                        name: "FK_CategorySubCategory_Categories_CategoriesID",
                        column: x => x.CategoriesID,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategorySubCategory_SubCategories_SubCategoriesID",
                        column: x => x.SubCategoriesID,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgeGroupToy",
                columns: table => new
                {
                    AgeGroupsID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToysID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeGroupToy", x => new { x.AgeGroupsID, x.ToysID });
                    table.ForeignKey(
                        name: "FK_AgeGroupToy_AgeGroups_AgeGroupsID",
                        column: x => x.AgeGroupsID,
                        principalTable: "AgeGroups",
                        principalColumn: "AgeGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgeGroupToy_Toys_ToysID",
                        column: x => x.ToysID,
                        principalTable: "Toys",
                        principalColumn: "ToyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UrlLow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlHigh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToyID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Images_Toys_ToyID",
                        column: x => x.ToyID,
                        principalTable: "Toys",
                        principalColumn: "ToyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubCategoryToy",
                columns: table => new
                {
                    SubCategoriesID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToysID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategoryToy", x => new { x.SubCategoriesID, x.ToysID });
                    table.ForeignKey(
                        name: "FK_SubCategoryToy_SubCategories_SubCategoriesID",
                        column: x => x.SubCategoriesID,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubCategoryToy_Toys_ToysID",
                        column: x => x.ToysID,
                        principalTable: "Toys",
                        principalColumn: "ToyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgeGroupToy_ToysID",
                table: "AgeGroupToy",
                column: "ToysID");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySubCategory_SubCategoriesID",
                table: "CategorySubCategory",
                column: "SubCategoriesID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ToyID",
                table: "Images",
                column: "ToyID");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryToy_ToysID",
                table: "SubCategoryToy",
                column: "ToysID");

            migrationBuilder.CreateIndex(
                name: "IX_Toys_BrandID",
                table: "Toys",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Toys_ColourID",
                table: "Toys",
                column: "ColourID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgeGroupToy");

            migrationBuilder.DropTable(
                name: "CategorySubCategory");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "SubCategoryToy");

            migrationBuilder.DropTable(
                name: "AgeGroups");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Toys");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Colours");
        }
    }
}
