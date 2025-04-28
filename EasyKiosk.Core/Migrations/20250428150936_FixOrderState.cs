using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyKiosk.Core.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2dafda20-06e5-4e73-b527-fac13d5b6c48");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "329db3e9-63ab-4781-af74-d5ab820a978c");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PayedPrice",
                table: "OrderDetail",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0d0d13b2-d6c6-4cbc-b9b7-45e25d8c1244", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e7210971-4f1a-4926-9553-202c1e1e8135", 0, "3f1acfee-af58-4cab-844a-19653511eb9a", "admin@email.com", true, false, null, "ADMIN@EMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEM+A2MPWKUFE6Zleoaixrn8tXzxbpQToagEaabpuhxazr7kHLC1GyHwiSlt3TFmp5Q==", null, false, "1d2df315-3ffc-4b9f-8a42-bdec45ccce8b", false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d0d13b2-d6c6-4cbc-b9b7-45e25d8c1244");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e7210971-4f1a-4926-9553-202c1e1e8135");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PayedPrice",
                table: "OrderDetail");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2dafda20-06e5-4e73-b527-fac13d5b6c48", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "329db3e9-63ab-4781-af74-d5ab820a978c", 0, "b6db6f5f-c1cf-41ec-8aca-c217cbf905e9", "admin@email.com", true, false, null, "ADMIN@EMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEEbMFP+ywhixhzjP6QlPxK+UB1D3eqgCDSG42Fs9lYIQGQWuZfTWI5IQB7tawq2pkw==", null, false, "c0af124f-a7d4-4115-b7e5-c372f5c29a70", false, "admin" });
        }
    }
}
