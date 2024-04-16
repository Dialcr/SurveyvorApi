using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class add_Application_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "SatiscationState", table: "Surveys");

            migrationBuilder.RenameColumn(
                name: "facultiesNumber",
                table: "University",
                newName: "FacultiesNumber"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Surveys",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Surveys",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<string>(
                name: "Tittle",
                table: "Surveys",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea"
            );

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    ApplicationState = table.Column<int>(type: "integer", nullable: false),
                    SurveyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Applications_SurveyId",
                table: "Applications",
                column: "SurveyId",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Applications");

            migrationBuilder.DropColumn(name: "Available", table: "Surveys");

            migrationBuilder.DropColumn(name: "Tittle", table: "Surveys");

            migrationBuilder.RenameColumn(
                name: "FacultiesNumber",
                table: "University",
                newName: "facultiesNumber"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Surveys",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100
            );

            migrationBuilder.AddColumn<string>(
                name: "SatiscationState",
                table: "Surveys",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "AspNetUsers",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100
            );
        }
    }
}
