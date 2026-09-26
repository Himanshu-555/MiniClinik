using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Mini_Clinic_Appointment_System.Data;
using Mini_Clinic_Appointment_System.Helper;
using Mini_Clinic_Appointment_System.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mini_Clinic_Appointment_System.Repository
{
    public class AuthRepository: IAuthRepository
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly MiniClinikContext context;

        public AuthRepository(IConfiguration configuration,UserManager<ApplicationUser> userManager
            ,RoleManager<IdentityRole> roleManager,MiniClinikContext context)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }
        public async Task<(bool success, string message)> RegisterAsync(
            SignUpModel model, string role)
        {
            if (await userManager.FindByEmailAsync(model.email) != null)
                return (false, "User already exists with this email.");

            var user = new ApplicationUser
            {
                UserName = model.email,
                Email = model.email
            };

            var result = await userManager.CreateAsync(user, model.password);

            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            await userManager.AddToRoleAsync(user, role);

            if (role == "patient")
            {
                context.Patients.Add(new Patient
                {
                    UserId = user.Id,
                    name = model.name,
                    email = model.email,
                    phone = model.phone,
                    age = model.age,
                    gender = model.gender,
                    isActive = true
                });

                await context.SaveChangesAsync();
            }
            else if (role == "Doctor")
            {
                context.Doctors.Add(new Doctor
                {
                    UserId = user.Id,
                    name = model.name,
                    email = model.email,
                    phone = model.phone,
                    age = model.age,
                    gender = model.gender,
                    isActive = true
                });

                await context.SaveChangesAsync();
            }
            return (true, $"{role} registered successfully.");
        }
        public async Task<string?> LoginAsync(SignInModel model, string requiredRole)
        {
            var user = await userManager.FindByEmailAsync(model.email);

            if (user == null || !await userManager.CheckPasswordAsync(user, model.password))
                return null;

            var roles = await userManager.GetRolesAsync(user);

            if (!roles.Contains(requiredRole))
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

            var secret = configuration["Jwt:Secret"];

            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("Jwt:Secret is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var expiry = requiredRole == "Admin"
                ? DateTime.UtcNow.AddDays(2)
                : DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: expiry,
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        

        /*public async Task<(bool success,string message)> RegisterPatientAsync(SignUpModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.email);
            if (existingUser != null)
            {
                return (false, "Patient already exists with this email.");
            }
            var user = new ApplicationUser
            {
                UserName = model.email,
                Email = model.email
            };
            var result = await userManager.CreateAsync(user,model.password);
            if(!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }
            if(!await roleManager.RoleExistsAsync("patient"))
            {
                await roleManager.CreateAsync(new IdentityRole("patient"));
            }
            await userManager.AddToRoleAsync(user, "patient");

            var patient = new Patient
            {
                UserId = user.Id,
                name = model.name,
                email = model.email,
                phone = model.phone,
                age = model.age,
                gender = model.gender
            };
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
            return (true, "Patient registered successfully.");
        }*/
        /*public async Task<string?> LoginPatientAsync(SignInModel model)
        {
            var user = await userManager.FindByEmailAsync(model.email);
            if (user == null)
                return null;
            var isPasswordValid = await userManager.CheckPasswordAsync(user, model.password);
            if (!isPasswordValid)
                return null;
            var roles = await userManager.GetRolesAsync(user);

            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }
            var secret = configuration["Jwt:Secret"];
            
            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("Jwt:Secret is not configured in appsettings.json");

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/



    }
}
