using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Notes.Migrations
{
    /// <inheritdoc />
    public partial class NotesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Password = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UserID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_notes_tb_users_UserID",
                        column: x => x.UserID,
                        principalTable: "tb_users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_notes_UserID",
                table: "tb_notes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_users_Email",
                table: "tb_users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_users_Username",
                table: "tb_users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_notes");

            migrationBuilder.DropTable(
                name: "tb_users");
        }
    }
}
