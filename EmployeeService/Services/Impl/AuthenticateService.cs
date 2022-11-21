using EmployeeService.Data;
using EmployeeService.Models;
using EmployeeService.Models.Dto;
using EmployeeService.Models.Requests;
using EmployeeService.Utils;
using EmployeeServiceData;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeService.Services.Impl
{
    public class AuthenticateService : IAuthenticateService
    {
        public const string SecretCode = "kYp3s6v9y/B?E(H+";

        private readonly Dictionary<string, SessionDto> _sessions =
            new Dictionary<string, SessionDto>();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AuthenticateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public SessionDto GetSessionInfo(string sessionToken)
        {
            SessionDto? sessionDto;
            lock (_sessions)
            {
                _sessions.TryGetValue(sessionToken, out sessionDto);
            }

            if (sessionDto == null)
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                EmployeeServiceDbContext context = scope.ServiceProvider.GetRequiredService<EmployeeServiceDbContext>();

                AccountSession? session = context
                                         .AccountSessions
                                         .FirstOrDefault(item => item.SessionToken == sessionToken);
                if (session == null)
                    return null!;

                Account account = context.Accounts.FirstOrDefault(item => item.AccountId == session.AccountId)!;

                sessionDto = GetSessionDto(account, session);

                if (sessionDto != null)
                {
                    lock (_sessions)
                    {
                        _sessions[sessionToken] = sessionDto;
                    }
                }
            }

            return sessionDto!;
        }

        public AuthenticationResponse Login(AuthenticationRequest authenticationRequest)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            EmployeeServiceDbContext context = scope.ServiceProvider.GetRequiredService<EmployeeServiceDbContext>();
            Account? account =
                !string.IsNullOrWhiteSpace(authenticationRequest.Login) ?
                FindAccountByLogin(context, authenticationRequest.Login) : null;
            if (account == null)
            {

                return new AuthenticationResponse
                {
                    Status = AuthenticationStatus.UserNotFound
                };
            }

            if (!PasswordUtils.VerifyPassword(authenticationRequest.Password, account.PasswordSalt, account.PasswordHash))
            {
                return new AuthenticationResponse
                {
                    Status = AuthenticationStatus.InvalidPassword
                };

            }

            AccountSession session = new AccountSession
            {
                AccountId = account.AccountId,
                SessionToken = CreateSessionToken(account),
                TimeCreated = DateTime.Now,
                TimeClosed = DateTime.Now,
                IsClosed = false,
            };
            context.AccountSessions.Add(session);
            context.SaveChanges();

            SessionDto sessionDto = GetSessionDto(account, session);

            lock (_sessions)
            {
                _sessions[session.SessionToken] = sessionDto;
            }

            return new AuthenticationResponse
            {
                Status = AuthenticationStatus.Seccess,
                Session = sessionDto
            };
        }

        private SessionDto GetSessionDto(Account account, AccountSession accountSession)
        {
            return new SessionDto
            {
                SessionId = accountSession.SessionId,
                SessionToken = accountSession.SessionToken!,
                Account = new AccountDto
                {
                    AccountId = account.AccountId,
                    EMail = account.EMail,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondName = account.SecondName,
                    Locked = account.Locked
                }

            };
        }

        private string CreateSessionToken(Account account)//error
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretCode);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,account.AccountId.ToString()),
                    new Claim(ClaimTypes.Name,account.EMail),
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)//error

            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private Account FindAccountByLogin(EmployeeServiceDbContext context, string login)
        {
            return context
                    .Accounts
                    .FirstOrDefault(account => account.EMail == login)!;
        }

    }
}
