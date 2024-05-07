using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyVechile.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Branch_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Branch_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Branch_Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Branch_ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Employee_ID = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Employee_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_Birthday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_Street_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employee_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Branch_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Employee_ID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Vehicle_PlateNumber = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    License_SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    License_Registeration = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    License_ExpirationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vehicle_ChassisNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vehicle_ManufactureYear = table.Column<int>(type: "int", nullable: false),
                    Vehicle_BrandName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Vehicle_Color = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Vehicle_Type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Vehicle_Insurance = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Branch_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Vehicle_PlateNumber);
                    table.ForeignKey(
                        name: "FK_Vehicle_Branches_Branch_ID",
                        column: x => x.Branch_ID,
                        principalTable: "Branches",
                        principalColumn: "Branch_ID");
                });

            migrationBuilder.CreateTable(
                name: "EmployeePhones",
                columns: table => new
                {
                    Employee_ID = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Employee_PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePhones", x => new { x.Employee_ID, x.Employee_PhoneNumber });
                    table.ForeignKey(
                        name: "FK_EmployeePhones_Employees_Employee_ID",
                        column: x => x.Employee_ID,
                        principalTable: "Employees",
                        principalColumn: "Employee_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesVehicle",
                columns: table => new
                {
                    EmployeesEmployee_ID = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Vehicle_PlateNumber = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesVehicle", x => new { x.EmployeesEmployee_ID, x.Vehicle_PlateNumber });
                    table.ForeignKey(
                        name: "FK_EmployeesVehicle_Employees_EmployeesEmployee_ID",
                        column: x => x.EmployeesEmployee_ID,
                        principalTable: "Employees",
                        principalColumn: "Employee_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeesVehicle_Vehicle_Vehicle_PlateNumber",
                        column: x => x.Vehicle_PlateNumber,
                        principalTable: "Vehicle",
                        principalColumn: "Vehicle_PlateNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesVehicle_Vehicle_PlateNumber",
                table: "EmployeesVehicle",
                column: "Vehicle_PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Branch_ID",
                table: "Vehicle",
                column: "Branch_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePhones");

            migrationBuilder.DropTable(
                name: "EmployeesVehicle");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
