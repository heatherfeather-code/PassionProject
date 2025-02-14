using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProjectYarnProjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectYarn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectYarn_ProjectId",
                table: "ProjectYarn",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectYarn_Projects_ProjectId",
                table: "ProjectYarn",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectYarn_Projects_ProjectId",
                table: "ProjectYarn");

            migrationBuilder.DropIndex(
                name: "IX_ProjectYarn_ProjectId",
                table: "ProjectYarn");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectYarn");
        }
    }
}
