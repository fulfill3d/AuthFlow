using System.IdentityModel.Tokens.Jwt;
using AuthFlow.Data.Database;
using AuthFlow.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthFlow.Service
{
    public class AuthService(
        ITokenService tokenService, 
        AuthFlowDbContext dbContext): IAuthService
    {
        
        public async Task<bool> VerifyAndProcess(string code, bool update = false)
        {
            var jObject = update
                ? await tokenService.ExchangeCodeForTokenAsync(code, true)
                : await tokenService.ExchangeCodeForTokenAsync(code);
            
            if (jObject == null)
            {
                return false;
            }

            var idToken = jObject.Value<string>("id_token");
            
            if (string.IsNullOrEmpty(idToken))
            {
                return false;
            }
            
            var entity = DecodeIdToken(idToken);

            if (update)
            {
                await UpdateEntity(entity);
            }
            else
            {
                await CreateNewEntity(entity);
            }

            return true;
        }
        
        private Entity DecodeIdToken(string idToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(idToken) as JwtSecurityToken;
            
            var payload = jsonToken.Payload;

            string emailsString = "";
            if (payload.ContainsKey("emails"))
            {
                emailsString = payload["emails"].ToString() ?? string.Empty;
                if (emailsString != string.Empty)
                {
                    emailsString = emailsString.Trim('[', ']', '\n', ' ', '\r', '"');
                }
            }

            return new Entity
            {
                FirstName = payload["name"].ToString() ?? string.Empty,
                LastName = payload["given_name"].ToString() ?? string.Empty,
                RefId = Guid.Parse(payload["oid"].ToString() ?? string.Empty),
                Email = emailsString ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsEnabled = true
            };
        }

        private async Task CreateNewEntity(Entity entity)
        {
            await dbContext.Entities.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        private async Task UpdateEntity(Entity entity)
        {
            var dbEntity = await dbContext.Entities
                .FirstOrDefaultAsync(e => e.RefId == entity.RefId);

            if (dbEntity == null) return;
            
            dbEntity.UpdatedAt = DateTime.UtcNow;
            dbEntity.Email = entity.Email;
            dbEntity.FirstName = entity.FirstName;
            dbEntity.LastName = entity.LastName;

            await dbContext.SaveChangesAsync();
        }
        
    }
}