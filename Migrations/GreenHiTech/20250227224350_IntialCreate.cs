using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenHiTech.Migrations.GreenHiTech
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressDetails",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    HouseNumber = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Street = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Province = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AddressD__A7C03E18B775225D", x => x.PkID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(75)", unicode: false, maxLength: 75, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__A7C03E183DBBDE88", x => x.PkID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    FkAddressID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__A7C03E180177EC3E", x => x.PkID);
                    table.ForeignKey(
                        name: "FKUser458995",
                        column: x => x.FkAddressID,
                        principalTable: "AddressDetails",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    FkCategoryID = table.Column<int>(type: "int", nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__A7C03E185AF044F3", x => x.PkID);
                    table.ForeignKey(
                        name: "FKProduct442106",
                        column: x => x.FkCategoryID,
                        principalTable: "Category",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUserID = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__A7C03E1877B52D70", x => x.PkID);
                    table.ForeignKey(
                        name: "FKCart316510",
                        column: x => x.FkUserID,
                        principalTable: "User",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUserID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(75)", unicode: false, maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__A7C03E18DFEDDD55", x => x.PkID);
                    table.ForeignKey(
                        name: "FKOrder677398",
                        column: x => x.FkUserID,
                        principalTable: "User",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AltText = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    FkProductID = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    CreateDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductI__A7C03E18C7186903", x => x.PkID);
                    table.ForeignKey(
                        name: "FKProductIma467861",
                        column: x => x.FkProductID,
                        principalTable: "Product",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateTable(
                name: "CartProduct",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkCartID = table.Column<int>(type: "int", nullable: false),
                    FkProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartProd__A7C03E188434383B", x => x.PkID);
                    table.ForeignKey(
                        name: "FKCartProduc150139",
                        column: x => x.FkCartID,
                        principalTable: "Cart",
                        principalColumn: "PkID");
                    table.ForeignKey(
                        name: "FKCartProduc893105",
                        column: x => x.FkProductID,
                        principalTable: "Product",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkOrderID = table.Column<int>(type: "int", nullable: false),
                    FkProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__A7C03E1842399867", x => x.PkID);
                    table.ForeignKey(
                        name: "FKOrderDetai54985",
                        column: x => x.FkOrderID,
                        principalTable: "Order",
                        principalColumn: "PkID");
                    table.ForeignKey(
                        name: "FKOrderDetai737279",
                        column: x => x.FkProductID,
                        principalTable: "Product",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkOderID = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__A7C03E18DCD08483", x => x.PkID);
                    table.ForeignKey(
                        name: "FKPayment376314",
                        column: x => x.FkOderID,
                        principalTable: "Order",
                        principalColumn: "PkID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_FkUserID",
                table: "Cart",
                column: "FkUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CartProduct_FkCartID",
                table: "CartProduct",
                column: "FkCartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartProduct_FkProductID",
                table: "CartProduct",
                column: "FkProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_FkUserID",
                table: "Order",
                column: "FkUserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_FkOrderID",
                table: "OrderDetails",
                column: "FkOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_FkProductID",
                table: "OrderDetails",
                column: "FkProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_FkOderID",
                table: "Payment",
                column: "FkOderID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_FkCategoryID",
                table: "Product",
                column: "FkCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_FkProductID",
                table: "ProductImage",
                column: "FkProductID");

            migrationBuilder.CreateIndex(
                name: "EmailUniqueness",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_FkAddressID",
                table: "User",
                column: "FkAddressID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartProduct");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "AddressDetails");
        }
    }
}
