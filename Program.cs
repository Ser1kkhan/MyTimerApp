using System;
using System.Threading;
using NAudio.Wave;

class TimerApp
{
    static void Main()
    {
        // Запрашиваем у пользователя время в секундах для установки таймера
        Console.WriteLine("Введите время в секундах для таймера:");
        int seconds;
        while (!int.TryParse(Console.ReadLine(), out seconds) || seconds < 0)
        {
            Console.WriteLine("Пожалуйста, введите корректное число секунд:");
        }

        // Выводим сообщение о установленном времени таймера
        Console.WriteLine("Таймер установлен на {0} секунд.", seconds);
        Console.WriteLine("Для паузы/возобновления нажмите 'p', для выхода - 'q'.");
        Console.WriteLine("");

        // Переменные для отслеживания позиции курсора и состояния таймера
        int timeLine = Console.CursorTop - 1;
        int messageLine = Console.CursorTop;
        bool isPaused = false;
        string message = "";

        // Основной цикл таймера
        while (seconds > 0)
        {
            // Уменьшаем оставшееся время на 1 секунду, если таймер не на паузе
            if (!isPaused)
            {
                Console.SetCursorPosition(0, timeLine);
                Console.WriteLine("Оставшееся время: {0} секунд   ", seconds);
                Thread.Sleep(1000);
                seconds--;
            }

            // Обработка нажатий клавиш для управления таймером
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.P)
                {
                    // Переключаем состояние таймера на паузу или возобновление
                    isPaused = !isPaused;
                    message = isPaused ? "Таймер на паузе.                " : "Таймер возобновлен.              ";
                }
                else if (key.Key == ConsoleKey.Q)
                {
                    // Выход из цикла при нажатии клавиши 'q'
                    break;
                }
            }

            // Выводим сообщение о состоянии таймера (пауза/возобновление)
            Console.SetCursorPosition(0, messageLine);
            Console.WriteLine(message);
        }

        // При завершении времени выводим сообщение и воспроизводим звук
        if (seconds == 0)
        {
            Console.SetCursorPosition(0, timeLine);
            Console.WriteLine("Время вышло!                     ");
            PlaySound("C:\\Users\\Admin\\source\\repos\\timer3\\zvuk\\vzryiv-vo-vremya-boya.wav");
        }

        // Сообщение остановки таймера
        Console.WriteLine("Таймер остановлен.");
    }

    // Функция для воспроизведения звука
    private static void PlaySound(string soundFilePath)
    {
        try
        {
            // Пытаемся воспроизвести звук из указанного звукового файла
            using (var audioFile = new AudioFileReader(soundFilePath))
            using (var outputDevice = new WaveOutEvent())
            {
                // Инициализируем аудио файл и воспроизводим его
                outputDevice.Init(audioFile);
                outputDevice.Play();

                // Ожидаем завершения воспроизведения звука
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }
        catch (Exception ex)
        {
            // Выводим сообщение об ошибке, если не удалось воспроизвести звук
            Console.WriteLine("Не удалось воспроизвести звук: " + ex.Message);
        }
    }
}
