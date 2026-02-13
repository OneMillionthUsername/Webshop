using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentRepositoryFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    PaymentDetailType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CardBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Last4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryMonth = table.Column<int>(type: "int", nullable: true),
                    ExpiryYear = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EpsPaymentDetail_ProviderPaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderPaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_PaymentId",
                table: "PaymentDetails",
                column: "PaymentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDetails");
        }
    }
}
