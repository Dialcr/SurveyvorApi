using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class add_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks");

            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAsks_Description",
                table: "SurveyAsks",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_ResponseValue",
                table: "ResponsePosibilities",
                column: "ResponseValue");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities",
                column: "SuveryAskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SurveyAsks_Description",
                table: "SurveyAsks");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks");

            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_ResponseValue",
                table: "ResponsePosibilities");

            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks",
                column: "SurveyId")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities",
                column: "SuveryAskId")
                .Annotation("Npgsql:IndexMethod", "hash");
        }
    }
}
