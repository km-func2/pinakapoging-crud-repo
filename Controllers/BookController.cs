namespace crud_redo.Controllers;

using System.Diagnostics   ;
using Microsoft.AspNetCore.Mvc;
using crud_redo.Models;
using crud_redo.Data;

public class BookController : Controller
{
    private ApplicationDbContext _db;

    public BookController(ApplicationDbContext db){
        _db = db;
    }

    public IActionResult Index(){

        var books=_db.Books.ToList();
        return View(books);
    }

    public IActionResult Create(){
        return View(); 
    }
    [HttpPost]
    public IActionResult CreateBook(BooksEntity book){
        _db.Books.Add(book);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        // return View();
         var book=_db.Books.Find(id);
        if(book==null){
            return NotFound();
        }
         _db.Books.Remove(book);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var books=_db.Books.Find(id);
        if(books==null){
            return NotFound();
        }
        return View(books);
        // return View();
        // return RedirectToAction("Create");
    }
    
    [HttpPost]
    public IActionResult EditBook(BooksEntity book){
        _db.Books.Update(book);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
 
}