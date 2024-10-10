using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Повышению что существующий качества в участия национальный укрепления.", "Великолепный Стальной Плащ", 2959.63m, "Популярный" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Существующий модель инновационный нашей технологий обеспечение сфера сознания административных.", "Практичный Гранитный Компьютер", 8420.03m, "Рекомендуемый" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Принципов очевидна же работы национальный.", "Лоснящийся Кожанный Клатч", 4649.85m, "Новинка" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 2", "Повседневная дальнейших другой.", "Потрясающий Бетонный Ремень", 9391.82m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 1", "Технологий внедрения административных.", "Грубый Хлопковый Автомобиль", 7300.37m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Рамки обучения выполнять представляет шагов.", "Маленький Натуральный Автомобиль", 1822.11m, "Рекомендуемый" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Влечёт обществом изменений напрямую и систему общества.", "Свободный Резиновый Кошелек", 6976.91m, "Рекомендуемый" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 3", "Что проблем новая работы таким от широким модель.", "Невероятный Бетонный Стул", 6787.90m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Обеспечивает поставленных сфера собой понимание проверки консультация особенности выбранный.", "Потрясающий Пластиковый Кепка", 7127.75m, "Популярный" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Напрямую представляет обуславливает укрепления гражданского влечёт.", "Эргономичный Хлопковый Сабо", 7600.42m, "Популярный" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Для управление ресурсосберегающих сознания высокотехнологичная экономической социально-экономическое модель постоянное.", "Лоснящийся Натуральный Берет", 9018.69m, "Рекомендуемый" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Консультация путь массового нас социально-ориентированный другой.", "Фантастический Резиновый Куртка", 7347.66m, "Новинка" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Профессионального создаёт консультация.", "Большой Деревянный Стол", 6349.97m, "Рекомендуемый" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 1", "Повышение административных инновационный уровня сомнений занимаемых активом значение.", "Эргономичный Пластиковый Носки", 2245.93m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 2", "Проект вызывает новая постоянный национальный всего управление.", "Свободный Бетонный Сабо", 2585.76m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Условий для напрямую предпосылки позиции.", "Великолепный Натуральный Портмоне", 1493.42m, "Новинка" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Значимость модель начало соответствующих повышение оценить начало новых принципов.", "Эргономичный Меховой Компьютер", 8553.67m, "Новинка" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 1", "Активности проблем структуры.", "Фантастический Пластиковый Плащ", 5364.48m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Богатый намеченных широкому.", "Большой Натуральный Сабо", 7619.50m, "Новинка" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Равным другой гражданского дальнейшее равным создание.", "Невероятный Натуральный Майка", 4.89m, "Рекомендуемый" });
        }
    }
}
