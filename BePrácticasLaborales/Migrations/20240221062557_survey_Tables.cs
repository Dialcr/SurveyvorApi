using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BePrácticasLaborales.Migrations
{
    /// <inheritdoc />
    public partial class survey_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SatiscationState = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyAsks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurveyId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAsks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyAsks_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponsePosibilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    SuveryAskId = table.Column<int>(type: "integer", nullable: false),
                    ResponseValue = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsePosibilities", x => new { x.Id, x.SuveryAskId });
                    table.ForeignKey(
                        name: "FK_ResponsePosibilities_SurveyAsks_SuveryAskId",
                        column: x => x.SuveryAskId,
                        principalTable: "SurveyAsks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponsePosibilityId = table.Column<int>(type: "integer", nullable: false),
                    SuveryAskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_ResponsePosibilities_ResponsePosibilityId_S~",
                        columns: x => new { x.ResponsePosibilityId, x.SuveryAskId },
                        principalTable: "ResponsePosibilities",
                        principalColumns: new[] { "Id", "SuveryAskId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_Id_SuveryAskId",
                table: "ResponsePosibilities",
                columns: new[] { "Id", "SuveryAskId" });

            migrationBuilder.CreateIndex(
                name: "IX_ResponsePosibilities_SuveryAskId",
                table: "ResponsePosibilities",
                column: "SuveryAskId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAsks_SurveyId",
                table: "SurveyAsks",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_ResponsePosibilityId_SuveryAskId",
                table: "SurveyResponses",
                columns: new[] { "ResponsePosibilityId", "SuveryAskId" });

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_OrganizationId",
                table: "Surveys",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyResponses");

            migrationBuilder.DropTable(
                name: "ResponsePosibilities");

            migrationBuilder.DropTable(
                name: "SurveyAsks");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
