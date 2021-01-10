using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoListWebApp.Data.Migrations
{
    public partial class CorrectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ListItem");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "ListItem");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ListItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateTime",
                table: "ListItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ListItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "ListItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LimitDateTime",
                table: "ListItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateTime",
                table: "ListItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDateTime",
                table: "ListItem");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ListItem");

            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "ListItem");

            migrationBuilder.DropColumn(
                name: "LimitDateTime",
                table: "ListItem");

            migrationBuilder.DropColumn(
                name: "UpdateDateTime",
                table: "ListItem");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ListItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ListItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ListItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
