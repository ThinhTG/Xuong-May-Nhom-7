using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarmentFactoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        //cập nhật IsActive cho các table
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TaskProducts",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AssemblyLines",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }


        /// <inheritdoc />
        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.CreateTable(
        //        name: "Categories",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Categories", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Roles",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Roles", x => x.Id);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Users",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            RoleId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Users", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Users_Roles_RoleId",
        //                column: x => x.RoleId,
        //                principalTable: "Roles",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Orders",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            TotalPrice = table.Column<double>(type: "float", nullable: false),
        //            UserId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Orders", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Orders_Users_UserId",
        //                column: x => x.UserId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "Products",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            Price = table.Column<double>(type: "float", nullable: false),
        //            CategoryId = table.Column<int>(type: "int", nullable: false),
        //            UserId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_Products", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_Products_Categories_CategoryId",
        //                column: x => x.CategoryId,
        //                principalTable: "Categories",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_Products_Users_UserId",
        //                column: x => x.UserId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "TaskProducts",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //            UserId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_TaskProducts", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_TaskProducts_Users_UserId",
        //                column: x => x.UserId,
        //                principalTable: "Users",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "OrderDetails",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Quantity = table.Column<int>(type: "int", nullable: false),
        //            ProductId = table.Column<int>(type: "int", nullable: false),
        //            OrderId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_OrderDetails", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_OrderDetails_Orders_OrderId",
        //                column: x => x.OrderId,
        //                principalTable: "Orders",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_OrderDetails_Products_ProductId",
        //                column: x => x.ProductId,
        //                principalTable: "Products",
        //                principalColumn: "Id");
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "AssemblyLines",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
        //            OrderDetailId = table.Column<int>(type: "int", nullable: false),
        //            TaskProductId = table.Column<int>(type: "int", nullable: false),
        //            UserId = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_AssemblyLines", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_AssemblyLines_OrderDetails_OrderDetailId",
        //                column: x => x.OrderDetailId,
        //                principalTable: "OrderDetails",
        //                principalColumn: "Id",
        //                onDelete: ReferentialAction.Cascade);
        //            table.ForeignKey(
        //                name: "FK_AssemblyLines_TaskProducts_TaskProductId",
        //                column: x => x.TaskProductId,
        //                principalTable: "TaskProducts",
        //                principalColumn: "Id");
        //            table.ForeignKey(
        //                name: "FK_AssemblyLines_Users_UserId",
        //                column: x => x.UserId,
        //                principalTable: "Users",
        //                principalColumn: "Id");
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "IX_AssemblyLines_OrderDetailId",
        //        table: "AssemblyLines",
        //        column: "OrderDetailId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_AssemblyLines_TaskProductId",
        //        table: "AssemblyLines",
        //        column: "TaskProductId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_AssemblyLines_UserId",
        //        table: "AssemblyLines",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_OrderDetails_OrderId",
        //        table: "OrderDetails",
        //        column: "OrderId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_OrderDetails_ProductId",
        //        table: "OrderDetails",
        //        column: "ProductId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Orders_UserId",
        //        table: "Orders",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Products_CategoryId",
        //        table: "Products",
        //        column: "CategoryId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Products_UserId",
        //        table: "Products",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_TaskProducts_UserId",
        //        table: "TaskProducts",
        //        column: "UserId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Users_RoleId",
        //        table: "Users",
        //        column: "RoleId");
        //}

        ///// <inheritdoc />
        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropTable(
        //        name: "AssemblyLines");

        //    migrationBuilder.DropTable(
        //        name: "OrderDetails");

        //    migrationBuilder.DropTable(
        //        name: "TaskProducts");

        //    migrationBuilder.DropTable(
        //        name: "Orders");

        //    migrationBuilder.DropTable(
        //        name: "Products");

        //    migrationBuilder.DropTable(
        //        name: "Categories");

        //    migrationBuilder.DropTable(
        //        name: "Users");

        //    migrationBuilder.DropTable(
        //        name: "Roles");
        //}
    }
}
