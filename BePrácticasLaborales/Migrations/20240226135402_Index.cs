using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BePrácticasLaborales.Migrations
{
    /// <inheritdoc />
    public partial class Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_Id_SuveryAskId",
                table: "ResponsePosibilities");

            migrationBuilder.AddColumn<int>(
                name: "SurveyAskId",
                table: "SurveyResponses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyAskId",
                table: "SurveyResponses",
                column: "SurveyAskId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_Id_SuveryAskId",
                table: "ResponsePosibilities",
                columns: new[] { "Id", "SuveryAskId" })
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_SurveyAsks_SurveyAskId",
                table: "SurveyResponses",
                column: "SurveyAskId",
                principalTable: "SurveyAsks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_SurveyAsks_SurveyAskId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_SurveyAskId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_Id_SuveryAskId",
                table: "ResponsePosibilities");

            migrationBuilder.DropColumn(
                name: "SurveyAskId",
                table: "SurveyResponses");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_Id_SuveryAskId",
                table: "ResponsePosibilities",
                columns: new[] { "Id", "SuveryAskId" });
        }
    }
}
