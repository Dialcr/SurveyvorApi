using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class add_image_into_user_university_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "BgImage",
                table: "University",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "University",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "University",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "facultiesNumber",
                table: "University",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "AspNetUsers",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgImage",
                table: "University");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "University");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "University");

            migrationBuilder.DropColumn(
                name: "facultiesNumber",
                table: "University");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");
        }
    }
}
