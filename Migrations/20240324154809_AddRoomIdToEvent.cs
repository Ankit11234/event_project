using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_project_1.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomIdToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rooms",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Rooms",
                newName: "room_id");

            migrationBuilder.AddColumn<string>(
                name: "room_name",
                table: "Rooms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Events",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "room_name",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Rooms",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "room_id",
                table: "Rooms",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Rooms",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
