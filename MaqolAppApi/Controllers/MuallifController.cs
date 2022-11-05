using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MaqolAppApi.Data;
using Microsoft.EntityFrameworkCore;
using MaqolAppApi.Models;

namespace MaqolAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuallifController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MuallifController(
            DataContext dataContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mualliflar = await _dataContext.Mualliflar.ToListAsync();
            return Ok(mualliflar);
        }

        [HttpGet("{muallifId:Guid}")]
        public async Task<IActionResult> GetById(Guid muallifId)
        {
            var muallif = await _dataContext.Mualliflar
                .FirstOrDefaultAsync(muallif => muallif.MuallifId == muallifId);
            if (muallif == null)
                return NotFound(new
                {
                    status=404,
                    error="Bunday muallif yoq"
                });
            return Ok(muallif);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MuallifPost muallifPost)
        {
            Muallif muallif = new Muallif
            {
                Familya = muallifPost.Familya,
                Ism = muallifPost.Ism,
                TugilganKuni = muallifPost.TugilganKuni,
                Passport = muallifPost.Passport
            };
            if(muallifPost.Rasm != null)
            {
                var rasmKengaytmasi = muallifPost.Rasm.FileName.Split('.').Last().ToLower();
                if(rasmKengaytmasi!="png" && rasmKengaytmasi != "jpg")
                {
                    return BadRequest(new
                    {
                        status = 400,
                        error = "Iltimos jgp va png formatlarida rasm yulang!"
                    });
                }
                string fileName = Guid.NewGuid().ToString() + "." + rasmKengaytmasi;
                string filePath = Path.Combine("images", "muallif", fileName);
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                using(var stream = System.IO.File.Open(fullPath, FileMode.OpenOrCreate))
                {
                    await muallifPost.Rasm.CopyToAsync(stream);
                    muallif.RasmUrl = filePath;
                }
            }
            await _dataContext.Mualliflar.AddAsync(muallif);
            await _dataContext.SaveChangesAsync();
            return Ok(muallif);
        }


    }
}
