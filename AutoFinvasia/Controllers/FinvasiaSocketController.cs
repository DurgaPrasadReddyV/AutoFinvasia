using AutoFinvasia.Clients;
using AutoFinvasia.Clients.Models;
using AutoFinvasia.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using System;
using System.Text;

namespace AutoFinvasia.Controllers
{
    [Route("api/finvasia-socket")]
    [ApiController]
    public class FinvasiaSocketController : ControllerBase
    {
        private readonly AutoFinvasiaDbContext _dbContext;
        private readonly FinvasiaSocketClient _finvasiaSocketClient;

        public FinvasiaSocketController(AutoFinvasiaDbContext dbContext, FinvasiaSocketClient finvasiaSocketClient)
        {
            _dbContext = dbContext;
            _finvasiaSocketClient = finvasiaSocketClient;
        }

        [HttpPost("connect")]
        public async Task Connect([FromBody] int id)
        {
            await _finvasiaSocketClient.ConnectAsync(id);
        }

        [HttpPost("disconnect")]
        public async Task Disconnect()
        {
            await _finvasiaSocketClient.DisconnectAsync();
        }
    }
}
