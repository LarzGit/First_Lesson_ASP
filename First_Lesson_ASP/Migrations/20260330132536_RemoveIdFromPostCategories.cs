using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace First_Lesson_ASP.Migrations
{
    public partial class RemoveIdFromPostCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 0. Очищаємо проміжну таблицю від дублікатів
            migrationBuilder.Sql("DELETE FROM PostCategories;");

            // 1. Спочатку знімаємо старий первинний ключ
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCategories",
                table: "PostCategories");

            // 2. Тепер безпечно видаляємо саму колонку Id
            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostCategories");

            // 3. Створюємо новий первинний ключ з двох колонок (композитний)
            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCategories",
                table: "PostCategories",
                columns: new[] { "PostId", "CategoryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Відкат: знімаємо композитний ключ
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCategories",
                table: "PostCategories");

            // Відкат: повертаємо колонку Id з автоінкрементом
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PostCategories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Відкат: робимо Id знову первинним ключем
            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCategories",
                table: "PostCategories",
                column: "Id");
        }
    }
}