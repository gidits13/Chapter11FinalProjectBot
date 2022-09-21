using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Chapter11FinalProjectBot.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Chapter11FinalProjectBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memmoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memmoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            if (message.Text == "/start")
            {
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                        InlineKeyboardButton.WithCallbackData($" Суммирование чисел" , $"sum"),
                        InlineKeyboardButton.WithCallbackData($" Подсчет колличества символов" , $"count"),
                        InlineKeyboardButton.WithCallbackData($" Ручной выбор действия" , $"choice")
                 });
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> выберите режим работы.</b> {Environment.NewLine} Доступно 3 режима{Environment.NewLine}" +
                    $"1 - Суммирование чисел, Бот будет считать Сумму чисел каждого сообщения{ Environment.NewLine}" +
                    $"2 - Подсчет колличесва символов в сообщении, Бот будет счиать символы для каждого сообщения{Environment.NewLine}" +
                    $"3 - Ручной режим, Бот будет предлагать выбор действия для каждого сообщения",
                    cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

            }
            else
            {
                var actionCode = _memoryStorage.GetSession(message.Chat.Id).actionCode;
                switch (actionCode)
                {
                    case "xxx":
                        {
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Не выбран режим работы. Нажмите menu->start", cancellationToken: ct);
                            break;
                        }
                    case "sum":
                        {
                            //вызываем сервис суммирования.обновляем сессию
                            _memoryStorage.GetSession(message.Chat.Id).message = message.Text;
                            string errorMessage = "";
                            var n = Sum.GetNumbersFromMessage(_memoryStorage.GetSession(message.Chat.Id).message, out errorMessage);
                            if (errorMessage != "")
                            {
                                await _telegramClient.SendTextMessageAsync(message.Chat.Id, errorMessage, cancellationToken: ct, parseMode: ParseMode.Html);
                            }
                            else
                            {
                                var s = Sum.SumNumbers(n);
                                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {s}", cancellationToken: ct, parseMode: ParseMode.Html);
                            }
                            break;
                        }
                    case "count":
                        {
                            _memoryStorage.GetSession(message.Chat.Id).message = message.Text;
                            var c = Count.MessageLenght(_memoryStorage.GetSession(message.Chat.Id).message);
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Количество символов в сообщении: {c}", cancellationToken: ct, parseMode: ParseMode.Html);
                            break;
                        }
                    case "choice":
                        {
                            _memoryStorage.GetSession(message.Chat.Id).message = message.Text;
                            var buttons = new List<InlineKeyboardButton[]>();
                            buttons.Add(new[]
                            {
                                InlineKeyboardButton.WithCallbackData($" Суммирование чисел" , $"qsum"),
                                InlineKeyboardButton.WithCallbackData($" Подсчет колличества символов" , $"qcount")
                        
                 });
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> выберите режим работы.</b> {Environment.NewLine}",
                                cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                            break;
                        }
                }
            }
        }
    }
}
