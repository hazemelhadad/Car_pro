using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyVechile.Migrations
{
    /// <inheritdoc />
    public partial class NewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeesVehicle");

            migrationBuilder.CreateTable(
                name: "EmployeesVehicles",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    VehiclePlateNumber = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesVehicles", x => new { x.VehiclePlateNumber, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeesVehicles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Employee_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeesVehicles_Vehicle_VehiclePlateNumber",
                        column: x => x.VehiclePlateNumber,
                        principalTable: "Vehicle",
                        principalColumn: "Vehicle_PlateNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesVehicles_EmployeeId",
                table: "EmployeesVehicles",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeesVehicles");

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
        }
    }
}
