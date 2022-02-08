using Microsoft.EntityFrameworkCore.Migrations;

namespace CrazyToys.Web.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgeGroupToy_AgeGroups_AgeGroupsId",
                table: "AgeGroupToy");

            migrationBuilder.DropForeignKey(
                name: "FK_AgeGroupToy_Toys_ToysId",
                table: "AgeGroupToy");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Toys_ToyId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Categories_CategoryID",
                table: "SubCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategoryToy_SubCategories_SubCategoriesId",
                table: "SubCategoryToy");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategoryToy_Toys_ToysId",
                table: "SubCategoryToy");

            migrationBuilder.DropForeignKey(
                name: "FK_Toys_Brands_BrandId",
                table: "Toys");

            migrationBuilder.DropForeignKey(
                name: "FK_Toys_Colours_ColourId",
                table: "Toys");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_CategoryID",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "SubCategories");

            migrationBuilder.RenameColumn(
                name: "ColourId",
                table: "Toys",
                newName: "ColourID");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "Toys",
                newName: "BrandID");

            migrationBuilder.RenameColumn(
                name: "IdToy",
                table: "Toys",
                newName: "ToyId");

            migrationBuilder.RenameIndex(
                name: "IX_Toys_ColourId",
                table: "Toys",
                newName: "IX_Toys_ColourID");

            migrationBuilder.RenameIndex(
                name: "IX_Toys_BrandId",
                table: "Toys",
                newName: "IX_Toys_BrandID");

            migrationBuilder.RenameColumn(
                name: "ToysId",
                table: "SubCategoryToy",
                newName: "ToysID");

            migrationBuilder.RenameColumn(
                name: "SubCategoriesId",
                table: "SubCategoryToy",
                newName: "SubCategoriesID");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategoryToy_ToysId",
                table: "SubCategoryToy",
                newName: "IX_SubCategoryToy_ToysID");

            migrationBuilder.RenameColumn(
                name: "IdSubCategory",
                table: "SubCategories",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "ToyId",
                table: "Images",
                newName: "ToyID");

            migrationBuilder.RenameColumn(
                name: "IdImage",
                table: "Images",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ToyId",
                table: "Images",
                newName: "IX_Images_ToyID");

            migrationBuilder.RenameColumn(
                name: "IdColour",
                table: "Colours",
                newName: "ColourId");

            migrationBuilder.RenameColumn(
                name: "IdBrand",
                table: "Brands",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "ToysId",
                table: "AgeGroupToy",
                newName: "ToysID");

            migrationBuilder.RenameColumn(
                name: "AgeGroupsId",
                table: "AgeGroupToy",
                newName: "AgeGroupsID");

            migrationBuilder.RenameIndex(
                name: "IX_AgeGroupToy_ToysId",
                table: "AgeGroupToy",
                newName: "IX_AgeGroupToy_ToysID");

            migrationBuilder.RenameColumn(
                name: "IdAgeGroup",
                table: "AgeGroups",
                newName: "AgeGroupId");

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

            migrationBuilder.CreateIndex(
                name: "IX_CategorySubCategory_SubCategoriesID",
                table: "CategorySubCategory",
                column: "SubCategoriesID");

            migrationBuilder.AddForeignKey(
                name: "FK_AgeGroupToy_AgeGroups_AgeGroupsID",
                table: "AgeGroupToy",
                column: "AgeGroupsID",
                principalTable: "AgeGroups",
                principalColumn: "AgeGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AgeGroupToy_Toys_ToysID",
                table: "AgeGroupToy",
                column: "ToysID",
                principalTable: "Toys",
                principalColumn: "ToyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Toys_ToyID",
                table: "Images",
                column: "ToyID",
                principalTable: "Toys",
                principalColumn: "ToyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategoryToy_SubCategories_SubCategoriesID",
                table: "SubCategoryToy",
                column: "SubCategoriesID",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategoryToy_Toys_ToysID",
                table: "SubCategoryToy",
                column: "ToysID",
                principalTable: "Toys",
                principalColumn: "ToyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Toys_Brands_BrandID",
                table: "Toys",
                column: "BrandID",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Toys_Colours_ColourID",
                table: "Toys",
                column: "ColourID",
                principalTable: "Colours",
                principalColumn: "ColourId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgeGroupToy_AgeGroups_AgeGroupsID",
                table: "AgeGroupToy");

            migrationBuilder.DropForeignKey(
                name: "FK_AgeGroupToy_Toys_ToysID",
                table: "AgeGroupToy");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Toys_ToyID",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategoryToy_SubCategories_SubCategoriesID",
                table: "SubCategoryToy");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategoryToy_Toys_ToysID",
                table: "SubCategoryToy");

            migrationBuilder.DropForeignKey(
                name: "FK_Toys_Brands_BrandID",
                table: "Toys");

            migrationBuilder.DropForeignKey(
                name: "FK_Toys_Colours_ColourID",
                table: "Toys");

            migrationBuilder.DropTable(
                name: "CategorySubCategory");

            migrationBuilder.RenameColumn(
                name: "ColourID",
                table: "Toys",
                newName: "ColourId");

            migrationBuilder.RenameColumn(
                name: "BrandID",
                table: "Toys",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "ToyId",
                table: "Toys",
                newName: "IdToy");

            migrationBuilder.RenameIndex(
                name: "IX_Toys_ColourID",
                table: "Toys",
                newName: "IX_Toys_ColourId");

            migrationBuilder.RenameIndex(
                name: "IX_Toys_BrandID",
                table: "Toys",
                newName: "IX_Toys_BrandId");

            migrationBuilder.RenameColumn(
                name: "ToysID",
                table: "SubCategoryToy",
                newName: "ToysId");

            migrationBuilder.RenameColumn(
                name: "SubCategoriesID",
                table: "SubCategoryToy",
                newName: "SubCategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategoryToy_ToysID",
                table: "SubCategoryToy",
                newName: "IX_SubCategoryToy_ToysId");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "SubCategories",
                newName: "IdSubCategory");

            migrationBuilder.RenameColumn(
                name: "ToyID",
                table: "Images",
                newName: "ToyId");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Images",
                newName: "IdImage");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ToyID",
                table: "Images",
                newName: "IX_Images_ToyId");

            migrationBuilder.RenameColumn(
                name: "ColourId",
                table: "Colours",
                newName: "IdColour");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "Brands",
                newName: "IdBrand");

            migrationBuilder.RenameColumn(
                name: "ToysID",
                table: "AgeGroupToy",
                newName: "ToysId");

            migrationBuilder.RenameColumn(
                name: "AgeGroupsID",
                table: "AgeGroupToy",
                newName: "AgeGroupsId");

            migrationBuilder.RenameIndex(
                name: "IX_AgeGroupToy_ToysID",
                table: "AgeGroupToy",
                newName: "IX_AgeGroupToy_ToysId");

            migrationBuilder.RenameColumn(
                name: "AgeGroupId",
                table: "AgeGroups",
                newName: "IdAgeGroup");

            migrationBuilder.AddColumn<string>(
                name: "CategoryID",
                table: "SubCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryID",
                table: "SubCategories",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_AgeGroupToy_AgeGroups_AgeGroupsId",
                table: "AgeGroupToy",
                column: "AgeGroupsId",
                principalTable: "AgeGroups",
                principalColumn: "IdAgeGroup",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AgeGroupToy_Toys_ToysId",
                table: "AgeGroupToy",
                column: "ToysId",
                principalTable: "Toys",
                principalColumn: "IdToy",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Toys_ToyId",
                table: "Images",
                column: "ToyId",
                principalTable: "Toys",
                principalColumn: "IdToy",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Categories_CategoryID",
                table: "SubCategories",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategoryToy_SubCategories_SubCategoriesId",
                table: "SubCategoryToy",
                column: "SubCategoriesId",
                principalTable: "SubCategories",
                principalColumn: "IdSubCategory",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategoryToy_Toys_ToysId",
                table: "SubCategoryToy",
                column: "ToysId",
                principalTable: "Toys",
                principalColumn: "IdToy",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Toys_Brands_BrandId",
                table: "Toys",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "IdBrand",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Toys_Colours_ColourId",
                table: "Toys",
                column: "ColourId",
                principalTable: "Colours",
                principalColumn: "IdColour",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
