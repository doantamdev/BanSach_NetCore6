using BanSach.DataAccess.Data;
using BanSach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebBanSach.Areas.Admin.Controllers
{
     [Area("Admin")]
    public class ChatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RespostaChats
        public async Task<IActionResult> Index()
        {
            return View(await _context.RespostaChat.ToListAsync());
        }

        // GET: RespostaChats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respostaChat = await _context.RespostaChat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respostaChat == null)
            {
                return NotFound();
            }

            return View(respostaChat);
        }

        // GET: RespostaChats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RespostaChats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,Reply")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: RespostaChats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respostaChat = await _context.RespostaChat.FindAsync(id);
            if (respostaChat == null)
            {
                return NotFound();
            }
            return View(respostaChat);
        }

        // POST: RespostaChats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,Reply")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespostaChatExists(chat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: RespostaChats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respostaChat = await _context.RespostaChat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respostaChat == null)
            {
                return NotFound();
            }

            return View(respostaChat);
        }

        // POST: RespostaChats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var respostaChat = await _context.RespostaChat.FindAsync(id);
            _context.RespostaChat.Remove(respostaChat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespostaChatExists(int id)
        {
            return _context.RespostaChat.Any(e => e.Id == id);
        }


        [HttpPost("api/Chat")]
        public async Task<JsonResult> Chat(RequestApi request)
        {
            var respostaChat = await _context.RespostaChat.Where(m => m.Message.ToUpper().Contains(request.message.ToUpper())).FirstOrDefaultAsync();

            if (respostaChat != null)
            {
                var resposta = new ResponseApi { reply = respostaChat.Reply };

                return Json(resposta);
            }
            else
            {
                var resposta = new ResponseApi { reply = "Tôi không hiểu yêu cầu của bạn, vui lòng xem lại bảng gợi ý!" };
                return Json(resposta);
            }
        }
    }
}
