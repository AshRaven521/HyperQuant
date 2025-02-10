# Hyper Quant
Тестовое задание. Цель: разработать Desktop-приложение, позволяющее обрабатывать получение trades, candles, tickers по протоколам REST и WebSocket, а так же создать портфель с заданным кол-вом криптовалют и выполнить конвертацию.

## Содержание
- [Технологии](#технологии)
- [Использование](#использование)
- [Пример выполнения](#пример-выполнения)
- [To do](#to-do)

## Технологии
- [RestSharp](https://restsharp.dev/)

## Использование

Создайте локально файл конфигураций config.json [CoinLayerAPI](https://coinlayer.com/thank-you-free-api) для создания крипто-портфеля
```
{
    "CoinLayerApi": "your-api-key"
}
```

## Пример выполнения

### Стартовый экран

В верхней части экрана кнопки навигации по приложению, остальную часть окна занимает место для отображения страниц приложения.

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/start_screen.jpg)

### Отображение после нажатия кнопки "REST Trade"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/rest_trade_button.jpg)

### Результат после добавления требуемых данных и нажатия кнопки "Show Trades"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/rest_trade_result.jpg)

### Результат "REST Candle"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/rest_candles_result.jpg)

### Результат "REST Ticker"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/rest_ticker_result.jpg)

### Результат "Web Socket Trade"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/web_socket_trades.jpg)

### Отображение после нажатия кнопки "Crypto Brief Case"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/crypto_brief_button.jpg)

#### Выбор локального конфигурационного файла (пример файла в разделе [#использование]) после нажатия кнопки "Select config file"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/select_config_button.jpg)

#### Результат "Show Wallets"

![image_alt](https://github.com/AshRaven521/HyperQuant/blob/d58105907a5be366ca70b2bb3c96680af2d59f2f/screenshots/show_wallet_button.jpg)

## Разработка

### Требования
Для установки и запуска проекта, необходимы 
- [VS 2022](https://visualstudio.microsoft.com/ru/vs/)
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)



## To do
- Доработать получение trades, candles по WebSocket 

 
