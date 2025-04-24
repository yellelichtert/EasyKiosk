using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyKiosk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4ad04c7-f08c-4651-9e10-bfa88174f724");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4fc00959-e40c-4a90-8254-70277f371a83");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "Order",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5f62d7a0-7bed-4284-a307-b89d8a56297c", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c7bbdd5e-6339-4892-b084-68996d2e4563", 0, "ed48eb56-7f54-4825-950c-b9eb198fdcf8", "admin@email.com", true, false, null, "ADMIN@EMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEHkExS12PWO1RoaphhbHZBRaJT2HssC07N+LD94d8wBTMRVmkeWBPP9Bh24VWMqChQ==", null, false, "2b844bc3-913f-4eeb-9161-e785ab4491c5", false, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeviceId",
                table: "Order",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Devices_DeviceId",
                table: "Order",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Devices_DeviceId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DeviceId",
                table: "Order");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f62d7a0-7bed-4284-a307-b89d8a56297c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c7bbdd5e-6339-4892-b084-68996d2e4563");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Order");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c4ad04c7-f08c-4651-9e10-bfa88174f724", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4fc00959-e40c-4a90-8254-70277f371a83", 0, "924e0438-f01b-48fe-a94a-6ae6859d5929", "admin@email.com", true, false, null, "ADMIN@EMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEASohES5N5m7RHxqOi7UgPcJihAXvy7/1ZZsZTNd8lpDn7WClKohyrb6sIsxCxA1rA==", null, false, "e4b21f67-5fa8-4657-bba2-83ce3b384171", false, "admin" });
        }
    }
}
