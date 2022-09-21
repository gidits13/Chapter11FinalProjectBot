using Chapter11FinalProjectBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Chapter11FinalProjectBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage memoryStorage)
        {
            _telegramClient = telegramClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;


            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).actionCode = callbackQuery.Data;
            // Отправляем в ответ уведомление о выборе
            string mode = callbackQuery.Data switch
            {
                "sum" => " Суммирование чисел",
                "count" => " Подсчет колличества символов",
                "choice" => "Ручной режим",
                "qsum" => "Суммирование чисел",
                "qcount" => "Подсчет колличества символов",
                _ => String.Empty
            };
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Режим работы - {mode}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);

            switch (callbackQuery.Data)
            {
                case "qsum":
                    {
                        string errorMessage = "";
                        string text = _memoryStorage.GetSession(callbackQuery.From.Id).message;
                        var n = Sum.GetNumbersFromMessage(text, out errorMessage);
                        if(errorMessage != "")
                        {
                            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, errorMessage, cancellationToken: ct, parseMode: ParseMode.Html);
                        }
                        else
                        {
                            var s = Sum.SumNumbers(n);
                            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Сумма чисел: {s}", cancellationToken: ct, parseMode: ParseMode.Html);
                        }
                        _memoryStorage.GetSession(callbackQuery.From.Id).actionCode = "choice";
                        break;
                    }
                case "qcount":
                    {
                        string text = _memoryStorage.GetSession(callbackQuery.From.Id).message;
                        var c = Count.MessageLenght(text);
                        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Количество символов в сообщении: {c}", cancellationToken: ct, parseMode: ParseMode.Html);
                        _memoryStorage.GetSession(callbackQuery.From.Id).actionCode = "choice";
                        break;

                    }
            }

/*            // Генерим информационное сообщение
            string languageText = callbackQuery.Data switch
            {
                "ru" => " Русский",
                "en" => " Английский",
                _ => String.Empty
            };
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);*/
        }
    }
}
