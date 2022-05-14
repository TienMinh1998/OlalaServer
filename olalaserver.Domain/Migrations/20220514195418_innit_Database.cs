using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace APIProject.Domain.Migrations
{
    public partial class innit_Database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    BonusPointPerKg = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<string>(maxLength: 100, nullable: true),
                    ProvinceCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    TypeNews = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    UrlImage = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CustomerID = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Viewed = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: true),
                    NewsID = table.Column<int>(nullable: true),
                    ProductID = table.Column<int>(nullable: true),
                    IsAdmin = table.Column<int>(nullable: false),
                    CustomerPhone = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<string>(maxLength: 100, nullable: true),
                    District_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    LatinName = table.Column<string>(nullable: true),
                    EnglishName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Size = table.Column<string>(maxLength: 150, nullable: true),
                    Origin = table.Column<string>(maxLength: 150, nullable: true),
                    Ingredient = table.Column<string>(maxLength: 150, nullable: true),
                    Usage = table.Column<string>(maxLength: 150, nullable: true),
                    StorageTemperature = table.Column<string>(maxLength: 150, nullable: true),
                    NetWeight = table.Column<double>(nullable: false),
                    MinQuantityStorage = table.Column<int>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: true),
                    CategoryID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(maxLength: 15, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    DeviceID = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    CodeTax = table.Column<string>(nullable: true),
                    CustomerTypeID = table.Column<int>(nullable: false),
                    Point = table.Column<long>(nullable: false),
                    IsConfirmRole = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ExpireDateOTP = table.Column<DateTime>(nullable: true),
                    OTP = table.Column<string>(nullable: true),
                    QtyOTP = table.Column<int>(nullable: false),
                    IsConfirmOTP = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerTypes_CustomerTypeID",
                        column: x => x.CustomerTypeID,
                        principalTable: "CustomerTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    PermissionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(maxLength: 100, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true),
                    RoleID = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageExports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Customer = table.Column<string>(nullable: true),
                    ReceiverName = table.Column<string>(nullable: true),
                    NumberCar = table.Column<string>(nullable: true),
                    ExportDate = table.Column<DateTime>(nullable: false),
                    StorageID = table.Column<int>(nullable: false),
                    ProvinceID = table.Column<int>(nullable: false),
                    Condition = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<long>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageExports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StorageExports_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageExports_Storages_StorageID",
                        column: x => x.StorageID,
                        principalTable: "Storages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageImports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    ImportDate = table.Column<DateTime>(nullable: false),
                    StorageID = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageImports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StorageImports_Storages_StorageID",
                        column: x => x.StorageID,
                        principalTable: "Storages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ImageUrl = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    OriginalPrice = table.Column<long>(nullable: false),
                    CustomerTypeID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStorages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    StorageID = table.Column<int>(nullable: true),
                    ManufactureDate = table.Column<DateTime>(nullable: true),
                    ExpiredDate = table.Column<DateTime>(nullable: true),
                    Supplier = table.Column<string>(nullable: true),
                    LotNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStorages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductStorages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStorages_Storages_StorageID",
                        column: x => x.StorageID,
                        principalTable: "Storages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    TotalPrice = table.Column<long>(nullable: false),
                    UsePoint = table.Column<long>(nullable: true),
                    ShipFee = table.Column<long>(nullable: true),
                    ProductSumPrice = table.Column<long>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false),
                    BuyerName = table.Column<string>(maxLength: 100, nullable: true),
                    BuyerPhone = table.Column<string>(maxLength: 15, nullable: true),
                    BuyerAddress = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    NoteComplain = table.Column<string>(nullable: true),
                    DeclineRequest = table.Column<int>(nullable: true),
                    DeclineNote = table.Column<string>(nullable: true),
                    PaymentType = table.Column<int>(nullable: true),
                    PaymentStatus = table.Column<int>(nullable: false),
                    ShipQuoteStatus = table.Column<int>(nullable: false),
                    UrlVnpay = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    ProvinceID = table.Column<int>(nullable: false),
                    DistrictID = table.Column<int>(nullable: false),
                    WardID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Districts_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "Districts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveAddresses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    IsDefault = table.Column<int>(nullable: false),
                    ProvinceID = table.Column<int>(nullable: false),
                    DistrictID = table.Column<int>(nullable: false),
                    WardID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveAddresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceiveAddresses_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveAddresses_Districts_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "Districts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveAddresses_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveAddresses_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    SumPrice = table.Column<long>(nullable: false),
                    ProductItemID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Carts_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_ProductItems_ProductItemID",
                        column: x => x.ProductItemID,
                        principalTable: "ProductItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStorageHistories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProductStorageID = table.Column<int>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStorageHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductStorageHistories_ProductStorages_ProductStorageID",
                        column: x => x.ProductStorageID,
                        principalTable: "ProductStorages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageExportDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    StorageExportID = table.Column<int>(nullable: false),
                    ProductStorageID = table.Column<int>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageExportDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StorageExportDetails_ProductStorages_ProductStorageID",
                        column: x => x.ProductStorageID,
                        principalTable: "ProductStorages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageExportDetails_StorageExports_StorageExportID",
                        column: x => x.StorageExportID,
                        principalTable: "StorageExports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageImportDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    StorageImportID = table.Column<int>(nullable: false),
                    ProductStorageID = table.Column<int>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<double>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageImportDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StorageImportDetails_ProductStorages_ProductStorageID",
                        column: x => x.ProductStorageID,
                        principalTable: "ProductStorages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageImportDetails_StorageImports_StorageImportID",
                        column: x => x.StorageImportID,
                        principalTable: "StorageImports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberPointHistories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Balance = table.Column<long>(nullable: false),
                    Point = table.Column<long>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    TypeAdd = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPointHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemberPointHistories_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberPointHistories_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderComplainImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ImageUrl = table.Column<string>(nullable: true),
                    OrderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderComplainImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderComplainImages_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    SumPrice = table.Column<long>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsActive = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderHistories_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerID",
                table: "Carts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductItemID",
                table: "Carts",
                column: "ProductItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerTypeID",
                table: "Customers",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPointHistories_CustomerID",
                table: "MemberPointHistories",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberPointHistories_OrderID",
                table: "MemberPointHistories",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComplainImages_OrderID",
                table: "OrderComplainImages",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_OrderID",
                table: "OrderHistories",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DistrictID",
                table: "Orders",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProvinceID",
                table: "Orders",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WardID",
                table: "Orders",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductID",
                table: "ProductImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_ProductID",
                table: "ProductItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStorageHistories_ProductStorageID",
                table: "ProductStorageHistories",
                column: "ProductStorageID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStorages_ProductID",
                table: "ProductStorages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStorages_StorageID",
                table: "ProductStorages",
                column: "StorageID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveAddresses_CustomerID",
                table: "ReceiveAddresses",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveAddresses_DistrictID",
                table: "ReceiveAddresses",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveAddresses_ProvinceID",
                table: "ReceiveAddresses",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveAddresses_WardID",
                table: "ReceiveAddresses",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionID",
                table: "RolePermissions",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleID",
                table: "RolePermissions",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageExportDetails_ProductStorageID",
                table: "StorageExportDetails",
                column: "ProductStorageID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageExportDetails_StorageExportID",
                table: "StorageExportDetails",
                column: "StorageExportID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageExports_ProvinceID",
                table: "StorageExports",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageExports_StorageID",
                table: "StorageExports",
                column: "StorageID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageImportDetails_ProductStorageID",
                table: "StorageImportDetails",
                column: "ProductStorageID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageImportDetails_StorageImportID",
                table: "StorageImportDetails",
                column: "StorageImportID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageImports_StorageID",
                table: "StorageImports",
                column: "StorageID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "MemberPointHistories");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderComplainImages");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderHistories");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductStorageHistories");

            migrationBuilder.DropTable(
                name: "ReceiveAddresses");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "StorageExportDetails");

            migrationBuilder.DropTable(
                name: "StorageImportDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "StorageExports");

            migrationBuilder.DropTable(
                name: "ProductStorages");

            migrationBuilder.DropTable(
                name: "StorageImports");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
