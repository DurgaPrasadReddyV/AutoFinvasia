using AutoFinvasia.Clients;
using AutoFinvasia.Clients.Models;
using AutoFinvasia.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using System.Text;

namespace AutoFinvasia.Controllers
{
    [Route("api/finvasia-credentials")]
    [ApiController]
    public class FinvasiaCredentialsController : ControllerBase
    {
        private readonly AutoFinvasiaDbContext _dbContext;
        private readonly FinvasiaApiClient _finvasiaApiClient;

        public FinvasiaCredentialsController(AutoFinvasiaDbContext dbContext, FinvasiaApiClient finvasiaApiClient)
        {
            _dbContext = dbContext;
            _finvasiaApiClient = finvasiaApiClient;
        }

        [HttpGet]
        public async Task<IEnumerable<FinvasiaCredentials>> Get()
        {
            return await _dbContext.FinvasiaCredentials.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<FinvasiaCredentials> Get(int id)
        {
            return await _dbContext.FinvasiaCredentials.FindAsync(id) ?? new();
        }

        [HttpPost]
        public async Task Post([FromBody] FinvasiaCredentials value)
        {
            _dbContext.Add(value);
            await _dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] FinvasiaCredentials value)
        {
            var existingEntity = await _dbContext.FinvasiaCredentials.FindAsync(id) ?? new();
            existingEntity.IMEI = value.IMEI;
            existingEntity.VendorCode = value.VendorCode;
            existingEntity.PAN = value.PAN;
            existingEntity.APIKeyHash = value.APIKeyHash;
            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            _dbContext.FinvasiaCredentials.Remove(new FinvasiaCredentials() { Id = id });
            await _dbContext.SaveChangesAsync();
        }

        [HttpGet("{id}/generate-token")]
        public async Task<LoginResponse?> GenerateToken(int id)
        {
            var credentials = await _dbContext.FinvasiaCredentials.FindAsync(id) ?? new();

            var loginInput = new LoginRequest();
            loginInput.UserId = credentials.UserID;
            loginInput.ApkVersion = "1.0.0";
            loginInput.VendorCode = credentials.VendorCode;
            loginInput.PasswordHash = credentials.PasswordHash;
            loginInput.AppKeyHash = credentials.APIKeyHash;
            loginInput.IMEI = credentials.IMEI;
            loginInput.Source = "API";
            loginInput.Factor2Code = credentials.PAN;// new Totp(Base32Encoding.ToBytes(credentials.TOTPKey)).ComputeTotp();

            var loginResponse = await _finvasiaApiClient.LoginAsync(loginInput);
            credentials.AccessToken = loginResponse?.Susertoken ?? string.Empty;
            await _dbContext.SaveChangesAsync();

            return loginResponse;
        }
    }
}
