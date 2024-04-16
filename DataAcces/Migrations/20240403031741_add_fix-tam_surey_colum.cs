using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class add_fixtam_surey_colum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResponsePosibilities_SurveyAsks_SuveryAskId",
                table: "ResponsePosibilities"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Tittle",
                table: "Surveys",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Surveys",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100
            );

            migrationBuilder.AlterColumn<int>(
                name: "SuveryAskId",
                table: "ResponsePosibilities",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsePosibilities_SurveyAsks_SuveryAskId",
                table: "ResponsePosibilities",
                column: "SuveryAskId",
                principalTable: "SurveyAsks",
                principalColumn: "Id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResponsePosibilities_SurveyAsks_SuveryAskId",
                table: "ResponsePosibilities"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Tittle",
                table: "Surveys",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Surveys",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300
            );

            migrationBuilder.AlterColumn<int>(
                name: "SuveryAskId",
                table: "ResponsePosibilities",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsePosibilities_SurveyAsks_SuveryAskId",
                table: "ResponsePosibilities",
                column: "SuveryAskId",
                principalTable: "SurveyAsks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
