using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcces.Migrations
{
    /// <inheritdoc />
    public partial class surveyresponse_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_ResponsePosibilities_ResponsePosibilityId",
                table: "SurveyResponses"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_SurveyAsks_SuveryAskId",
                table: "SurveyResponses"
            );

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_ResponsePosibilityId",
                table: "SurveyResponses"
            );

            migrationBuilder.DropColumn(name: "ResponsePosibilityId", table: "SurveyResponses");

            migrationBuilder.RenameColumn(
                name: "SuveryAskId",
                table: "SurveyResponses",
                newName: "SurveyId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResponses_SuveryAskId",
                table: "SurveyResponses",
                newName: "IX_SurveyResponses_SurveyId"
            );

            migrationBuilder.CreateTable(
                name: "SurveyAskResponses",
                columns: table => new
                {
                    SurveyResponseId = table.Column<int>(type: "integer", nullable: false),
                    ResponsePosibilityId = table.Column<int>(type: "integer", nullable: false),
                    SuveryAskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_SurveyAskResponses",
                        x => new
                        {
                            x.SurveyResponseId,
                            x.SuveryAskId,
                            x.ResponsePosibilityId
                        }
                    );
                    table.ForeignKey(
                        name: "FK_SurveyAskResponses_ResponsePosibilities_ResponsePosibilityId",
                        column: x => x.ResponsePosibilityId,
                        principalTable: "ResponsePosibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_SurveyAskResponses_SurveyAsks_SuveryAskId",
                        column: x => x.SuveryAskId,
                        principalTable: "SurveyAsks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_SurveyAskResponses_SurveyResponses_SurveyResponseId",
                        column: x => x.SurveyResponseId,
                        principalTable: "SurveyResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAskResponses_ResponsePosibilityId",
                table: "SurveyAskResponses",
                column: "ResponsePosibilityId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAskResponses_SuveryAskId",
                table: "SurveyAskResponses",
                column: "SuveryAskId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyId",
                table: "SurveyResponses",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyId",
                table: "SurveyResponses"
            );

            migrationBuilder.DropTable(name: "SurveyAskResponses");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "SurveyResponses",
                newName: "SuveryAskId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResponses_SurveyId",
                table: "SurveyResponses",
                newName: "IX_SurveyResponses_SuveryAskId"
            );

            migrationBuilder.AddColumn<int>(
                name: "ResponsePosibilityId",
                table: "SurveyResponses",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_ResponsePosibilityId",
                table: "SurveyResponses",
                column: "ResponsePosibilityId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_ResponsePosibilities_ResponsePosibilityId",
                table: "SurveyResponses",
                column: "ResponsePosibilityId",
                principalTable: "ResponsePosibilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_SurveyAsks_SuveryAskId",
                table: "SurveyResponses",
                column: "SuveryAskId",
                principalTable: "SurveyAsks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
