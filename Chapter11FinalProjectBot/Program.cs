using Chapter11FinalProjectBot.Controllers;
using Chapter11FinalProjectBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;

namespace Chapter11FinalProjectBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
           .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
           .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
           .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }
        static void ConfigureServices(IServiceCollection services)
        {
            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddSingleton<IStorage, MemoryStorage>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("5793699447:AAFavna7eJbYHNqBnQQWfizpkQySDOazQic"));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
    }
}