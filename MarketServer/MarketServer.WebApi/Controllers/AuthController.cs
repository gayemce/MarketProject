using AutoMapper;
using MarketServer.WebApi.Context;
using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserModel = MarketServer.WebApi.Models.User;

namespace MarketServer.WebApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper  _mapper;

    public AuthController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Register(RegisterDto request)
    {
        UserModel user = _mapper.Map<UserModel>(request);

        _context.Add(user);
        _context.SaveChanges();

        return Ok(new {Message = "Kayit işlemi başarıyla tamamlandı" });
    }

    [HttpPost]
    public IActionResult Login(LoginDto request)
    {
        UserModel user = _context.Users.Where(p => p.Username == request.UsernameOrEmail || p.Email == request.UsernameOrEmail).FirstOrDefault();

        if(user is null)
        {
            return BadRequest(new { Message = "Kullanıcı Bulunamadı!" });
        }

        if(user.Password != request.Password)
        {
            return BadRequest(new { Message = "Şifre Yanlış!" });
        }

        string token = JwtService.CreatToken(user);

        return Ok(new LoginResponseDto(Token: token, UserId: user.Id, UserName: user.GetName()));
    }


}
