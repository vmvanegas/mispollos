using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mispollos.Persistence.Migrations
{
    public partial class emailfieldaddedtocliententity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Cliente");
        }
    }
}