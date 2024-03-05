using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class testMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_ResponsePosibilities_ResponsePosibilityId_S~",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_ResponsePosibilityId_SuveryAskId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponsePosibilities",
                table: "ResponsePosibilities");

            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_Id_SuveryAskId",
                table: "ResponsePosibilities");

            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities");

            migrationBuilder.AlterColumn<int>(
                name: "SuveryAskId",
                table: "ResponsePosibilities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ResponsePosibilities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponsePosibilities",
                table: "ResponsePosibilities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_ResponsePosibilityId",
                table: "SurveyResponses",
                column: "ResponsePosibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SuveryAskId",
                table: "SurveyResponses",
                column: "SuveryAskId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_ResponsePosibilities_ResponsePosibilityId",
                table: "SurveyResponses",
                column: "ResponsePosibilityId",
                principalTable: "ResponsePosibilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_SurveyAsks_SuveryAskId",
                table: "SurveyResponses",
                column: "SuveryAskId",
                principalTable: "SurveyAsks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_ResponsePosibilities_ResponsePosibilityId",
                table: "SurveyResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_SurveyAsks_SuveryAskId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_ResponsePosibilityId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_SuveryAskId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponsePosibilities",
                table: "ResponsePosibilities");

            migrationBuilder.DropIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities");

            migrationBuilder.AlterColumn<int>(
                name: "SuveryAskId",
                table: "ResponsePosibilities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ResponsePosibilities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponsePosibilities",
                table: "ResponsePosibilities",
                columns: new[] { "Id", "SuveryAskId" });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_ResponsePosibilityId_SuveryAskId",
                table: "SurveyResponses",
                columns: new[] { "ResponsePosibilityId", "SuveryAskId" });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_Id_SuveryAskId",
                table: "ResponsePosibilities",
                columns: new[] { "Id", "SuveryAskId" });

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities",
                column: "SuveryAskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_ResponsePosibilities_ResponsePosibilityId_S~",
                table: "SurveyResponses",
                columns: new[] { "ResponsePosibilityId", "SuveryAskId" },
                principalTable: "ResponsePosibilities",
                principalColumns: new[] { "Id", "SuveryAskId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
