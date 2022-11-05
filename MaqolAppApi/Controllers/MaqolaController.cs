using MaqolAppApi.Data;
using MaqolAppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaqolAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaqolaController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MaqolaController(
            DataContext dataContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var maqolalar = await _dataContext.Maqolalar
                .Include(maqola => maqola.Muallif)
                .Select(maqola =>maqola.ToMaqolaView())
                .ToListAsync();
            return Ok(maqolalar);
        }

        [HttpGet("{maqolaId:Guid}")]
        public async Task<IActionResult> GetById(Guid maqolaId)
        {
            var maqola = await _dataContext.Maqolalar
                .FirstOrDefaultAsync(maq => maq.MaqolaId == maqolaId);
            if(maqola == null)
                return NotFound(new
                {
                    status=404,
                    error="Bunday maqola yoq!"
                });
            maqola.KurishlarSoni++;
            await _dataContext.SaveChangesAsync();
            return Ok(maqola);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MaqolaPost maqolaPost)
        {
            Maqola maqola = maqolaPost.ToMaqola();
            if (maqolaPost.Rasm != null)
            {
                string rasmKengaytmasi = maqolaPost.Rasm.FileName.Split('.').Last().ToLower();
                if(rasmKengaytmasi != "png" && rasmKengaytmasi != "jpg")
                {
                    return BadRequest(new
                    {
                        status = 400,
                        error = "Iltimos jgp va png formatlarida rasm yulang!"
                    });
                }
                string fileName = Guid.NewGuid().ToString() + '.' + rasmKengaytmasi;
                string filePath = Path.Combine("images", "maqola", fileName);
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                using(FileStream stream = System.IO.File.Open(fullPath, FileMode.OpenOrCreate))
                {
                    await maqolaPost.Rasm.CopyToAsync(stream);
                    maqola.RasmUrl = filePath;
                }
                await _dataContext.Maqolalar.AddAsync(maqola);
                await _dataContext.SaveChangesAsync(); 
            }
            return Ok(maqola);
        }

    }
}
