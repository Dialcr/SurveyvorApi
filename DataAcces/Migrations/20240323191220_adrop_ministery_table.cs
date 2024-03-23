using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class adrop_ministery_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organization_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Organization_MinisteryId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Organization_OrganizationId",
                table: "Surveys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organization",
                table: "Organization");

            migrationBuilder.DropIndex(
                name: "IX_Organization_MinisteryId",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "MinisteryId",
                table: "Organization");

            migrationBuilder.RenameTable(
                name: "Organization",
                newName: "University");

            migrationBuilder.AddPrimaryKey(
                name: "PK_University",
                table: "University",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_University_Name",
                table: "University",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_University_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId",
                principalTable: "University",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_University_OrganizationId",
                table: "Surveys",
                column: "OrganizationId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_University_OrganizationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_University_OrganizationId",
                table: "Surveys");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_University",
                table: "University");

            migrationBuilder.DropIndex(
                name: "IX_University_Name",
                table: "University");

            migrationBuilder.RenameTable(
                name: "University",
                newName: "Organization");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Organization",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MinisteryId",
                table: "Organization",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organization",
                table: "Organization",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_MinisteryId",
                table: "Organization",
                column: "MinisteryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organization_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Organization_MinisteryId",
                table: "Organization",
                column: "MinisteryId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Organization_OrganizationId",
                table: "Surveys",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
