using AutoFinvasia.Clients.Models;
using AutoFinvasia.Entities;
using AutoFinvasia.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Xml.Linq;
using Websocket.Client;

namespace AutoFinvasia.Clients
{
    public class FinvasiaSocketClient : IAsyncDisposable
    {
        private readonly FinvasiaSocketOptions _finvasiaSocketOptions;
        private readonly IWebsocketClient _socketClient;
        private readonly ILogger<FinvasiaSocketClient> _logger;
        private readonly IServiceProvider _serviceProvider;
        private FinvasiaCredentials? finvasiaCredentials;

        public FinvasiaSocketClient(IOptions<FinvasiaSocketOptions> finvasiaSocketOptions, ILogger<FinvasiaSocketClient> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _finvasiaSocketOptions = finvasiaSocketOptions.Value;

            var factory = new Func<ClientWebSocket>(() =>
            {
                var client = new ClientWebSocket
                {
                    Options =
                    {
                        KeepAliveInterval = TimeSpan.Zero
                    }
                };
                return client;
            });

            _socketClient = new WebsocketClient(new Uri(_finvasiaSocketOptions.Uri), factory);

            _socketClient.Name = "FinvasiaWebSocket";
            _socketClient.ReconnectTimeout = TimeSpan.FromSeconds(_finvasiaSocketOptions.ReconnectTimeout);
            _socketClient.ErrorReconnectTimeout = TimeSpan.FromSeconds(_finvasiaSocketOptions.ErrorReconnectTimeout);
        }

        public async ValueTask DisposeAsync()
        {
            if (_socketClient == null)
                return;

            await _socketClient.Stop(WebSocketCloseStatus.NormalClosure, string.Empty);
            _socketClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task ConnectAsync(int id)
        {
            if (_socketClient == null || _socketClient.IsRunning)
                return;

            using (var migrationScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = migrationScope.ServiceProvider.GetRequiredService<AutoFinvasiaDbContext>())
                {
                    finvasiaCredentials = await context.FinvasiaCredentials.FindAsync(id) ?? new();
                }
            }

            var connectMessage = new ConnectMessage();
            connectMessage.uid = finvasiaCredentials.UserID;
            connectMessage.susertoken = finvasiaCredentials.AccessToken;
            connectMessage.t = "c";
            connectMessage.actid = finvasiaCredentials.UserID;

            _socketClient.ReconnectionHappened.Subscribe(info =>
            {
                _logger.LogInformation($"Reconnection happened, type: {info.Type}, url: {_socketClient.Url}");
                _socketClient.Send(JsonSerializer.Serialize(connectMessage));
            });

            _socketClient.DisconnectionHappened.Subscribe(info =>
                _logger.LogWarning($"Disconnection happened, type: {info.Type}"));

            _socketClient.MessageReceived.Subscribe(msg =>
            {
                _logger.LogInformation($"Message received: {msg}");
            });

            _socketClient.IsTextMessageConversionEnabled = false;

            await _socketClient.Start();
            _socketClient.Send(JsonSerializer.Serialize(connectMessage));

            var subscribeDepth = new SubscribeDepth();
            subscribeDepth.k = "NSE" + "|" + "22";
            subscribeDepth.t = "d";
            _socketClient.Send(JsonSerializer.Serialize(subscribeDepth));
        }

        public async Task DisconnectAsync()
        {
            if (_socketClient == null)
                return;

            await _socketClient.Stop(WebSocketCloseStatus.NormalClosure, string.Empty);
        }
    }
}
