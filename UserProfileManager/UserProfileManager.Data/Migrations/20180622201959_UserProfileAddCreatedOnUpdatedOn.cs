using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserProfileManager.Data.Migrations
{
    public partial class UserProfileAddCreatedOnUpdatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "UserProfiles",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOnUtc",
                table: "UserProfiles",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getutcdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedOnUtc",
                table: "UserProfiles");
        }
    }
}
