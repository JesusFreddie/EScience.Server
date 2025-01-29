using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EScinece.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeArticle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeArticle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    HashedPassword = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_TypeArticle_TypeArticleId",
                        column: x => x.TypeArticleId,
                        principalTable: "TypeArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleParticipant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleParticipant_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleParticipant_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleParticipant_Article_ArticleId1",
                        column: x => x.ArticleId1,
                        principalTable: "Article",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArticleBranch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleBranch_ArticleParticipant_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "ArticleParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleBranch_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleBranchVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleBranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleBranchVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleBranchVersion_ArticleBranch_ArticleBranchId",
                        column: x => x.ArticleBranchId,
                        principalTable: "ArticleBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleBranchVersion_ArticleParticipant_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "ArticleParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_TypeArticleId",
                table: "Article",
                column: "TypeArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleBranch_ArticleId",
                table: "ArticleBranch",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleBranch_CreatorId",
                table: "ArticleBranch",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleBranchVersion_ArticleBranchId",
                table: "ArticleBranchVersion",
                column: "ArticleBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleBranchVersion_CreatorId",
                table: "ArticleBranchVersion",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleParticipant_AccountId",
                table: "ArticleParticipant",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleParticipant_ArticleId",
                table: "ArticleParticipant",
                column: "ArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleParticipant_ArticleId1",
                table: "ArticleParticipant",
                column: "ArticleId1");

            migrationBuilder.CreateIndex(
                name: "IX_TypeArticle_Name",
                table: "TypeArticle",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleBranchVersion");

            migrationBuilder.DropTable(
                name: "ArticleBranch");

            migrationBuilder.DropTable(
                name: "ArticleParticipant");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "TypeArticle");
        }
    }
}
